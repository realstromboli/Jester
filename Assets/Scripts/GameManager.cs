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

    public PlayerMovement pmScript;

    void Start()
    {
        isGameActive = true;
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    

    void Update()
    {
        PauseGame();
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
}
