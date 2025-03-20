using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTester : MonoBehaviour
{
    public DialogueConversation convo;

    public void StartConvo()
    {
        DialogueManager.StartConversation(convo);
    }
}
