using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI speakerName, dialogue;
    public Image speakerSprite;
    public float dialogueTypeSpeed = 0.02f;
    public float dialogueDelay = 3.0f;
    public int dialogueViewedSave;
    public bool makingDescision;

    public GameObject buttonPrefab;
    public Transform buttonContainer;

    private int currentIndex;
    private int boxLeftScale = 192;
    private DialogueConversation currentConvo;
    private static DialogueManager instance;
    private Animator anim;
    private Coroutine typing;
    private Image dialogueBox;
    private Canvas dialogueCanvas;

    private GameManager gameManager;

    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;
    public int correctAnswersCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            anim = GetComponent<Animator>();
            dialogueBox = GetComponent<Image>();
            gameManager = FindObjectOfType<GameManager>();

            originalAnchorMin = dialogueBox.rectTransform.anchorMin;
            originalAnchorMax = dialogueBox.rectTransform.anchorMax;
            originalOffsetMin = dialogueBox.rectTransform.offsetMin;
            originalOffsetMax = dialogueBox.rectTransform.offsetMax;
            dialogueCanvas = gameObject.transform.parent.GetComponent<Canvas>();

            // dialogueViewedSave set to the saved number
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
        //instance.dialogueViewedSave++;

        instance.ReadNext();
    }

    public void ReadNext()
    {
        if (currentIndex >= currentConvo.GetLength() + 1)
        {
            instance.anim.SetBool("isOpen", false);
            return;
        }

        var speaker = currentConvo.GetLineByIndex(currentIndex).speaker;
        //speakerName.text = currentConvo.GetLineByIndex(currentIndex).speaker.GetName();

        // Check the speaker sprite
        if (speaker != null)
        {
            speakerName.text = speaker.GetName();

            if (currentConvo.GetLineByIndex(currentIndex).speaker.isSpriteless)
            {
                speakerSprite.gameObject.SetActive(false);
                // Adjust the width of the dialogue box
                dialogueBox.rectTransform.offsetMin = new Vector2(originalOffsetMin.x - boxLeftScale, dialogueBox.rectTransform.offsetMin.y); // Move left edge
                dialogueBox.rectTransform.offsetMax = new Vector2(originalOffsetMax.x, dialogueBox.rectTransform.offsetMax.y); // Keep right edge
            }
            else
            {
                speakerSprite.gameObject.SetActive(true);
                speakerSprite.sprite = speaker.GetSprite();
                // Restore dialogueBox to its original size
                dialogueBox.rectTransform.offsetMin = originalOffsetMin;
                dialogueBox.rectTransform.offsetMax = originalOffsetMax;
            }
        }
        else
        {
            speakerName.text = "";
            speakerSprite.gameObject.SetActive(false);
            // Adjust the width of the dialogue box
            dialogueBox.rectTransform.offsetMin = new Vector2(originalOffsetMin.x - boxLeftScale, dialogueBox.rectTransform.offsetMin.y); // Move left edge
            dialogueBox.rectTransform.offsetMax = new Vector2(originalOffsetMax.x, dialogueBox.rectTransform.offsetMax.y); // Keep right edge
        }

        if (typing == null)
        {
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }
            typing = instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));
        }
        else
        {
            instance.StopCoroutine(typing);
            typing = null;
            typing = instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));
        }

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

        DialogueLine currentLine = currentConvo.GetLineByIndex(currentIndex - 1);

        if (currentLine.dialogueOptions != null && currentLine.dialogueOptions.Length > 0)
        {
            DisplayOptions(currentLine.dialogueOptions);
        }
        else
        {
            StartCoroutine(WaitAndReadNext(text));
        }
    }

    private void DisplayOptions(string[] options)
    {
        makingDescision = true;
        dialogueCanvas.GetComponent<Canvas>().sortingOrder = 3;

        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        VerticalLayoutGroup layoutGroup = buttonContainer.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = buttonContainer.gameObject.AddComponent<VerticalLayoutGroup>();
        }

        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.spacing = 10; // Adjust the spacing as needed
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childControlWidth = true;

        foreach (string option in options)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = option;
            button.GetComponent<Button>().onClick.AddListener(() => OnOptionSelected(option));

            // Ensure the button has a LayoutElement component to control its size
            LayoutElement layoutElement = button.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = button.AddComponent<LayoutElement>();
            }
            layoutElement.minWidth = buttonContainer.GetComponent<RectTransform>().rect.width;
            layoutElement.preferredHeight = 40; // Adjust the height as needed
        }
    }

    private void OnOptionSelected(string option)
    {
        makingDescision = false;
        dialogueCanvas.GetComponent<Canvas>().sortingOrder = 0;

        // Handle the option selected logic here
        if (option == "Antonio" || option == "Lottie" || option == "Desire" || option == "Colombo" || option == "Green" || option == "Montague" || option == "The Magnificent")
        {
            correctAnswersCount++;
        }

        if (option != "Montague" && correctAnswersCount == 2)
        {
            dialogueViewedSave++;
        }
        else if (option == "Montague" && correctAnswersCount == 3)
        {
            dialogueViewedSave++;
        }
        Debug.Log(option);


        // For now, just continue the conversation
        foreach (Transform child in buttonContainer)
        {
            if (correctAnswersCount <= 0 || correctAnswersCount >= 2)
            {
                correctAnswersCount = 0;
            }
            Destroy(child.gameObject);
        }
        ReadNext();
    }

    /*
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
    }*/

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

    public void LoadData(GameData data)
    {
        dialogueViewedSave = data.dialogueViewedSave;
    }

    public void SaveData(ref GameData data)
    {
        data.dialogueViewedSave = dialogueViewedSave;
        
    }
}
