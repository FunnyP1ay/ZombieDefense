using Cysharp.Threading.Tasks; // UniTask 네임스페이스
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class UnitaskGunnerHuman : Character
{
    private static readonly float Time1 = 1f;
    private static readonly float Time2 = 1.1f;
    private static readonly float AttackTime = 0.5f;
    private float m_currentTime;

    public GameObject moveTarget;
    public ParticleSystem fireEffect;
    private NavMeshAgent agent;
    private float m_detectionRadius = 10f;
    private bool m_isDie = false;

    private CancellationTokenSource _cancellationTokenSource;

    public enum State
    {
        Move,
        Attack
    }
    public State state = State.Move;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }

    public override void RespawnSetting()
    {
        m_isDie = false;
        target = null;
        moveTarget = null;
        state = State.Move;
        m_currentTime = Time2;
        m_animator.SetBool("Attack", false);
        _cancellationTokenSource = new CancellationTokenSource();
        AIStateAsync(_cancellationTokenSource.Token).Forget(); // 비동기 메서드 실행
    }

    private async UniTaskVoid AIStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                switch (state)
                {
                    case State.Move:
                        Move();
                        break;
                    case State.Attack:
                        Attack();
                        break;
                }

                // m_currentTime 동안 대기 (밀리초 단위 변환)
                await UniTask.Delay((int)(m_currentTime * 1000), cancellationToken: cancellationToken);
            }
        }
        catch 
        {
            // 코루틴이 취소되었을 때의 처리 (필요 시)
        }
    }

    public override void Move()
    {
        CheckMove();
        CheckTarget();
        NextCoroutineTime();
    }

    public override void Attack()
    {
        CheckTarget();
        CheckAttack();
    }

    private void CheckMove()
    {
        if (moveTarget == null || Vector3.Distance(transform.position, moveTarget.transform.position) < 2f)
        {
            int randNum = Random.Range(0, GameManager.Instance.playerCityData.wayPointList.Count);
            moveTarget = GameManager.Instance.playerCityData.wayPointList[randNum];
            agent.SetDestination(moveTarget.transform.position);
        }
    }

    public void CheckTarget()
    {
        if (target == null || !target.activeSelf || Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRadius, targetLayer);
            if (colliders.Length > 0)
            {
                state = State.Attack;
                agent.speed = 0;
                fireEffect.Play();
                m_animator.SetBool("Attack", true);
                moveTarget = null;
                m_currentTime = AttackTime;

                int randnum = Random.Range(0, colliders.Length);
                target = colliders[randnum].gameObject;
            }
            else
            {
                state = State.Move;
                agent.speed = 3.5f;
                m_animator.SetBool("Attack", false);
            }
        }
    }

    public void CheckAttack()
    {
        if (target != null && target.activeSelf && Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            target.GetComponent<Character>().TakeDamage(attackPower);
            agent.ResetPath();
            transform.LookAt(target.transform);
            fireEffect.Play();
            m_animator.SetBool("Attack", true);
            base.Attack();
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
            GameManager.Instance.playerCityData.PlayerTeamCountUpdate(-1);
            base.Die();
        }
    }

    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}