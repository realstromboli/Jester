using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public int maskCount = 0;

    public Vector3 playerPosition = Vector3.zero;

    public bool slot1Full;
    public bool slot2Full;
    public bool slot3Full;
    public bool slot4Full;
    public bool slot5Full;
    public bool slot6Full;
    public bool slot7Full;
    public bool slot8Full;
    public bool slot9Full;

    public int thing;

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

        // (255, 1, 110) for outdoor scene
        // (250, 25, -275) for indoor circus
        // (77, 0, 1485) for inside trailer
        playerPosition = new Vector3(77, 0, 1485);

        slot1Full = false;
        slot2Full = false;
        slot3Full = false;
        slot4Full = false;
        slot5Full = false;
        slot6Full = false;
        slot7Full = false;
        slot8Full = false;
        slot9Full = false;
        thing = 0;

    }
}
