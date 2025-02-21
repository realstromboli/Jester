using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public DialogueSpeaker speaker;
    [TextArea]
    public string dialogue;
}
