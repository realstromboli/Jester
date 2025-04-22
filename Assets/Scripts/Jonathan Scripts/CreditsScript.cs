using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMyScene : MonoBehaviour
{
	private StartManager startManager;

	void Start()
    {
        startManager = FindObjectOfType<StartManager>();
    }

    public void LoadMenu()
	{
        startManager.startScreen.gameObject.SetActive(true);
        startManager.settingsScreen.gameObject.SetActive(false);
        startManager.controlsScreen.gameObject.SetActive(false);
        Debug.Log("Load main menu");
		UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
	}
}
