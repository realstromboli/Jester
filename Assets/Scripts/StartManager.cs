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

    
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;

    void Start()
    {
        //isGameActive = true;
        settingsScreen.gameObject.SetActive(false);
        controlsScreen.gameObject.SetActive(false);
        dpmScript = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
    }

    void Update()
    {
        
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

    

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
