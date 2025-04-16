using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using JetBrains.Annotations;
using TMPro.Examples;

public class DialogueManager : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI speakerName, dialogue;
    public Image speakerSprite;
    public float dialogueTypeSpeed = 0.02f;
    public float dialogueDelay = 1.5f;
    public int dialogueViewedSave;
    public bool makingDescision;
    public bool dialogueActive;

    public GameObject buttonPrefab;
    public GameObject skipText;
    public GameObject magicianDoor;
    public GameObject magicianCards;
    public Transform buttonContainer;

    private int currentIndex;
    private int boxLeftScale = 382;
    private DialogueConversation currentConvo;
    private static DialogueManager instance;
    private Animator anim;
    private Coroutine typing;
    private Image dialogueBox;
    private Canvas dialogueCanvas;

    private GameManager gameManager;
    private PlayerMovement pmScript;
    private Timer timerScript;

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

            dialogueActive = false;

            // dialogueViewedSave set to the saved number
        }
        else
        {
            Destroy(gameObject);
        }

        skipText = GameObject.Find("SkipText");
        skipText.SetActive(false);
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        timerScript = GameObject.Find("MaskIndicator").GetComponent<Timer>();
    }

    private void Update()
    {
        DialogueLine currentLine = currentConvo.GetLineByIndex(currentIndex - 1);

        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) && dialogueActive && currentLine.dialogueOptions.Length <= 0)
        {
            StopAllCoroutines();
            //StopCoroutine("WaitAndReadNext");
            ReadNext();
        }

        // Check if dialogueViewedSave reaches 5
        if (dialogueViewedSave == 7)
        {
            StartParticleEffects();
        }

        if (dialogueActive)
        {
            skipText.SetActive(true);
            timerScript.Pause = true;
            timerScript.obscurity.color = new Color(timerScript.obscurity.color.r, timerScript.obscurity.color.g, timerScript.obscurity.color.b, 0);
        }
        else
        {
            skipText.SetActive(false);
            timerScript.Pause = false;
        }

        magicianDoor = GameObject.Find("Magician Door");
        magicianCards = GameObject.Find("MagicianCards");

        if (dialogueViewedSave >= 13) //update based on dialogueViewedSave after end of Parkour 1
        {
            magicianDoor.gameObject.SetActive(false);
            pmScript.enabledGhostWorld1 = false;
        }

        SetObjectiveText();
    }

    /*public static void StartConversation(DialogueConversation convo)
    {
        instance.dialogueActive = true;
        instance.anim.SetBool("isOpen", true);
        instance.currentIndex = 0;
        Debug.Log(instance.currentIndex);
        instance.currentConvo = convo;
        instance.speakerName.text = "";
        instance.dialogue.text = "";
        //instance.dialogueViewedSave++;

        instance.ReadNext();
    }*/

    public static void StartConversation(DialogueConversation convo)
    {
        instance.StartCoroutine(instance.StartConversationWithDelay(convo));
    }

    private IEnumerator StartConversationWithDelay(DialogueConversation convo)
    {
        yield return new WaitForSeconds(0.1f);
        instance.dialogueActive = true;
        instance.anim.SetBool("isOpen", true);
        instance.currentIndex = 0;
        instance.currentConvo = convo;
        instance.speakerName.text = "";
        instance.dialogue.text = "";

        instance.ReadNext();
    }

    public void ReadNext()
    {
        Debug.Log("Yo 3");
        if (currentIndex >= currentConvo.GetLength() + 1)
        {
            instance.anim.SetBool("isOpen", false);
            dialogueActive = false;
            currentIndex = 0;
            currentConvo = null;
            correctAnswersCount = 0;
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
            makingDescision = true;
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
            layoutElement.preferredHeight = 85; // Adjust the height as needed
        }
    }

    private void OnOptionSelected(string option)
    {
        makingDescision = false;
        dialogueCanvas.GetComponent<Canvas>().sortingOrder = 0;

        // Handle the option selected logic here
        if (option == "Colombo" && correctAnswersCount != 1)
        {
            correctAnswersCount = -1;
        }

        if (option == "Green" && correctAnswersCount != 1)
        {
            correctAnswersCount = -1;
        }

        if (option == "Montague" && correctAnswersCount != 2)
        {
            correctAnswersCount = -1;
        }

        if (option == "Antonio" || option == "Lottie" || option == "Désiré" || option == "Colombo" || option == "Green" || option == "Montague" || option == "The Magnificent")
        {
            correctAnswersCount++;
        }

        if (option != "The Magnificent" && correctAnswersCount == 2)
        {
            dialogueViewedSave++;
        }
        else if (option == "Montague" && correctAnswersCount == 3)
        {
            dialogueViewedSave++;
        }
        Debug.Log(option);

        if (option == "Antonio" && correctAnswersCount > 0)
        {
            correctAnswersCount = 1;
        }

        if (option == "Lottie" && correctAnswersCount > 0)
        {
            correctAnswersCount = 1;
        }

        if (option == "Désiré" && correctAnswersCount > 0)
        {
            correctAnswersCount = 1;
        }

        foreach (Transform child in buttonContainer)
        {
            if ((correctAnswersCount <= 0 || correctAnswersCount >= 2) && option != "The Magnificent")
            {
                correctAnswersCount = 0;
            }
            else if (option == "Montague" && correctAnswersCount >= 3)
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

    private void StartParticleEffects()
    {
        // Find all particle systems in the scene
        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();

        // Loop through the particle systems and start the ones with the specified name
        int foundCount = 0;
        foreach (ParticleSystem ps in particleSystems)
        {
            if (ps.name == "Particle System") // Replace with the actual name of your particle systems
            {
                var main = ps.main;
                main.loop = true; // Enable looping
                ps.Play(); // Start the particle system
                foundCount++;
                if (foundCount == 2) break; // Stop after finding and starting two particle systems
            }
        }
    }

    public TMP_Text objectiveText;

    private void SetObjectiveText()
    {

        switch (dialogueViewedSave)
        {
            case 0:
                objectiveText.text = "Get accustomed to your trailer, anything amiss?";
                break;
            case 1:
                objectiveText.text = "See what is on the vanity";
                break;
            case 2:
                objectiveText.text = "Any significance to the name Antonio Colombo?";
                break;
            case 4:
                objectiveText.text = "Explore the Big Top after leaving your trailer";
                break;
            case 5:
                objectiveText.text = "Any objects related to the ghost in the Big Top?";
                break;
            case 6:
                objectiveText.text = "Find the torn off pieces of the picture";
                break;
            case 8:
                objectiveText.text = "Examine the completed picture";
                break;
            case 9:
                objectiveText.text = "Any significance to the name Lottie Green?";
                break;
            case 11:
                objectiveText.text = "Enter the Spirit World through the picture to recover Lottie's memories";
                break;
            case 12:
                objectiveText.text = "Traverse the Spirit World to recover Lottie's memories";
                break;
            case 13:
                objectiveText.text = "Explore the Big Top for any changes after coming back";
                break;
            case 14:
                objectiveText.text = "Find the Magician's six cards";
                break;
            case 15:
                objectiveText.text = "Figure out the Magician's name";
                break;
            case 17:
                objectiveText.text = "Enter the Spirit World through the picture to recover Desire's memories";
                break;
            default:
                objectiveText.text = "Keep progressing!";
                break;
        }
    }

    public void LoadData(GameData data)
    {
        dialogueViewedSave = data.dialogueViewedSave;
    }

    public void SaveData(ref GameData data)
    {
        data.dialogueViewedSave = dialogueViewedSave;
        
    }
}
