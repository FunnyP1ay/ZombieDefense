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
        // UI 오브젝트가 항상 카메라를 바라보도록 회전
        Vector3 direction = (rectTransform.position - mainCamera.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rectTransform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
}
