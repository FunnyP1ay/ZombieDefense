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
    public int noUse1;
    public DialogueData dialogueData;  // �ش� ���丮�� ���

    [Header("Ÿ�Ӷ��� �� ���� ������Ʈ��")]
    public int noUse2;
    public GameObject[] Clearobjectives;

    [Header("Ÿ�Ӷ��� �� ų ������Ʈ��")]
    public int noUse3;
    public GameObject[] Activeobjectives;

    [Header("�Ϲ� �̼� ���� ������Ʈ��(Ÿ�Ӷ��� ����)")]
    public int noUse4;
    public GameObject[] BasicMission;

    [Header("SPY �̼� ���� ������Ʈ��(Ÿ�Ӷ��� ����)")]
    public int noUse5;
    public GameObject[] SPYMission;

    [Header("���丮 ������� ������ ������Ʈ")]
    public int noUse6;
    public GameObject[] StoryStartObject;

    [Header("���丮 ���� �� ������ ������Ʈ")]
    public int noUse7;
    public GameObject[] StoryEndObject;

    [Header("�̵� ��ǥ")]
    public GameObject MoveTarget;
}