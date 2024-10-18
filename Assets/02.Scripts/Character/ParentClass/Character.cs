using UnityEngine;
using Lean.Pool;

public class Character : MonoBehaviour
{
    public float health;
    public float speed;
    public float attackPower;
    protected Rigidbody m_rigidbody;
    protected Animator m_animator;
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
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
    }

}
