using UnityEngine;

public class FarmAsset : MonoBehaviour
{
    public bool isBuildArea = true;
    public bool BuildFinish = false;
    [Header("자원 증가 수치")]
    public int MoneyValue = 1;
    [Header("설치비용")]
    public int BuildPrice;
    public LayerMask CanNotBuildLayer; // 설치 불가 레이어
    public LayerMask BuildLayer;
    public ParticleSystem BuildEffect;
    private Renderer renderer;
    private Color originalColor;

    // 설치 불가 감지 반경
    public float checkRadius = 1f;

    private void OnEnable()
    {
        BuildFinish = false;
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public bool CheckBuildArea()
    {
        // 설치 가능 여부를 초기화
        isBuildArea = true;

        // 반경 내 설치 불가 오브젝트가 있는지 확인
        Collider[] colliders = Physics.OverlapSphere(renderer.bounds.center, checkRadius, CanNotBuildLayer);
        if (colliders.Length > 1)
        {
            isBuildArea = false;
            SetMaterialColor(Color.red, 0.5f);
        }
        else
        {
            SetMaterialColor(Color.green, 0.5f);
        }
        print(colliders.Length);

        return isBuildArea;
    }

    private void SetMaterialColor(Color color, float alpha)
    {
        color.a = alpha;
        renderer.material.color = color;
    }

    public virtual void BuildObject(FarmAsset prefab)
    {
        FarmAsset farmObject = Instantiate(prefab);
        Renderer farmObjectRenderer = farmObject.GetComponent<Renderer>();
        Color color = farmObjectRenderer.material.color;
        color.a = 1;
        farmObjectRenderer.material.color = color;
        farmObject.BuildFinish = true;
        Rigidbody rb = farmObject.GetComponent<Rigidbody>();
        farmObject.GetComponent<Collider>().isTrigger = false;
        if (rb != null)
        {
            Destroy(rb);
        }
        farmObject.BuildEffect.Play();
        GameManager.Instance.playerCityData.PlayerTaxUpdate(MoneyValue);
        GameManager.Instance.playerCityData.UsingMoney(BuildPrice);
    }
}