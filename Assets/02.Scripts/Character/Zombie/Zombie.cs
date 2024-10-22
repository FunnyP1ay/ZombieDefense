using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Character
{
    private WaitForSecondsRealtime oneSecond = new WaitForSecondsRealtime(1f);
    public GameObject baseTarget;
    private NavMeshAgent agent;
    private float m_detectionRadius = 10f;
    public int m_moveBugFixCount = 0;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
    }
    public override void RespawnSetting()
    {
        baseTarget = GameManager.Instance.playerCityData.playerBase;
        target = null;
        agent.SetDestination(baseTarget.transform.position);
        StartCoroutine(AIState());
    }
    private IEnumerator AIState()
    {
        while (true)
        {
            Move();
            yield return oneSecond;
        }
    }
    public override void Move()
    {
        MoveSupport();
        SettingTarget();
        Attack();
        base.Move();
    }
    public void SettingTarget()
    {
        // 맨처음 타겟 조차 없을 때 , 타겟이 죽어서 비활성화 일 때 , 거리가 5f 보다 멀어졌을 때.
        if (target == null || target.activeSelf == false || Vector3.Distance(this.transform.position, target.transform.position) > 5f)
        {
           
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_detectionRadius, targetLayer);
            if (colliders.Length > 0)
            {
                print("플레이어를 찾았습니다!");
                int randnum = Random.Range(0, colliders.Length);
                target = colliders[randnum].gameObject;
                agent.SetDestination(target.transform.position);
            }
        }

    }
    public void MoveSupport()
    {
        m_moveBugFixCount++;
        if (m_moveBugFixCount > 3)
        {
            target = null;
            m_moveBugFixCount = 0;
            agent.SetDestination(baseTarget.transform.position);
        }
    }
    public override void Attack()
    {
        if (target != null && Vector3.Distance(this.transform.position, target.transform.position) <= 1f)
        {
            target.gameObject.GetComponent<Character>().TakeDamage(1f);
            m_animator.SetTrigger("Attack");
            base.Attack();
        }
    }
    protected override void Die()
    {
        agent.ResetPath();
        StopCoroutine(AIState());
        target = null;
        base.Die();
    }
    private void OnDisable()
    {
    }
}
