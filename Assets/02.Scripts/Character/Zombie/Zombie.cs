using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Lean.Pool;

public class Zombie : Character
{
    private WaitForSecondsRealtime m_time1 = new WaitForSecondsRealtime(1f);
    private WaitForSecondsRealtime m_time2 = new WaitForSecondsRealtime(1.1f);
    private WaitForSecondsRealtime m_currentTime;
    private NavMeshAgent agent;
    public GameObject baseTarget;
    public int m_moveSupportCount = 0;
    private float m_detectionRadius = 10f;
    private bool m_isDie = false;
    
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
        m_currentTime = m_time2;
        StartCoroutine(AIState());
    }
    private IEnumerator AIState()
    {
        while (true)
        {
            Move();
            yield return m_currentTime;
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
        // 맨처음 타겟 조차 없을 때 , 타겟이 죽어서 비활성화 일 때 , 거리가 5f 보다 멀어졌을 때.
        if (target == null || target.activeSelf == false || Vector3.Distance(this.transform.position, target.transform.position) > 7f)
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
        if (target != null && Vector3.Distance(this.transform.position, target.transform.position) <= attackRange)
        {
            target.gameObject.GetComponent<Character>().TakeDamage(attackPower);
            m_animator.SetTrigger("Attack");
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
        if (!m_isDie)
        {
            m_isDie = true;
            StopCoroutine(AIState());
            agent.ResetPath();
            target = null;
            base.Die();
        }

    }
    private void OnDisable()
    {
    }
}
