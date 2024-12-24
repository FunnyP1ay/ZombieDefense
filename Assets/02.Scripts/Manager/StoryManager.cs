
using UnityEngine;
using UnityEngine.Rendering;

public class StoryManager : MonoBehaviour
{
    [Header("���丮 ���")]
    public StoryData[] stories;
    public StoryData currentStory;
    private int currentStoryIndex = 0;
    void Start()
    {
        StartStory(currentStoryIndex);
    }

    public void StartStory(int index)
    {
        if (index < 0 || index >= stories.Length)
        {
            Debug.LogError("��ȿ���� ���� ���丮 �ε����Դϴ�.");
            return;
        }
        Debug.Log($"���丮 ����: {stories[index].storyName}");
        currentStory = stories[index];
    }
    public void StoryObjectClear()
    {

        foreach (var obj in currentStory.Clearobjectives)
        {
            obj.SetActive(false);
        }
    }
    public void StoryMissonStart()
    {
        foreach (var missionObject in currentStory.Missionobjectives)
        {
            missionObject.GetComponent<SPYGunner>().SPYMissionStart();
        }
    }
    public void StoryObjectActive()
    {
        foreach (var missionObject in currentStory.Activeobjectives)
        {
            missionObject.SetActive(true);
        }
    }
    public void NextStory()
    {
        currentStoryIndex++;
        if (currentStoryIndex < stories.Length)
        {
            StartStory(currentStoryIndex);
        }
        else
        {
            Debug.Log("��� ���丮�� �Ϸ��߽��ϴ�!");
        }
    }
}
