
using UnityEngine;
using System.Collections;

public class ZombieSpawner : CharacterSpawner
{
    private void OnEnable()
    {
        StartCoroutine(SpawnZombies());
    }
    private IEnumerator SpawnZombies()
    {
        while (true) // 무한 루프
        {
            Character zombie = base.CharacterSpawn();
            ZombieSetting(zombie);
           yield return new WaitForSeconds(2f);
        }
    }
    private void ZombieSetting(Character zombie)
    {
        zombie.health += GameManager.Instance.zombieCityData.HealthLevel;
        zombie.attackPower += GameManager.Instance.zombieCityData.AttackLevel;
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnZombies());
    }
}
