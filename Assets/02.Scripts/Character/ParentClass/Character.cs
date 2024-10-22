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

        Debug.Log(gameObject.name + "��/�� " + attackPower + " ��ŭ �����߽��ϴ�.");
        
    }
    public void TakeDamage(float damage)
    {
        // �������� �Ծ����ϴ�.
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + "��/�� �׾����ϴ�.");
        LeanPool.Despawn(this.gameObject);
        // �ִϸ��̼� �����Ű�� �κ�
    }

}
