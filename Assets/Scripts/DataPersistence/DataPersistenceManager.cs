using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    
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
}
