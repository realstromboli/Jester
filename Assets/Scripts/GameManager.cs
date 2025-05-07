using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public GameObject settingsScreen;
    public GameObject controlsScreen;
    public GameObject pauseScreen;
    public GameObject pauseCanvas;
    public GameObject HUD;
    public GameObject startScreen;
    public GameObject inventoryScreen;

    public bool isGameActive;
    public bool inventoryOpen;
    public bool startScreenOpen;
    public Rigidbody playerRb;
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;
    public MaskToggle maskScript;
    public Timer timerScript;
    public DialogueManager dmScript;
    public SceneTransition stScript;
    public GameData gameData;

    public string currentSceneName;
    public Vector3 playerPosition;

    [Header("Sprites")]
    public Sprite placeholderSprite;

    public static GameManager instance
    {
        get; private set;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        isGameActive = false; // Set initial game state
        inventoryOpen = false;
        startScreenOpen = true;
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        dpmScript = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        maskScript = GameObject.Find("Player").GetComponent<MaskToggle>();
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        HUD = GameObject.Find("HUD");
        stScript = GameObject.Find("SceneTransition").GetComponent<SceneTransition>();
        HUD.SetActive(false); // Ensure HUD is hidden initially
        pauseScreen.SetActive(false); // Ensure pause screen is hidden initially
        inventoryScreen.SetActive(false);

        itemNameText.text = "";
        itemDescriptionText.text = "";
        SetImageAlpha(inventoryItem, 0f);
    }

    void Update()
    {
        PauseGame();
        FreezePlayer();
        InventoryManager();
        UpdatePlayerPosition();

        // Update HUD visibility based on game state
        if (isGameActive || dmScript.dialogueActive)
        {
            HUD.SetActive(true);
            timerScript.Pause = false;
        }
        else if (!isGameActive || dmScript.dialogueActive == false)
        {
            HUD.SetActive(false);
            timerScript.Pause = true;
            timerScript.obscurity.color = new Color(timerScript.obscurity.color.r, timerScript.obscurity.color.g, timerScript.obscurity.color.b, 0);
        }

        if (Input.GetKeyDown(KeyCode.I) && isGameActive && !inventoryOpen && dmScript.dialogueActive == false)
        {
            inventoryScreen.SetActive(true);
            isGameActive = false;
            inventoryOpen = true;
            inventoryScreen.GetComponent<Canvas>().sortingOrder = 3;
        }
        else if (Input.GetKeyDown(KeyCode.I) && !isGameActive && inventoryOpen)
        {
            inventoryScreen.SetActive(false);
            isGameActive = true;
            inventoryOpen = false;
            inventoryScreen.GetComponent<Canvas>().sortingOrder = 1;
        }

        //if (!isGameActive)
        //{
        //    Time.timeScale = 0;
        //}
        //else if (isGameActive)
        //{
        //    Time.timeScale = 1;
        //}
    }

    private void UpdatePlayerPosition()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerPosition = player.transform.position;
        }
        else
        {
            Debug.LogWarning("Player GameObject not found");
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        pauseScreen.SetActive(false);
        HUD.SetActive(false);
        Item0Text();
        StartCoroutine(IDKDelay());
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive && !inventoryOpen && !startScreenOpen)
        {
            pauseScreen.SetActive(true);
            settingsScreen.SetActive(false);
            controlsScreen.SetActive(false);
            isGameActive = false;
            pauseCanvas.GetComponent<Canvas>().sortingOrder = 2;
            inventoryScreen.GetComponent<Canvas>().sortingOrder = 1;
            timerScript.Pause = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isGameActive && !inventoryOpen && !startScreenOpen)
        {
            pauseScreen.SetActive(false);
            settingsScreen.SetActive(false);
            controlsScreen.SetActive(false);
            isGameActive = true;
            pauseCanvas.GetComponent<Canvas>().sortingOrder = 1;
            inventoryScreen.GetComponent<Canvas>().sortingOrder = 2;
            timerScript.Pause = false;
        }
    }

    public void backToPause()
    {
        pauseScreen.SetActive(true);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
    }

    public void Unpause()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        isGameActive = true;
        inventoryScreen.SetActive(false);
        inventoryOpen = false;
    }

    public void OpenSettings()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }

    public void OpenControls()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void FreezePlayer()
    {
        if (!isGameActive || dmScript.makingDescision)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            playerRb.constraints = RigidbodyConstraints.None;
            playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
    }



    public void NewGame()
    {
        //scene 6 is outdoors
        //scene 7 is circus tent
        //scene 5 is inside trailer
        //scene 1 is test scene

        stScript = GameObject.Find("SceneTransition").GetComponent<SceneTransition>();
        stScript.sceneToGoTo = "TUT";
        if (stScript != null)
        {
            StartCoroutine(stScript.FadeOutToScene(stScript.fadeUI.GetComponent<UnityEngine.UI.Image>(), stScript.fadeUIColor));
            StartCoroutine(pmScript.SetRespawnLocationAfterDelay());
        }
        Debug.Log("Starting Game");
        StartCoroutine(NewDelay());
        maskScript.maskStatus = false;
    }

    public void LoadGame()
    {
        stScript.sceneToGoTo = gameData.currentSceneName;
        StartCoroutine(LoadSceneAndData(stScript.sceneToGoTo, gameData));
        SceneManager.LoadScene(1);
        StartCoroutine(LoadDelay());
        maskScript.maskStatus = false;
    }

    public IEnumerator NewDelay()
    {
        yield return null;
        startScreen.SetActive(false);
        dpmScript.NewGame();
        isGameActive = true;
        startScreenOpen = false;
        startScreen.GetComponent<Canvas>().sortingOrder = 0;
        Item0Text();
        StartCoroutine(PlayerPosDelay());
    }

    public IEnumerator LoadDelay()
    {
        yield return null;
        startScreen.SetActive(false);
        dpmScript.LoadGame();
        isGameActive = true;
        startScreenOpen = false;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public IEnumerator IDKDelay()
    {
        yield return null;
        HUD.SetActive(false);
        startScreen.SetActive(true);
        isGameActive = false;
        startScreenOpen = true;
        startScreen.GetComponentInChildren<Canvas>().sortingOrder = 3;
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public IEnumerator PlayerPosDelay()
    {
        yield return null;
        playerPosition = new Vector3(203, 14, 13);
    }

    public void AddItem(string itemName, int itemQuantity, Sprite itemSprite)
    {
        Debug.Log("Item added: " + itemName + ", Quantity: " + itemQuantity + ", Sprite: " + itemSprite);
        //for (int i = 0; i < isScript.Length; i++)
        //{
        //    if (isScript[i].isFull == false)
        //    {
        //        isScript[i].AddItem(itemName, itemQuantity, itemSprite);
        //        return;
        //    }
        //}
    }

    [Header("Inventory Stuff")]

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;
    public GameObject slot5;
    public GameObject slot6;
    public GameObject slot7;
    public GameObject slot8;
    public GameObject slot9;
    public GameObject slot10;
    public GameObject slot11;
    public GameObject slot12;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;
    public GameObject item7;
    public GameObject item8;
    public GameObject item9;
    public GameObject item10;
    public GameObject item11;
    public GameObject item12;

    public bool slot1Full;
    public bool slot2Full;
    public bool slot3Full;
    public bool slot4Full;
    public bool slot5Full;
    public bool slot6Full;
    public bool slot7Full;
    public bool slot8Full;
    public bool slot9Full;
    public bool slot10Full;
    public bool slot11Full;
    public bool slot12Full;

    public Image inventoryItem;
    public Image item1Image;
    public Image item2Image;
    public Image item3Image;
    public Image item4Image;
    public Image item5Image;
    public Image item6Image;
    public Image item7Image;
    public Image item8Image;
    public Image item9Image;
    public Image item10Image;
    public Image item11Image;
    public Image item12Image;

    public void InventoryManager()
    {

        if (slot1Full)
        {
            slot1.SetActive(true);
            //item1.GetComponent<Renderer>().enabled = false;
            //item1.GetComponent<Collider>().enabled = false;
        }
        else if (!slot1Full)
        {
            slot1.SetActive(false);
        }

        if (slot2Full)
        {
            slot2.SetActive(true);
            //item2.GetComponent<Renderer>().enabled = false;
            //item2.GetComponent<Collider>().enabled = false;
        }
        else if (!slot2Full)
        {
            slot2.SetActive(false);
        }

        if (slot3Full)
        {
            slot3.SetActive(true);
            //item3.GetComponent<Renderer>().enabled = false;
            //item3.GetComponent<Collider>().enabled = false;
        }
        else if (!slot3Full)
        {
            slot3.SetActive(false);
        }

        if (slot4Full)
        {
            slot4.SetActive(true);
            //item4.GetComponent<Renderer>().enabled = false;
            //item4.GetComponent<Collider>().enabled = false;
        }
        else if (!slot4Full)
        {
            slot4.SetActive(false);

            // bc its a ghost item
            //item4.GetComponent<Renderer>().enabled = true;
            //item4.GetComponent<Collider>().enabled = true;
        }

        if (slot5Full)
        {
            slot5.SetActive(true);
        }
        else if (!slot5Full)
        {
            slot5.SetActive(false);
        }

        if (slot6Full)
        {
            slot6.SetActive(true);
        }
        else if (!slot6Full)
        {
            slot6.SetActive(false);
        }

        if (slot7Full)
        {
            slot7.SetActive(true);
        }
        else if (!slot7Full)
        {
            slot7.SetActive(false);
        }

        if (slot8Full)
        {
            slot8.SetActive(true);
        }
        else if (!slot8Full)
        {
            slot8.SetActive(false);
        }

        if (slot9Full)
        {
            slot9.SetActive(true);
        }
        else if (!slot9Full)
        {
            slot9.SetActive(false);
        }

        if (slot10Full)
        {
            slot10.SetActive(true);
        }
        else if (!slot10Full)
        {
            slot10.SetActive(false);
        }

        if (slot11Full)
        {
            slot11.SetActive(true);
        }
        else if (!slot11Full)
        {
            slot11.SetActive(false);
        }

        if (slot12Full)
        {
            slot12.SetActive(true);
        }
        else if (!slot12Full)
        {
            slot12.SetActive(false);
        }
    }

    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    public void Item0Text()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        inventoryItem.sprite = null;
        SetImageAlpha(inventoryItem, 0f);
    }

    public void Item1Text()
    {
        itemNameText.text = "Mask Instructions (Press Q)";
        itemDescriptionText.text = "Follow these instructions properly. If the ghost does appear angry, seemingly without their humanity, it can be restored. You must say their full name to them to remind them of who they are. When wearing the mask ghostly objects are revealed but some on the human plane are obscured, wearing in intervals is the best way to reveal all.";
        inventoryItem.sprite = item1Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item2Text()
    {
        itemNameText.text = "Antonio's Flyer";
        itemDescriptionText.text = "This flyer advertizes a Jester with the stage name Oliver, but ghostly writing overwrites it to say Antonio Colombo..";
        inventoryItem.sprite = item2Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item3Text()
    {
        itemNameText.text = "Item 3";
        itemDescriptionText.text = "This is what item 3 does";
        inventoryItem.sprite = item3Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item4Text()
    {
        itemNameText.text = "Item 4";
        itemDescriptionText.text = "This is what item 4 does";
        inventoryItem.sprite = item4Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item5Text()
    {
        itemNameText.text = "Lottie's Image";
        itemDescriptionText.text = "This picture shows off a trapezist named Charlotte Green. But when masked, Charlotte is scribbled out with 'Lottie' written in it's place. Interesting..";
        inventoryItem.sprite = item5Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item6Text()
    {
        itemNameText.text = "Trapeze Grapple Instructions";
        itemDescriptionText.text = "Hitting right click on a grappleable object (indicated by red reticle) will allow the player to throw a rope out to pull themselves toward and above an object! Keep in mind that grappling will only give a player momentum when they pull upwards towards an object.";
        inventoryItem.sprite = item6Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item7Text()
    {
        itemNameText.text = "Item 7";
        itemDescriptionText.text = "This is what item 7 does";
        inventoryItem.sprite = item7Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item8Text()
    {
        itemNameText.text = "Item 8";
        itemDescriptionText.text = "This is what item 8 does";
        inventoryItem.sprite = item8Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item9Text()
    {
        itemNameText.text = "Desire's Cards";
        itemDescriptionText.text = "These 6 cards belong to 'The Magnificent' Montague. With the cards arranged, the letters on them will spell out his name: Desire.";
        inventoryItem.sprite = item9Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item10Text()
    {
        itemNameText.text = "Magician Flip Instructions";
        itemDescriptionText.text = "Presing F while there are magic platforms above you will reverse gravity, this can be toggled on and off as long as you have proper land above you!";
        inventoryItem.sprite = item10Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item11Text()
    {
        itemNameText.text = "Item 11";
        itemDescriptionText.text = "This is what item 11 does";
        inventoryItem.sprite = item11Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void Item12Text()
    {
        itemNameText.text = "Item 12";
        itemDescriptionText.text = "This is what item 12 does";
        inventoryItem.sprite = item12Image.sprite;
        SetImageAlpha(inventoryItem, 1f);
    }

    public void NullSlotClick()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        inventoryItem.sprite = null;
        SetImageAlpha(inventoryItem, 0f);
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void LoadData(GameData data)
    {
        this.slot1Full = data.slot1Full;
        this.slot2Full = data.slot2Full;
        this.slot3Full = data.slot3Full;
        this.slot4Full = data.slot4Full;
        this.slot5Full = data.slot5Full;
        this.slot6Full = data.slot6Full;
        this.slot7Full = data.slot7Full;
        this.slot8Full = data.slot8Full;
        this.slot9Full = data.slot9Full;
        this.slot10Full = data.slot10Full;
        this.slot11Full = data.slot11Full;
        this.slot12Full = data.slot12Full;

        StartCoroutine(LoadSceneAndData(data.currentSceneName, data));
    }

    public void SaveData(ref GameData data)
    {
        data.slot1Full = this.slot1Full;
        data.slot2Full = this.slot2Full;
        data.slot3Full = this.slot3Full;
        data.slot4Full = this.slot4Full;
        data.slot5Full = this.slot5Full;
        data.slot6Full = this.slot6Full;
        data.slot7Full = this.slot7Full;
        data.slot8Full = this.slot8Full;
        data.slot9Full = this.slot9Full;
        data.slot10Full = this.slot10Full;
        data.slot11Full = this.slot11Full;
        data.slot12Full = this.slot12Full;

        data.currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current scene name: " + data.currentSceneName);
    }

    public IEnumerator LoadSceneAndData(string sceneName, GameData data)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is empty, defaulting to 'Inside Trailer'");
            sceneName = "StartScreen";
        }

        // Load the saved scene
        SceneManager.LoadScene(sceneName);

        // Wait for the scene to load
        yield return null;

        Debug.Log("Saved scene loaded: " + sceneName);
    }
}
