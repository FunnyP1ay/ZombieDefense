using UnityEngine;

public class FarmAsset : MonoBehaviour
{
    public bool isBuildArea = true;
    public bool BuildFinish = false;
    [Header("자원 증가 수치")]
    public int FoodValue = 1;
    public LayerMask CanNotBuildLayer; // 설치 불가 레이어
    public LayerMask BuildLayer;
    Renderer renderer;
    Color color;

    private void OnEnable()
    {
        BuildFinish = false;
    }

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        color = renderer.material.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if (BuildFinish == false)
        {
            // 충돌한 오브젝트의 레이어가 CanNotBuildLayer에 포함되어 있는지 체크
            if ((CanNotBuildLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                print($"지을 수 없는 곳 (Layer: {other.gameObject.layer})");
                color = Color.red;
                color.a = 0.5f;
                isBuildArea = false;
                renderer.material.color = color;
            }
            else
            {
                print($"지을 수 있는 곳 (Layer: {other.gameObject.layer})");
                color = Color.white;
                color.a = 0.5f;
                isBuildArea = true;
                renderer.material.color = color;
            }
        }
    }

    public virtual void BuildObject(FarmAsset prefab)
    {
        FarmAsset farmObject = Instantiate(prefab);
        Renderer farmObjectrenderer = farmObject.GetComponent<Renderer>();
        Color color = farmObjectrenderer.material.color;
        color.a = 1;
        farmObject.BuildFinish = true;
        Rigidbody rb = farmObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
        GameManager.Instance.playerCityData.CityTax += FoodValue;
    }
}
