using UnityEngine;

public class StoryChecker : MonoBehaviour
{
    public StoryManager storyManager;
    public enum CheckType
    {
        Trigger,
        Button
    }
    public CheckType checkType;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out PlayerMove player))
        {
            if (checkType == CheckType.Trigger)
            {
                storyManager.NextStory();
                this.gameObject.SetActive(false);
            }
            else
            {
                GameManager.Instance.UI_InGame.fKeyPanel.SetActive(true);
                GameManager.Instance.player.GetComponent<PlayerMove>().isFKey = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMove player))
        {
            GameManager.Instance.UI_InGame.fKeyPanel.SetActive(false);
            GameManager.Instance.player.GetComponent<PlayerMove>().isFKey = false;
        }
    }
}
