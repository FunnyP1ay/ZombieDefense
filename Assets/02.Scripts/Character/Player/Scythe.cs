using UnityEngine;

public class Scythe : Weapon
{
    public LayerMask FarmLayer;
    public override void UsingEvent(Transform player)
    {
        print("³´À» ÈÖµÑ·¶½À´Ï´Ù.");
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, FarmLayer);
        if(colliders.Length > 0)
        {
            GameManager.Instance.playerCityData.UsingMoney(1);
            GameManager.Instance.playerCityData.SPYScoreUpdate(1);
        }
    }
}
