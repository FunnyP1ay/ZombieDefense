using UnityEngine;


public class StoryData : MonoBehaviour
{
    [Header("���丮 ����")]
    public string storyName; // ���丮 �̸�
    [TextArea]
    public string description; // ���丮 ����

    [Header("Ÿ�Ӷ��� �ִ���?")]
    public bool TimeLine = false;
    [Header("���丮 ���")]
    public DialogueData dialogueData; // �ش� ���丮�� ���
    [Header("Ÿ�Ӷ��� �� ���� ������Ʈ��")]
    public GameObject[] Clearobjectives;
    [Header("Ÿ�Ӷ��� �� ų ������Ʈ��")]
    public GameObject[] Activeobjectives;
    [Header("Ÿ�Ӷ��� �Ϲ� �̼� ���� ������Ʈ��")]
    public GameObject[] FirestMission;
    [Header("Ÿ�Ӷ��� SPY �̼� ���� ������Ʈ��")]
    public GameObject[] LastMission;
    [Header("���丮 ������� ������ ������Ʈ")]
    public GameObject[] StoryStartObject;
    [Header("���丮 ���� �� ������ ������Ʈ")]
    public GameObject[] StoryEndObject;
    [Header("�̵� ��ǥ")]
    public GameObject MoveTarget;
}