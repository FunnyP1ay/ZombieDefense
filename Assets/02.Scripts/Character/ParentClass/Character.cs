using UnityEngine;
using Lean.Pool;

public class Character : MonoBehaviour
{
    public float health;
    public float speed;
    public float attackPower;
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

        Debug.Log(gameObject.name + "이/가 " + attackPower + " 만큼 공격했습니다.");
        
    }
    public void TakeDamage(float damage)
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
        LeanPool.Despawn(this.gameObject);
        // 애니매이션 실행시키는 부분
    }

}
