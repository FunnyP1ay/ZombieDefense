using Cysharp.Threading.Tasks; // UniTask 네임스페이스
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class UnitaskZombie : Character
{
    private static readonly float Time1 = 1f;
    private static readonly float Time2 = 1.1f;
    private float m_currentTime;
    private NavMeshAgent agent;
    public GameObject baseTarget;
    public int m_moveSupportCount = 0;
    private float m_detectionRadius = 10f;
    private bool m_isDie = false;

    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    public override void RespawnSetting()
    {
        baseTarget = GameManager.Instance.playerCityData.playerBase;
        m_isDie = false;
        target = null;
        agent.SetDestination(baseTarget.transform.position);
        m_currentTime = Time2;

        _cancellationTokenSource = new CancellationTokenSource();
        AIStateAsync(_cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid AIStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Move();

                // m_currentTime 만큼 대기 (밀리초로 변환)
                await UniTask.Delay((int)(m_currentTime * 1000), cancellationToken: cancellationToken);
            }
        }
        catch
        {
            // 코루틴이 취소되었을 때의 처리
        }
    }

    public override void Move()
    {
        MoveSupport();
        SettingTarget();
        Attack();
        NextCoroutineTime();
    }

    public void SettingTarget()
    {
        if (target == null || !target.activeSelf || Vector3.Distance(transform.position, target.transform.position) > 7f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRadius, targetLayer);
            if (colliders.Length > 0)
            {
                int randnum = Random.Range(0, colliders.Length);
                target = colliders[randnum].gameObject;
                agent.SetDestination(target.transform.position);
            }
        }
    }

    public void MoveSupport()
    {
        m_moveSupportCount++;
        if (m_moveSupportCount > 3)
        {
            target = null;
            m_moveSupportCount = 0;
            agent.SetDestination(baseTarget.transform.position);
        }
    }

    public override void Attack()
    {
        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            target.GetComponent<Character>().TakeDamage(attackPower);
            m_animator.SetTrigger("Attack");
        }
    }

    public void NextCoroutineTime()
    {
        m_currentTime = Random.Range(0, 2) == 0 ? Time1 : Time2;
    }

    protected override void Die()
    {
        if (!m_isDie)
        {
            m_isDie = true;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            agent.ResetPath();
            target = null;
            GameManager.Instance.zombieCityData.ZombieCountUpdate(-1);
            base.Die();
        }
    }

    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}