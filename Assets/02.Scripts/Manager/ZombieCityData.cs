using UnityEngine;
using TMPro;

public class ZombieCityData : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.zombieCityData = this;
    }
    public int HealthLevel = 0;
    public int AttackLevel = 0;
    public int ZombieCount = 0;
    //TODO UI ¼Õº¸±â
    public TMP_Text text;
    public void ZombieCountUpdate(int value)
    {
        ZombieCount +=value;
        text.text = ZombieCount.ToString();
    }
}
