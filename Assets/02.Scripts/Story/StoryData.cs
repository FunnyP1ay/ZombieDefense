using UnityEngine;


public class StoryData : MonoBehaviour
{
    [Header("스토리 정보")]
    public string storyName; // 스토리 이름
    [TextArea]
    public string description; // 스토리 설명

    [Header("스토리 대사")]
    public DialogueData dialogueData; // 해당 스토리의 대사
    [Header("타임라인 후 삭제 오브젝트들")]
    public GameObject[] Clearobjectives;
    [Header("타임라인 후 킬 오브젝트들")]
    public GameObject[] Activeobjectives;
    [Header("타임라인 First 미션 실행 오브젝트들")]
    public GameObject[] FirestMission;
    [Header("타임라인 Last 미션 실행 오브젝트들")]
    public GameObject[] LastMission;
    [Header("스토리 시작즉시 켜지는 퀘스트 오브젝트")]
    public StoryChecker QuestObject;
}