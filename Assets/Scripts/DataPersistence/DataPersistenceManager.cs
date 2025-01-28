using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    
    private GameData gameData;

    public static DataPersistenceManager instance
    {
        get; private set;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager in scene");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // TODO - Load any saved data from a raw file using data handler
        // if no data loads, initialize new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }
        // TODO - push the loaded data to all other scripts that need it
    }

    public void SaveGame()
    {
        // TODO - pass data to other scripts for updates
        // TODO - save data to a file using data handler
    }

}
