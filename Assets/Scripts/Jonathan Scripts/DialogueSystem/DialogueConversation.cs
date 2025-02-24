#pragma warning disable 0649
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/New Conversation")]
public class DialogueConversation : ScriptableObject
{
    [SerializeField] private DialogueLine[] allLines;

    public DialogueLine GetLineByIndex(int index)
    {
        return allLines[index];
    }

    public int GetLength()
    {
        return allLines.Length - 1;
    }
}
