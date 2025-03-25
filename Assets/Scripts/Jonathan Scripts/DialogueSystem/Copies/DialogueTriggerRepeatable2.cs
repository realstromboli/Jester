using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerRepeatable2 : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public int setDialogueViewedSave;
    public LayerMask interactableLayer;
    public PlayerCamera pcScript;
    public PlayerMovement pmScript;
    public int currentDialogueViewedSave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();

        if (pmScript.magicianCureTrigger == true)
        {
            Destroy(this);
        }
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Player") && dialogueManager.dialogueViewedSave == viewNumber)
    //    {
    //        DialogueManager.StartConversation(convo);
    //        Destroy(gameObject);
    //    }
    //}

    public void startConvo()
    {
        if (dialogueManager.dialogueViewedSave == viewNumber)
        {
            DialogueManager.StartConversation(convo);
            dialogueManager.dialogueViewedSave = setDialogueViewedSave;
        }

        else if (dialogueManager.dialogueViewedSave > viewNumber)
        {
            currentDialogueViewedSave = dialogueManager.dialogueViewedSave;
            dialogueManager.dialogueViewedSave = 4;
            DialogueManager.StartConversation(convo);
            dialogueManager.dialogueViewedSave = currentDialogueViewedSave;
        }
    }
}
