using Cysharp.Threading.Tasks; // UniTask 네임스페이스
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class UnitaskGunnerHuman : Character ,Imission
{
    protected static readonly float Time1 = 1f;
    protected static readonly float Time2 = 1.21f;
    protected static readonly float AttackTime = 0.5f;
    protected float m_currentTime;

    public GameObject moveTarget;
    public ParticleSystem fireEffect;
    protected NavMeshAgent agent;
    protected float m_detectionRadius = 10;
    protected bool m_isDie = false;
    public VisualEffect BloodVFX;
    private int triggerID = 0; // 트리거 ID로 고유 이벤트 식별

    protected CancellationTokenSource _cancellationTokenSource;

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
        m_detectionRadius = attackRange * 2f;
    }
    public virtual void MissionStart()
    {
        RespawnSetting();
    }
    public override void RespawnSetting()
    {
        m_isDie = false;
        target = null;
        moveTarget = null;
        state = State.Move;
        m_currentTime = Time2;
        health += 10;
        m_animator.SetBool("Attack", false);
        m_animator.SetFloat("Speed", 0);
        _cancellationTokenSource = new CancellationTokenSource();
        AIStateAsync(_cancellationTokenSource.Token).Forget(); // 비동기 메서드 실행
    }

    protected async UniTaskVoid AIStateAsync(CancellationToken cancellationToken)
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
                m_animator.SetFloat("Speed", agent.speed);
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
                int randnum = Random.Range(0, colliders.Length);
                target = colliders[randnum].gameObject;
                target.GetComponent<Character>().TakeDamage(attackPower);
                agent.ResetPath();
                transform.LookAt(target.transform);
                agent.speed = 0;
                fireEffect.Play();
                m_animator.SetBool("Attack", true);
                moveTarget = null;
                m_currentTime = AttackTime;

            }
            else
            {
                state = State.Move;
                agent.speed = 3.5f;
                m_animator.SetBool("Attack", false);
                CheckMove();
            }
        }
    }

    public void CheckAttack()
    {
        if (target != null && target.activeSelf && Vector3.Distance(transform.position, target.transform.position) <= attackRange && target.GetComponent<Character>().health > 0)
        {

            target.GetComponent<Character>().TakeDamage(attackPower);
            agent.ResetPath();
            transform.LookAt(target.transform);
            fireEffect.Play();
            m_animator.SetBool("Attack", true);
            base.Attack();
        }
        else
        {
            state = State.Move;
            agent.speed = 3.5f;
            m_animator.SetBool("Attack", false);
            CheckMove();
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
            if (BloodVFX != null)
            {
                // 고유 트리거 이벤트 생성
                BloodVFX.SetInt("TriggerID", triggerID);
                BloodVFX.SendEvent("OnTrigger");

                // 트리거 ID 업데이트
                triggerID++;
            }
        }
        base.TakeDamage(damage);
    }
    protected override void Die()
    {
        if (!m_isDie)
        {
            m_isDie = true;
            // _cancellationTokenSource가 이미 Dispose 되었는지 확인 후 취소
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null; // 중복 호출 방지를 위해 null로 설정
            }
            agent.ResetPath();
            target = null;
            GameManager.Instance.playerCityData.PlayerTeamCountUpdate(-1);
            m_animator.SetTrigger("Die");
        }
    }
    public void OnDieAnimationEnd()
    {
        base.Die();
    }

}