using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerName, dialogue;
    public Image speakerSprite;
    public float dialogueTypeSpeed = 0.02f;
    public float dialogueDelay = 3.0f;

    private int currentIndex;
    private DialogueConversation currentConvo;
    private static DialogueManager instance;
    private Animator anim;
    private Coroutine typing;

    private GameManager gameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            anim = GetComponent<Animator>();
            gameManager = FindObjectOfType<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartConversation(DialogueConversation convo)
    {
        instance.anim.SetBool("isOpen", true);
        instance.currentIndex = 0;
        instance.currentConvo = convo;
        instance.speakerName.text = "";
        instance.dialogue.text = "";

        instance.ReadNext();
    }

    public void ReadNext()
    {
        if (currentIndex >= currentConvo.GetLength() + 1)
        {
            instance.anim.SetBool("isOpen", false);
            return;
        }

        speakerName.text = currentConvo.GetLineByIndex(currentIndex).speaker.GetName();

        if (typing == null)
        {
            //dialogue.text = currentConvo.GetLineByIndex(currentIndex).dialogue;
            typing = instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));
        }
        else
        {
            instance.StopCoroutine(typing);
            typing = null;
            typing = instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));
        }
        
        speakerSprite.sprite = currentConvo.GetLineByIndex(currentIndex).speaker.GetSprite();
        currentIndex++;
    }

    private IEnumerator WaitAndReadNext(string text)
    {
        float waitTime = (dialogueTypeSpeed * text.Length) + dialogueDelay;
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            // Pause if the game is not active
            while (!gameManager.isGameActive)
            {
                yield return null;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ReadNext();
    }

    private IEnumerator TypeText(string text)
    {
        dialogue.text = "";
        int index = 0;

        while (index < text.Length)
        {
            // Pause if the game is not active
            while (!gameManager.isGameActive)
            {
                yield return null;
            }

            dialogue.text += text[index];
            index++;
            yield return new WaitForSeconds(dialogueTypeSpeed);
        }

        typing = null;

        StartCoroutine(WaitAndReadNext(text));
    }

    /*
    private IEnumerator WaitAndReadNext(string text)
    {
        yield return new WaitForSeconds((dialogueTypeSpeed * text.Length) + dialogueDelay);
        ReadNext();
    }

    private IEnumerator TypeText(string text)
    {
        dialogue.text = "";
        bool complete = false;
        int index = 0;

        while (!complete)
        {
            dialogue.text += text[index];
            index++;
            yield return new WaitForSeconds(dialogueTypeSpeed);

            if (index == text.Length)
            {
                complete = true;
            }
        }

        typing = null;

        StartCoroutine(WaitAndReadNext(text));
    }
    */
}
