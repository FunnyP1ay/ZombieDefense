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
    public TMP_Text UI_CityMoney;
    [Header("도시 수입")]
    public int CityTax = 0;
    public TMP_Text UI_CityTax;
    [Header("도시 유닛 업그레이드 현황")]
    public int HealthLevel = 0;
    public int AttackLevel = 0;
    [Header("도시 방어 유닛 수")]
    public int PlayerTeamCount = 0;
    public TMP_Text UI_CityUnitCount;
    private bool isRunning = true;

    [Header("스파이 점수")]
    public int SPYScore = 0;
    public TMP_Text UI_SPYScore;
    private void Awake()
    {
        GameManager.Instance.playerCityData = this;
    }
    private void Start()
    {
        StartIncrementing();
    }
    public void UIUpdate(int value, TMP_Text UItext)
    {
        UItext.text = value.ToString();
    }
    public void UIUpdate(float value, TMP_Text UItext)
    {
        UItext.text = value.ToString();
    }
    private async void StartIncrementing()
    {
        while (isRunning)
        {
            await UniTask.Delay(2000); 
            CityMoney += CityTax;
            UIUpdate(CityMoney, UI_CityMoney);
        }
    }

    public void PlayerTeamCountUpdate(int value)
    {
        PlayerTeamCount += value;
        UIUpdate(PlayerTeamCount, UI_CityUnitCount);
    }
    public void UsingMoney(int value)
    {
        CityMoney -= value;
        UIUpdate(CityMoney, UI_CityMoney);
    }
    public void PlayerTaxUpdate(int value)
    {
        CityTax += value;
        UIUpdate(CityTax, UI_CityTax);
    }
    public void SPYScoreUpdate(int value)
    {
        SPYScore += value;
        UIUpdate(SPYScore, UI_SPYScore);
    }

    private void OnDisable()
    {
        isRunning = false; // 루프 종료
    }

}
