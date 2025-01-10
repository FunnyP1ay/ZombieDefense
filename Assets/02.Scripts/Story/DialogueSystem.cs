using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.4f;

    public DialogueData dialogueData; // 스크립터블 오브젝트 참조
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
            Debug.LogError("DialogueData가 설정되지 않았습니다.");
        }
    }

    public void StartNextDialogue()
    {
        if (dialogues.Count == 0)
        {
            Debug.Log("대화 끝");
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
        yield return new WaitForSecondsRealtime(2.5f); // 다음 대사 전 잠시 대기
        StartNextDialogue();
    }
}