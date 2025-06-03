using UnityEngine;


public class StoryData : MonoBehaviour
{
    [Header("스토리 정보")]
    public string storyName; // 스토리 이름
    [TextArea]
    public string description; // 스토리 설명

    [Header("타임라인 있는지?")]
    public bool TimeLine = false;

    [Header("스토리 대사")]
    public int noUse1;
    public DialogueData dialogueData;  // 해당 스토리의 대사

    [Header("타임라인 후 삭제 오브젝트들")]
    public int noUse2;
    public GameObject[] Clearobjectives;

    [Header("타임라인 후 킬 오브젝트들")]
    public int noUse3;
    public GameObject[] Activeobjectives;

    [Header("일반 미션 실행 오브젝트들(타임라인 가능)")]
    public int noUse4;
    public GameObject[] BasicMission;

    [Header("SPY 미션 실행 오브젝트들(타임라인 가능)")]
    public int noUse5;
    public GameObject[] SPYMission;

    [Header("스토리 시작즉시 켜지는 오브젝트")]
    public int noUse6;
    public GameObject[] StoryStartObject;

    [Header("스토리 끝날 때 꺼지는 오브젝트")]
    public int noUse7;
    public GameObject[] StoryEndObject;

    [Header("이동 목표")]
    public GameObject MoveTarget;
}