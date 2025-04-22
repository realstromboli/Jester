using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionDialogueTrigger1 : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public LayerMask interactableLayer;
    public PlayerCamera pcScript;
    public DialogueTrigger dtScript;
    public PlayerMovement pmScript;
    public GameManager gmScript;
    public Grappling grappleScript;
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
        grappleScript = GameObject.Find("Player").GetComponent<Grappling>();
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        if (dialogueManager.dialogueViewedSave >= 10)
        {
            objectRenderer.material = newMaterial;
            dtScript = GameObject.Find("HiddenDialogueSpeaker5").GetComponent<DialogueTrigger>();
            pmScript.hasTrapezistPower = true;
            grappleScript.StartGrapple();
            dtScript.startConvo();
            pmScript.logText.text = "Trapezist Grapple Acquired! (Right click objects when reticle turns red)";
            pmScript.logText.gameObject.SetActive(true);
            gmScript.slot6Full = true;
            StartCoroutine(FadeOutText(pmScript.logText, 4f));
            
        }
    }

    public IEnumerator ok()
    {
        yield return new WaitForSeconds(3f);
        pmScript.logText.gameObject.SetActive(true);

    }

    public IEnumerator FadeOutText(TextMeshProUGUI textElement, float duration)
    {
        textElement.alpha = 1f;
        float elapsedTime = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            textElement.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        textElement.alpha = 0f;
        Destroy(this);
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
        if (dialogueManager.dialogueViewedSave == viewNumber && pmScript.trapezistCureTrigger)
        {
            
            DialogueManager.StartConversation(convo);
            //Destroy(this);

            // add if statement for conditional for correct and incorrect answers

            DialogueManager dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
            
        }
    }
}
