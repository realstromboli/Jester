using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public int maskCount = 0;

    public Vector3 playerPosition = Vector3.zero;

    public string itemName;
    public int itemQuantity;
    public Sprite itemSprite;

    public Dictionary<string, bool> collectedItems;

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
        this.itemQuantity = 0;
        this.itemName = "";
        this.itemSprite = null;
        collectedItems = new Dictionary<string, bool>();
    }
}
