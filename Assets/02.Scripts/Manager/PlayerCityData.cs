using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCityData : MonoBehaviour
{
    public GameObject playerBase;
    public List<GameObject> wayPointList;
    public int CityMoney = 0;
    public int HealthLevel = 0;
    public int AttackLevel = 0;
    public int PlayerTeamCount = 0;
    public TMP_Text UI_PlayerTeamCount;
    private void Awake()
    {
        GameManager.Instance.playerCityData = this;
    }
    public void PlayerTeamCountUpdate(int value)
    {
        PlayerTeamCount += value;
        UI_PlayerTeamCount.text = PlayerTeamCount.ToString();
    }
}
