
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
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Character zombie = base.CharacterSpawn();
            ZombieSetting(zombie);
        }
    }
    private void ZombieSetting(Character zombie)
    {
        if (GameManager.Instance.zombieCityData != null&&GameManager.Instance.zombieCityData.ZombieCount<100)
        {
            zombie = zombie.GetComponent<Zombie>();
            zombie.health += GameManager.Instance.zombieCityData.HealthLevel;
            zombie.attackPower += GameManager.Instance.zombieCityData.AttackLevel;
            zombie.RespawnSetting();
            GameManager.Instance.zombieCityData.ZombieCountUpdate(1);
        }
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnZombies());
    }
}
