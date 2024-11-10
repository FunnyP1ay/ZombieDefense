using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public string imageAddress; // Addressable 경로 (기본값으로 설정 가능)

    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    // 비동기적으로 이미지 로드 (동적으로 경로를 전달받도록 수정)
    public void LoadImage(string _imageAddress)
    {
        Addressables.LoadAssetAsync<Sprite>(_imageAddress).Completed += OnImageLoaded;
    }

    // 이미지 로드 완료 후 처리
    private void OnImageLoaded(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            imageComponent.sprite = obj.Result;
        }
        else
        {
            Debug.LogError("이미지 로드 실패");
        }
    }

    // 이미지 언로드
    public void UnloadImage(string _imageAddress)
    {
        Addressables.Release(_imageAddress);
    }
}