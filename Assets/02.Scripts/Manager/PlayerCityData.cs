using UnityEngine;

public class PlayerCityData : MonoBehaviour
{
    public GameObject playerBase;
    public int CityMoney = 0;
    public int HealthLevel = 0;
    public int AttackLevel = 0;
    private void Awake()
    {
        GameManager.Instance.playerCityData = this;
    }
}
