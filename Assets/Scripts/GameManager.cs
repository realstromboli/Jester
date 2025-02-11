using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{

    public GameObject settingsScreen;
    public GameObject controlsScreen;
    public GameObject pauseScreen;

    public bool isGameActive;
    public Rigidbody playerRb;
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;

    void Start()
    {
        isGameActive = false;
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        PauseGame();
        FreezePlayer();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        isGameActive = false;
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
            playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        }
    }

    public void NewGame()
    {

        SceneManager.LoadScene(1);
        isGameActive = true;
        dpmScript.NewGame();
        StartCoroutine(NewDelay());
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
        
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);

        StartCoroutine(LoadDelay());
    }

    public IEnumerator NewDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isGameActive = true;
        dpmScript.NewGame();
    }

    public IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isGameActive = true;
        dpmScript.LoadGame();
    }
}
