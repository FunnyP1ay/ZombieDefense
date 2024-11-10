using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public string imageAddress; // Addressable ��� (�⺻������ ���� ����)

    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    // �񵿱������� �̹��� �ε� (�������� ��θ� ���޹޵��� ����)
    public void LoadImage(string _imageAddress)
    {
        Addressables.LoadAssetAsync<Sprite>(_imageAddress).Completed += OnImageLoaded;
    }

    // �̹��� �ε� �Ϸ� �� ó��
    private void OnImageLoaded(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            imageComponent.sprite = obj.Result;
        }
        else
        {
            Debug.LogError("�̹��� �ε� ����");
        }
    }

    // �̹��� ��ε�
    public void UnloadImage(string _imageAddress)
    {
        Addressables.Release(_imageAddress);
    }
}