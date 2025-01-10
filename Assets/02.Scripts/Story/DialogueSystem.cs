using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.4f;

    public DialogueData dialogueData; // ��ũ���ͺ� ������Ʈ ����
    private Queue<string> dialogues = new Queue<string>();

    public void SettingNextDialogue()
    {
        StopAllCoroutines();
        if (dialogueData != null)
        {
            foreach (var dialogue in dialogueData.dialogues)
            {
                dialogues.Enqueue(dialogue);
            }
            StartNextDialogue();
        }
        else
        {
            Debug.LogError("DialogueData�� �������� �ʾҽ��ϴ�.");
        }
    }

    public void StartNextDialogue()
    {
        if (dialogues.Count == 0)
        {
            Debug.Log("��ȭ ��");
            dialogueText.text = "";
            return;
        }

        string nextDialogue = dialogues.Dequeue();
        StartCoroutine(TypeDialogue(nextDialogue));
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        yield return new WaitForSecondsRealtime(2.5f); // ���� ��� �� ��� ���
        StartNextDialogue();
    }
}