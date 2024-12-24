using UnityEngine;


public class StoryData : MonoBehaviour
{
    [Header("스토리 정보")]
    public string storyName; // 스토리 이름
    [TextArea]
    public string description; // 스토리 설명

    [Header("타임라인 후 삭제 오브젝트들")]
    public GameObject[] Clearobjectives;
    [Header("타임라인 후 킬 오브젝트들")]
    public GameObject[] Activeobjectives;
    [Header("타임라인 후 실행 오브젝트들")]
    public GameObject[] Missionobjectives; 
}