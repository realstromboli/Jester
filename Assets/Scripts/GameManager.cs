using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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

    [SerializeField]
    public ItemSlot[] isScript;

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
            isGameActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isGameActive && !inventoryOpen)
        {
            pauseScreen.SetActive(false);
            settingsScreen.SetActive(false);
            controlsScreen.SetActive(false);
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
        for (int i = 0; i < isScript.Length; i++)
        {
            if (isScript[i].isFull == false)
            {
                isScript[i].AddItem(itemName, itemQuantity, itemSprite);
                return;
            }
        }
    }
}
