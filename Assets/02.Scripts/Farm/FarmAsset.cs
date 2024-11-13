using UnityEngine;

public class FarmAsset : MonoBehaviour
{
    public bool isBuildArea = true;
    public bool BuildFinish = false;
    [Header("�ڿ� ���� ��ġ")]
    public int MoneyValue = 1;
    [Header("��ġ���")]
    public int BuildPrice;
    public LayerMask CanNotBuildLayer; // ��ġ �Ұ� ���̾�
    public LayerMask BuildLayer;
    public ParticleSystem BuildEffect;
    private Renderer renderer;
    private Color originalColor;

    // ��ġ �Ұ� ���� �ݰ�
    public float checkRadius = 1f;

    private void OnEnable()
    {
        BuildFinish = false;
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public bool CheckBuildArea()
    {
        // ��ġ ���� ���θ� �ʱ�ȭ
        isBuildArea = true;

        // �ݰ� �� ��ġ �Ұ� ������Ʈ�� �ִ��� Ȯ��
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