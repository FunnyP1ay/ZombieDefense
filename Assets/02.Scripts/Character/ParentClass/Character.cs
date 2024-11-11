using UnityEngine;
using Lean.Pool;

public class Character : MonoBehaviour
{
    public float health;
    public float attackPower;
    public float attackRange;
    public GameObject target;
    public LayerMask targetLayer;
    protected Animator m_animator;


    public enum TeamValue
    {
        PlayerTeam,
        Zombie,
        EnemyHuman
    }
    public TeamValue Team;

    public virtual void RespawnSetting()
    {

    }
    public virtual void Move()
    {

    }
    public virtual void Attack()
    {


        
    }
    public virtual void TakeDamage(float damage)
    {
        // 데미지를 입었습니다.
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "이/가 죽었습니다.");
        health = 0;
        LeanPool.Despawn(this.gameObject);
        // 애니매이션 실행시키는 부분
    }

}
