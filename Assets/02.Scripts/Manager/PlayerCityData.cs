using UnityEngine;

public class PlayerCityData : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.playerCityData = this;
    }
    public int CityMoney = 0;
    public int HealthLevel = 0;
    public int AttackLevel = 0;
}
