using UnityEngine;
using TMPro;

public class ZombieCityData : MonoBehaviour
{

    public int HealthLevel = 0;
    public int AttackLevel = 0;
    public int ZombieCount = 0;
    //TODO UI ¼Õº¸±â
    private void Awake()
    {
        GameManager.Instance.zombieCityData = this;
    }
}
