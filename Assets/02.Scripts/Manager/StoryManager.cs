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
        if(currentStory.TimeLine == false)
        {
            StorySPYMissonStart();
        }

        // DialogueSystem에 대사 데이터 설정
        if (dialogueSystem != null && currentStory.dialogueData != null)
        {
            dialogueSystem.dialogueData = currentStory.dialogueData;
            dialogueSystem.SettingNextDialogue(); // 대사 재생 시작
        }
    }

    public void StoryObjectClear()
    {
        foreach (var obj in currentStory.TimeLineAfterClearobjectives)
        {
            obj.SetActive(false);
        }
    }

    public void StoryFirstMissonStart()
    {
        foreach (var missionObject in currentStory.BasicMission)
        {
            if (missionObject.TryGetComponent(out Imission imission))
            {
                imission.MissionStart();
                Debug.Log("첫 번째 미션 시작");
            }
        }
    }

    public void StorySPYMissonStart()
    {
        if (currentStory.MoveTarget != null)
        {
            foreach (var missionObject in currentStory.SPYMission)
            {
                if (missionObject.TryGetComponent(out ISPY SPY))
                {
                    SPY.SPYMission(currentStory.MoveTarget);
                }
            }
        }
    }

    public void StoryObjectActive()
    {
        foreach (var missionObject in currentStory.TimeLineAfterActiveobjectives)
        {
            missionObject.SetActive(true);
        }
    }
    public void QuestObjectSetting()
    {

        if (currentStory.StoryStartObject.Length != 0)
        {
            foreach (GameObject _object in currentStory.StoryStartObject)
            {
                _object.SetActive(true);
            }
        }
    }
    public void QuestObjectOff()
    {
        if (currentStory.StoryEndObject.Length != 0)
        {
            foreach (GameObject _object in currentStory.StoryEndObject)
            {
                _object.SetActive(false);
            }
        }
    }
    public void NextStory()
    {
        QuestObjectOff();
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
