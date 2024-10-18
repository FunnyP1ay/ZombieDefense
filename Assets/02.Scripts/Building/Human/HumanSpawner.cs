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
        while (true)
        {
            CityMoneyCheck();
            yield return new WaitForSeconds(2f);
        }
    }
    private void CityMoneyCheck()
    {
        if (GameManager.Instance.playerCityData != null && GameManager.Instance.playerCityData.CityMoney >= 50)
        {
            GameManager.Instance.playerCityData.CityMoney -= 50;
            Character human = base.CharacterSpawn();
            HumanSetting(human);
        }
    }
    private void HumanSetting(Character human)
    {
        human.health += GameManager.Instance.playerCityData.HealthLevel;
        human.attackPower += GameManager.Instance.playerCityData.AttackLevel;
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnHuman());
    }
}
