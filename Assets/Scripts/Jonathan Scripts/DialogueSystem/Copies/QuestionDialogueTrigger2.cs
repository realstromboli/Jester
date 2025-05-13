using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionDialogueTrigger2 : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public DialogueConversation convo;
    public int viewNumber;
    public LayerMask interactableLayer;
    public PlayerCamera pcScript;
    public DialogueTrigger dtScript;
    public PlayerMovement pmScript;
    public GameManager gmScript;
    public Material newMaterial;
    public Material newMaterial2;
    public Material newMaterial3;

    public Renderer objectRenderer;
    public Renderer objectRenderer2;
    public Renderer objectRenderer3;

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
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (dialogueManager.dialogueViewedSave >= 21)
        {
            objectRenderer.material = newMaterial;
            objectRenderer2.material = newMaterial2;
            objectRenderer3.material = newMaterial3;
            dtScript = GameObject.Find("HiddenDialogueSpeaker8").GetComponent<DialogueTrigger>();
            pmScript.hasMagicianPower = true;
            dtScript.startConvo();
            pmScript.logText.text = "Magician Flip Acquired! (Press F ro flip gravity while magic platforms are above) (Press I for info)";
            pmScript.logText.gameObject.SetActive(true);
            gmScript.slot10Full = true;
            StartCoroutine(FadeOutText(pmScript.logText, 10f));
        }
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
        if (dialogueManager.dialogueViewedSave == viewNumber && pmScript.magicianCureTrigger)
        {
            
            DialogueManager.StartConversation(convo);
            //Destroy(this);

            // add if statement for conditional for correct and incorrect answers

            DialogueManager dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
            
        }
    }
}
