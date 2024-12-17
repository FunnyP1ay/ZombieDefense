using Cysharp.Threading.Tasks; // UniTask ���ӽ����̽�
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
    public GameObject DiePrefab;
    public int m_moveSupportCount = 0;
    private float m_detectionRadius = 10f;
    private bool m_isDie = false;
    private bool m_isPlayerTarget = false;
    public ParticleSystem BloodParticle;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    public override void RespawnSetting()
    {
        health += 5;
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

                // m_currentTime ��ŭ ��� (�и��ʷ� ��ȯ)
                await UniTask.Delay((int)(m_currentTime * 1000), cancellationToken: cancellationToken);
            }
        }
        catch
        {
        }
    }

    public override void Move()
    {
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
                if (target.name == "Player")
                    m_isPlayerTarget = true;
                else
                    m_isPlayerTarget = false;

            }
            else
            {
                print("���� �׳� ������ �����մϴ�.");
                agent.SetDestination(baseTarget.transform.position);
            }
        }
        else
        {
            if (m_isPlayerTarget)
                agent.SetDestination(target.transform.position);
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
    public override void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            BloodParticle.Play();
        }
        base.TakeDamage(damage);
    }
    protected override void Die()
    {
        if (!m_isDie)
        {
            m_isDie = true;
            // _cancellationTokenSource�� �̹� Dispose �Ǿ����� Ȯ�� �� ���
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null; // �ߺ� ȣ�� ������ ���� null�� ����
            }
            agent.ResetPath();
            target = null;
            GameManager.Instance.zombieCityData.ZombieCountUpdate(-1);
            base.Die();
        }
    }

}