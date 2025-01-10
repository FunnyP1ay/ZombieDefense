using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [Header("스토리 목록")]
    public StoryData[] stories;
    public StoryData currentStory;

    private int currentStoryIndex = 0;

    public DialogueSystem dialogueSystem; // DialogueSystem 참조

    void Start()
    {
        StartStory(currentStoryIndex);
        GameManager.Instance.storyManager = this;
    }

    public void StartStory(int index)
    {
        if (index < 0 || index >= stories.Length)
        {
            Debug.LogError("유효하지 않은 스토리 인덱스입니다.");
            return;
        }

        Debug.Log($"스토리 시작: {stories[index].storyName}");
        currentStory = stories[index];
        QuestObjectSetting();
        // DialogueSystem에 대사 데이터 설정
        if (dialogueSystem != null && currentStory.dialogueData != null)
        {
            dialogueSystem.dialogueData = currentStory.dialogueData;
            dialogueSystem.SettingNextDialogue(); // 대사 재생 시작
        }
    }

    public void StoryObjectClear()
    {
        foreach (var obj in currentStory.Clearobjectives)
        {
            obj.SetActive(false);
        }
    }

    public void StoryFirstMissonStart()
    {
        foreach (var missionObject in currentStory.FirestMission)
        {
            if (missionObject.TryGetComponent(out Imission imission))
            {
                imission.MissionStart();
                Debug.Log("첫 번째 미션 시작");
            }
        }
    }

    public void StoryLastMissonStart()
    {
        foreach (var missionObject in currentStory.LastMission)
        {
            if (missionObject.TryGetComponent(out Imission imission))
            {
                imission.MissionStart();
            }
        }
    }

    public void StoryObjectActive()
    {
        foreach (var missionObject in currentStory.Activeobjectives)
        {
            missionObject.SetActive(true);
        }
    }
    public void QuestObjectSetting()
    {
        if(currentStory.QuestObject!=null)
        currentStory.QuestObject.gameObject.SetActive(true);
    }
    public void NextStory()
    {
        if (currentStory.QuestObject != null)
            currentStory.QuestObject.gameObject.SetActive(false);
        currentStoryIndex++;
        if (currentStoryIndex < stories.Length)
        {
            StartStory(currentStoryIndex);
        }
        else
        {
            Debug.Log("모든 스토리를 완료했습니다!");
        }
    }
}
