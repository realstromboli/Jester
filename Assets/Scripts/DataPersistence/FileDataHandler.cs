using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{

    private string dataDirectoryPath = "";
    private string dataFileName = "";

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // use Path.Combine to account for different OS file path conventions
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the JSON string to a C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Error loading data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // use Path.Combine to account for different OS file path conventions
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            // create the directory the file will be written to if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object to a JSON string
            string dataToStore = JsonUtility.ToJson(data, true);

            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }
}
