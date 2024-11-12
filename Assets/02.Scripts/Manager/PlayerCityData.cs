using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlayerCityData : MonoBehaviour
{
    public GameObject playerBase;
    public List<GameObject> wayPointList;
    [Header("���� �ڿ� ��Ȳ")]
    public int CityMoney = 0;
    public int FoodCount = 0;
    [Header("���� ����")]
    public int CityTax = 0;
    [Header("���� ���� ���׷��̵� ��Ȳ")]
    public int HealthLevel = 0;
    public int AttackLevel = 0;
    [Header("���� ��� ���� ��")]
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
        isRunning = false; // ���� ����
    }

}
