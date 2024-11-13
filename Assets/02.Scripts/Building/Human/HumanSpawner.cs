using UnityEngine;
using System.Collections;

public class HumanSpawner : CharacterSpawner
{

    public void SpawnGunner()
    {
        if (GameManager.Instance.playerCityData != null && GameManager.Instance.playerCityData.CityMoney >= 50)
        {
            GameManager.Instance.playerCityData.UsingMoney(50);
            Character gunnerHuman = base.CharacterSpawn();
            HumanSetting(gunnerHuman);
        }
    }
    private void HumanSetting(Character gunnerHuman)
    {
        gunnerHuman.GetComponent<UnitaskGunnerHuman>();
        gunnerHuman.health += GameManager.Instance.playerCityData.HealthLevel;
        gunnerHuman.attackPower += GameManager.Instance.playerCityData.AttackLevel;
        gunnerHuman.RespawnSetting();
        GameManager.Instance.playerCityData.PlayerTeamCountUpdate(1);
    }

}
