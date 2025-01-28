using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject startScreen;
    //public GameObject levelEndScreen;
    public GameObject settingsScreen;
    public GameObject controlsScreen;
    //public GameObject gameOverScreen;
    //public GameObject gameHUD;

    public bool isGameActive = false;
    public Rigidbody playerRb;
    public PlayerMovement pmScript;

    void Start()
    {
        //isGameActive = true;
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void StartGame()
    {

        SceneManager.LoadScene(1);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        isGameActive = true;
    }

    public void OpenSettings()
    {
        startScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
        controlsScreen.gameObject.SetActive(false);
    }

    public void OpenControls()
    {
        startScreen.gameObject.SetActive(false);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(true);
    }

    public void BackToStart()
    {
        startScreen.gameObject.SetActive(true);
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
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

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
