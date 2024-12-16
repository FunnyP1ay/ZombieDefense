using UnityEngine;

public class Scythe : Weapon
{
    public override void UsingEvent(Transform player)
    {
        print("낫을 휘둘렀습니다.");
        // 향후 근처 밭이 있을 때 독일에 식량 제공 하는 기능 추가
    }
}
