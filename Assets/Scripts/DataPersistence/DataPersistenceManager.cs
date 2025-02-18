using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    
    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler fdhScript;

    public static DataPersistenceManager instance
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
        this.fdhScript = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            LoadGame();
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData); // Update all components with new game data
        }
    }

    public void LoadGame()
    {
        // Load any saved data from a raw file using data handler
        this.gameData = fdhScript.Load();

        // if no data loads, initialize new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }
        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }

        Debug.Log("Loaded button count = " + gameData.maskCount);


    }

    public void SaveGame()
    {
        // pass data to other scripts for updates
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }

        Debug.Log("Saved button count = " + gameData.maskCount);

        // save data to a file using data handler
        fdhScript.Save(gameData);


    }


    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
