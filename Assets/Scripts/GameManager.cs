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

    public bool isGameActive;
    public Rigidbody playerRb;
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;

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
        isGameActive = false;
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        HUD = GameObject.Find("HUD");
    }

    void Update()
    {
        PauseGame();
        FreezePlayer();
        if (isGameActive == false)
        {
            HUD.gameObject.SetActive(false);
        }
        if (isGameActive == true)
        {
            HUD.gameObject.SetActive(true);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        pauseScreen.gameObject.SetActive(false);
        HUD.gameObject.SetActive(false);
        StartCoroutine(IDKDelay());
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive == true)
        {
            pauseScreen.gameObject.SetActive(true);
            settingsScreen.gameObject.SetActive(false);
            controlsScreen.gameObject.SetActive(false);
            isGameActive = false;
            //pmScript.FreezePlayer();
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
        //pmScript.UnfreezePlayer();
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
            playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
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
        startScreen.gameObject.SetActive(false);
        dpmScript.NewGame();
        isGameActive = true;
    }

    public IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(0.1f);
        startScreen.gameObject.SetActive(false);
        dpmScript.LoadGame();
        isGameActive = true;
    }

    public IEnumerator IDKDelay()
    {
        yield return new WaitForSeconds(0.1f);
        HUD.gameObject.SetActive(false);
        startScreen.gameObject.SetActive(true);
        isGameActive = false;
    }
}
