using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Lean.Pool;
public class GunnerHuman : Character
{
    private WaitForSecondsRealtime m_time1 = new WaitForSecondsRealtime(1f);
    private WaitForSecondsRealtime m_time2 = new WaitForSecondsRealtime(1.1f);
    private WaitForSecondsRealtime m_attackTime = new WaitForSecondsRealtime(0.5f);
    private WaitForSecondsRealtime m_currentTime;
    public GameObject moveTarget;
    public ParticleSystem fireEffect;
    private NavMeshAgent agent;
    private float m_detectionRadius = 10f;

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
        target = null;
        moveTarget = null;
        m_currentTime = m_time2;
        m_animator.SetBool("Attack", false);
        StartCoroutine(AIState());
    }
    private IEnumerator AIState()
    {
        while (true)
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
            yield return m_currentTime;
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
        // 맨처음 웨이포인트 조차 없을 때 , 거리가 2f 보다 가까울 때
        if (moveTarget == null || Vector3.Distance(this.transform.position, moveTarget.transform.position) < 2f)
        {
            int randNum = Random.Range(0, GameManager.Instance.playerCityData.wayPointList.Count);
            moveTarget = GameManager.Instance.playerCityData.wayPointList[randNum];
            agent.SetDestination(moveTarget.transform.position);
        }
    }
    public void CheckTarget()
    {
        // 타겟 조차 없을 때 , 타겟이 죽어서 비활성화 일 때 , 사거리 보다 멀어졌을 때.
        if (target == null || target.activeSelf == false || Vector3.Distance(this.transform.position, target.transform.position) > attackRange)
        {

            Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRadius, targetLayer);
            if (colliders.Length > 0)
            {
                state = State.Attack;
                agent.speed = 0;
                fireEffect.Play();
                m_animator.SetBool("Attack", true);
                moveTarget = null;
                m_currentTime = m_attackTime;
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
        if (target != null && target.activeSelf && Vector3.Distance(this.transform.position, target.transform.position) <= attackRange)
        {
            target.gameObject.GetComponent<Character>().TakeDamage(attackPower);
            agent.ResetPath();
            transform.LookAt(target.transform);
            fireEffect.Play();
            m_animator.SetBool("Attack",true);
            base.Attack();
        }
    }
 
  
    public void NextCoroutineTime()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
            m_currentTime = m_time1;
        else
            m_currentTime = m_time2;
    }
    protected override void Die()
    {
        agent.ResetPath();
        StopCoroutine(AIState());
        target = null;
        GameManager.Instance.playerCityData.PlayerTeamCountUpdate(-1);
        LeanPool.Despawn(this.gameObject);
    }

    private void OnDisable()
    {
    }
}
