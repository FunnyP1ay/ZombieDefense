using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Story/Dialogue")]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)] public string[] dialogues;
}

