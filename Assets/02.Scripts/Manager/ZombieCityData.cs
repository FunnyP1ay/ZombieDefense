using UnityEngine;

public class ZombieCityData : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.zombieCityData = this;
    }
    public int HealthLevel = 0;
    public int AttackLevel = 0;
}
