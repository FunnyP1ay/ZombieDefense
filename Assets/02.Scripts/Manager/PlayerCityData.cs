using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlayerCityData : MonoBehaviour
{
    public GameObject playerBase;
    public List<GameObject> wayPointList;
    [Header("도시 자원 현황")]
    public int CityMoney = 0;
    public int FoodCount = 0;
    [Header("도시 수입")]
    public int CityTax = 0;
    [Header("도시 유닛 업그레이드 현황")]
    public int HealthLevel = 0;
    public int AttackLevel = 0;
    [Header("도시 방어 유닛 수")]
    public int PlayerTeamCount = 0;
    public TMP_Text UI_PlayerTeamCount;

    private bool isRunning = true;
    private void Awake()
    {
        GameManager.Instance.playerCityData = this;
    }
    private void Start()
    {
        StartIncrementing();
    }
    private async void StartIncrementing()
    {
        while (isRunning)
        {
            await UniTask.Delay(2000); 
            CityMoney += CityTax;
        }
    }
    public void PlayerTeamCountUpdate(int value)
    {
        PlayerTeamCount += value;
        UI_PlayerTeamCount.text = PlayerTeamCount.ToString();
    }
    public void GiveCityMoney() 
    {
        CityMoney += 50;
    }
    private void OnDisable()
    {
        isRunning = false; // 루프 종료
    }

}
