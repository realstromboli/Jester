using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public DialogueSpeaker speaker;
    public AudioClip dialogueAudio;
    [TextArea]
    public string dialogue;
    public string[] dialogueOptions;
}
