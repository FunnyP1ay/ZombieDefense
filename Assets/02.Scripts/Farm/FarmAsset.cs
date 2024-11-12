using UnityEngine;

public class FarmAsset : MonoBehaviour
{
    public bool isBuildArea = true;
    public bool BuildFinish = false;
    [Header("�ڿ� ���� ��ġ")]
    public int FoodValue = 1;
    public LayerMask CanNotBuildLayer; // ��ġ �Ұ� ���̾�
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
            // �浹�� ������Ʈ�� ���̾ CanNotBuildLayer�� ���ԵǾ� �ִ��� üũ
            if ((CanNotBuildLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                print($"���� �� ���� �� (Layer: {other.gameObject.layer})");
                color = Color.red;
                color.a = 0.5f;
                isBuildArea = false;
                renderer.material.color = color;
            }
            else
            {
                print($"���� �� �ִ� �� (Layer: {other.gameObject.layer})");
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
