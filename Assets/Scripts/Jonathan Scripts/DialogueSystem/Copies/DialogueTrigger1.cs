using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger1 : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public LayerMask interactableLayer;
    public PlayerCamera pcScript;
    public PlayerMovement pmScript;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (pmScript.jesterCureTrigger == true)
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
        if (dialogueManager.dialogueViewedSave >= viewNumber)
        {
            
            DialogueManager.StartConversation(convo);
            //Destroy(this);

            // add if statement for conditional for correct and incorrect answers

            DialogueManager dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        }
    }
}
