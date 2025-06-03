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
    public int dummy1;
    public GameObject[] TimeLineAfterClearobjectives;

    [Header("Ÿ�Ӷ��� �� ų ������Ʈ��")]
    public int dummy2;
    public GameObject[] TimeLineAfterActiveobjectives;

    [Header("Ÿ�Ӷ��� �� �Ϲ� �̼� ���� ������Ʈ��")]
    public int dummy3;
    public GameObject[] BasicMission;

    [Header("Ÿ�Ӷ��� �� SPY �̼� ���� ������Ʈ��")]
    public int dummy4;
    public GameObject[] SPYMission;

    [Header("���丮 ������� ������ ������Ʈ")]
    public int dummy5;
    public GameObject[] StoryStartObject;

    [Header("���丮 ���� �� ������ ������Ʈ")]
    public int dummy6;
    public GameObject[] StoryEndObject;

    [Header("�̵� ��ǥ")]
    public GameObject MoveTarget;
}