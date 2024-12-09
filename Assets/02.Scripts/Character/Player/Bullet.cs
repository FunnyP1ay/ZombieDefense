using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Character character)&& (character.Team == Character.TeamValue.Zombie|| character.Team == Character.TeamValue.EnemyHuman))
        {
            character.TakeDamage(Damage);
            print($"{character.name}���� ������ {Damage}�� ����ϴ�!");
            Lean.Pool.LeanPool.Despawn(gameObject);
        }
    }
}
