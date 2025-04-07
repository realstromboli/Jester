using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public LayerMask interactableLayer;
    public PlayerCamera pcScript;
    public DialogueTrigger dtScript;
    public PlayerMovement pmScript;
    public Material newMaterial;

    public Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        if (dialogueManager.dialogueViewedSave >= 3 )
        {
            objectRenderer.material = newMaterial;
            dtScript = GameObject.Find("HiddenDialogueSpeaker").GetComponent<DialogueTrigger>();
            dtScript.startConvo();
            pmScript.hasJesterPower = true;
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
            //Destroy(this);

            // add if statement for conditional for correct and incorrect answers

            DialogueManager dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
            
        }
    }
}
