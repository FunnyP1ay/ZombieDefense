using UnityEngine;


public class StoryData : MonoBehaviour
{
    [Header("���丮 ����")]
    public string storyName; // ���丮 �̸�
    [TextArea]
    public string description; // ���丮 ����

    [Header("Ÿ�Ӷ��� �� ���� ������Ʈ��")]
    public GameObject[] Clearobjectives;
    [Header("Ÿ�Ӷ��� �� ų ������Ʈ��")]
    public GameObject[] Activeobjectives;
    [Header("Ÿ�Ӷ��� First �̼� ���� ������Ʈ��")]
    public GameObject[] FirestMission;
    [Header("Ÿ�Ӷ��� Last �̼� ���� ������Ʈ��")]
    public GameObject[] LastMission; 
}