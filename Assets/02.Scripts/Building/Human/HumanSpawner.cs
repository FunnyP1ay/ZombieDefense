using UnityEngine;
using System.Collections;

public class HumanSpawner : CharacterSpawner
{
    private void OnEnable()
    {
        StartCoroutine(SpawnHuman());
    }
    private IEnumerator SpawnHuman()
    {
        while (true) // 무한 루프
        {
            Character human = base.CharacterSpawn();
            HumanSetting(human);
            yield return new WaitForSeconds(2f);
        }
    }
    private void HumanSetting(Character human)
    {
        if (GameManager.Instance.playerCityData != null)
        {
            human.health += GameManager.Instance.playerCityData.HealthLevel;
            human.attackPower += GameManager.Instance.playerCityData.AttackLevel;
        }
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnHuman());
    }
}
