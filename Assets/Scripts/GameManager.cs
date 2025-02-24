using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IDataPersistence, IPointerClickHandler
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
    public Rigidbody playerRb;
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;

    public GameObject selectedShader;
    public bool thisItemSelected;

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
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        HUD = GameObject.Find("HUD");
        HUD.SetActive(false); // Ensure HUD is hidden initially
        pauseScreen.SetActive(false); // Ensure pause screen is hidden initially
        inventoryScreen.SetActive(false);
    }

    void Update()
    {
        PauseGame();
        FreezePlayer();
        InventoryManager();

        // Update HUD visibility based on game state
        HUD.SetActive(isGameActive);

        if (Input.GetKeyDown(KeyCode.I) && isGameActive && !inventoryOpen)
        {
            inventoryScreen.SetActive(true);
            isGameActive = false;
            inventoryOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && !isGameActive && inventoryOpen)
        {
            inventoryScreen.SetActive(false);
            isGameActive = true;
            inventoryOpen = false;
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        pauseScreen.SetActive(false);
        HUD.SetActive(false);
        StartCoroutine(IDKDelay());
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive && !inventoryOpen)
        {
            pauseScreen.SetActive(true);
            settingsScreen.SetActive(false);
            controlsScreen.SetActive(false);
            pauseCanvas.GetComponent<Canvas>().sortingOrder = 2;
            isGameActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isGameActive && !inventoryOpen)
        {
            pauseScreen.SetActive(false);
            settingsScreen.SetActive(false);
            controlsScreen.SetActive(false);
            pauseCanvas.GetComponent<Canvas>().sortingOrder = 0;
            isGameActive = true;
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
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void FreezePlayer()
    {
        if (!isGameActive)
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
        SceneManager.LoadScene(3);
        Debug.Log("Starting Game");
        StartCoroutine(NewDelay());
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        StartCoroutine(LoadDelay());
    }

    public IEnumerator NewDelay()
    {
        yield return new WaitForSeconds(0.1f);
        startScreen.SetActive(false);
        dpmScript.NewGame();
        isGameActive = true;
    }

    public IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(0.1f);
        startScreen.SetActive(false);
        dpmScript.LoadGame();
        isGameActive = true;
    }

    public IEnumerator IDKDelay()
    {
        yield return new WaitForSeconds(0.1f);
        HUD.SetActive(false);
        startScreen.SetActive(true);
        isGameActive = false;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click");
        }
    }

    public void OnRightClick()
    {
        selectedShader.SetActive(true);
        thisItemSelected = true;
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

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    public GameObject item6;
    public GameObject item7;
    public GameObject item8;
    public GameObject item9;

    public GameObject slotPanel1;
    public GameObject slotPanel2;
    public GameObject slotPanel3;
    public GameObject slotPanel4;
    public GameObject slotPanel5;
    public GameObject slotPanel6;
    public GameObject slotPanel7;
    public GameObject slotPanel8;
    public GameObject slotPanel9;

    public bool slot1Full;
    public bool slot2Full;
    public bool slot3Full;
    public bool slot4Full;
    public bool slot5Full;
    public bool slot6Full;
    public bool slot7Full;
    public bool slot8Full;
    public bool slot9Full;

    public void InventoryManager()
    {
        item1 = GameObject.Find("PlaceholderItem1");
        item2 = GameObject.Find("PlaceholderItem2");
        item3 = GameObject.Find("PlaceholderItem3");

        if (slot1Full)
        {
            slot1.SetActive(true);
            item1.GetComponent<Renderer>().enabled = false;
            item1.GetComponent<Collider>().enabled = false;
        }
        else if (!slot1Full)
        {
            slot1.SetActive(false);
            item1.GetComponent<Renderer>().enabled = true;
            item1.GetComponent<Collider>().enabled = true;
        }

        if (slot2Full)
        {
            slot2.SetActive(true);
            item2.GetComponent<Renderer>().enabled = false;
            item2.GetComponent<Collider>().enabled = false;
        }
        else if (!slot2Full)
        {
            slot2.SetActive(false);
            item2.GetComponent<Renderer>().enabled = true;
            item2.GetComponent<Collider>().enabled = true;
        }

        if (slot3Full)
        {
            slot3.SetActive(true);
            item3.GetComponent<Renderer>().enabled = false;
            item3.GetComponent<Collider>().enabled = false;
        }
        else if (!slot3Full)
        {
            slot3.SetActive(false);
            item3.GetComponent<Renderer>().enabled = true;
            item3.GetComponent<Collider>().enabled = true;
        }

        //if (slot4Full)
        //{
        //    slot4.SetActive(true);
        //    item4.GetComponent<Renderer>().enabled = false;
        //    item4.GetComponent<Collider>().enabled = false;
        //}
        //else if (!slot4Full)
        //{
        //    slot4.SetActive(false);
        //    item4.GetComponent<Renderer>().enabled = true;
        //    item4.GetComponent<Collider>().enabled = true;
        //}

        //if (slot5Full)
        //{
        //    slot5.SetActive(true);
        //    item5.GetComponent<Renderer>().enabled = false;
        //    item5.GetComponent<Collider>().enabled = false;
        //}
        //else if (!slot5Full)
        //{
        //    slot5.SetActive(false);
        //    item5.GetComponent<Renderer>().enabled = true;
        //    item5.GetComponent<Collider>().enabled = true;
        //}

        //if (slot6Full)
        //{
        //    slot6.SetActive(true);
        //    item6.GetComponent<Renderer>().enabled = false;
        //    item6.GetComponent<Collider>().enabled = false;
        //}
        //else if (!slot6Full)
        //{
        //    slot6.SetActive(false);
        //    item6.GetComponent<Renderer>().enabled = true;
        //    item6.GetComponent<Collider>().enabled = true;
        //}

        //if (slot7Full)
        //{
        //    slot7.SetActive(true);
        //    item7.GetComponent<Renderer>().enabled = false;
        //    item7.GetComponent<Collider>().enabled = false;
        //}
        //else if (!slot7Full)
        //{
        //    slot7.SetActive(false);
        //    item7.GetComponent<Renderer>().enabled = true;
        //    item7.GetComponent<Collider>().enabled = true;
        //}

        //if (slot8Full)
        //{
        //    slot8.SetActive(true);
        //    item8.GetComponent<Renderer>().enabled = false;
        //    item8.GetComponent<Collider>().enabled = false;
        //}
        //else if (!slot8Full)
        //{
        //    slot8.SetActive(false);
        //    item8.GetComponent<Renderer>().enabled = true;
        //    item8.GetComponent<Collider>().enabled = true;
        //}

        //if (slot9Full)
        //{
        //    slot9.SetActive(true);
        //    item9.GetComponent<Renderer>().enabled = false;
        //    item9.GetComponent<Collider>().enabled = false;
        //}
        //else if (!slot9Full)
        //{
        //    slot9.SetActive(false);
        //    item9.GetComponent<Renderer>().enabled = true;
        //    item9.GetComponent<Collider>().enabled = true;
        //}
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
    }
}
