using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform rectTransform;

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // UI ������Ʈ�� �׻� ī�޶� �ٶ󺸵��� ȸ��
        Vector3 direction = (rectTransform.position - mainCamera.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rectTransform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
}
