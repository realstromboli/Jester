using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public LayerMask interactableLayer;
    public PlayerCamera pcScript;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && dialogueManager.dialogueViewedSave == viewNumber)
        {
            DialogueManager.StartConversation(convo);
            Destroy(gameObject);
        }
    }

    public void startConvo()
    {
        if (dialogueManager.dialogueViewedSave == viewNumber)
        {
            DialogueManager.StartConversation(convo);
        }
    }
}
