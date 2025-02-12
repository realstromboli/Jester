using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public int maskCount = 0;

    public Vector3 playerPosition = Vector3.zero;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    // the values defined in this constructor will be the default values
    // the game starts when there's no save file

    public GameData()
    {
        this.maskCount = 0;
        playerPosition = Vector3.zero;
    }
}
