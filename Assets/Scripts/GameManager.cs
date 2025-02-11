using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{

    public GameObject settingsScreen;
    public GameObject controlsScreen;
    public GameObject pauseScreen;
    public GameObject HUD;
    public GameObject player;

    public bool isGameActive;
    public Rigidbody playerRb;

    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;

    public static GameManager instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure only one HUD instance
        GameObject existingHUD = GameObject.Find("Pause&HUDCanvas");
        if (existingHUD != null && existingHUD != HUD)
        {
            Destroy(existingHUD);
        }
        DontDestroyOnLoad(HUD);
    }

    void Start()
    {
        isGameActive = false;
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        HUD.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
    }

    

    

    void Update()
    {
        PauseGame();
        FreezePlayer();
    }

    public void NewGame()
    {

        SceneManager.LoadScene(1);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        isGameActive = true;
        HUD.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        isGameActive = true;
        HUD.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
        StartCoroutine(LoadDelay());
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        isGameActive = false;
        HUD.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        StartCoroutine(NewDelay());
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive == true)
        {
            pauseScreen.gameObject.SetActive(true);
            settingsScreen.gameObject.SetActive(false);
            controlsScreen.gameObject.SetActive(false);
            isGameActive = false;
            pmScript.FreezePlayer();
        }
    }


    public void backToPause()
    {
        pauseScreen.gameObject.SetActive(true);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
    }

    public void Unpause()
    {
        pauseScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);

        isGameActive = true;
        pmScript.UnfreezePlayer();
    }

    public void OpenSettings()
    {
        pauseScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
        controlsScreen.gameObject.SetActive(false);
    }

    public void OpenControls()
    {
        pauseScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void FreezePlayer()
    {
        if (isGameActive == false)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePosition;
            playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (isGameActive == true)
        {
            playerRb.constraints = RigidbodyConstraints.None;
            playerRb.constraints = RigidbodyConstraints.FreezeRotation;

        }
    }

    public IEnumerator NewDelay()
    {
        yield return new WaitForSeconds(0.1f);

        dpmScript.NewGame();
    }

    public IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(0.1f);

        dpmScript.LoadGame();

    }

}
