using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    public Button InfinityWaveGameButton;
    private void Start()
    {
        InfinityWaveGameButton.onClick.AddListener(() => SceneManager.LoadScene("InfinityWaveGame"));
    }
    public void Quit()
    {
        Application.Quit();
    }
}
