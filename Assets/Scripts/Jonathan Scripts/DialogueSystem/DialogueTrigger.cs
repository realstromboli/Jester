using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public float raycastDistance = 8f;
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

    private void hitEForDialogue()
    {
        RaycastHit hit;
        if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, raycastDistance, interactableLayer))
        {
            DialogueManager.StartConversation(convo);
        }
    }
}
