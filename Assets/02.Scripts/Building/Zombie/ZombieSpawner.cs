
using UnityEngine;
using System.Collections;

public class ZombieSpawner : CharacterSpawner
{
    private WaitForSecondsRealtime m_twoSeccond = new WaitForSecondsRealtime(2);
    private void OnEnable()
    {
        StartCoroutine(SpawnZombies());
    }
    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return m_twoSeccond;
            if (GameManager.Instance.zombieCityData != null && GameManager.Instance.zombieCityData.ZombieCount < 200)
            {
                Character zombie = base.CharacterSpawn();
                ZombieSetting(zombie);
            }

        }
    }
    private void ZombieSetting(Character zombie)
    {
        zombie = zombie.GetComponent<UnitaskZombie>();
        zombie.health += GameManager.Instance.zombieCityData.HealthLevel;
        zombie.attackPower += GameManager.Instance.zombieCityData.AttackLevel;
        zombie.RespawnSetting();
        GameManager.Instance.zombieCityData.ZombieCountUpdate(1);
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnZombies());
    }
}
