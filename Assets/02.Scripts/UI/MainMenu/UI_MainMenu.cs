using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    public Button InfinityWaveGameButton;
    private bool isStart = false;
    private void Start()
    {
        InfinityWaveGameButton.onClick.AddListener(() => GameStart());
    }
    public void GameStart()
    {
        if (isStart == false)
        {
            isStart = true;
            Invoke("Go", 3f);
        }
    }
    private void Go()
    {
        SceneManager.LoadScene("InfinityWaveGame");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
