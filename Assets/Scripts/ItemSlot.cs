using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDataPersistence
{
    // Item data
    public string itemName;
    public int itemQuantity;
    public Sprite itemSprite;
    public bool isFull;

    // Item slot
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    void Start()
    {

    }

    void Update()
    {

    }

    public void AddItem(string itemName, int itemQuantity, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.itemQuantity = itemQuantity;
        this.itemSprite = itemSprite;
        isFull = true;

        quantityText.text = itemQuantity.ToString(); 
        quantityText.enabled = true;
        itemImage.sprite = itemSprite; 
        itemImage.enabled = true;
    }

    public void LoadData(GameData data)
    {
        this.itemName = data.itemName;
        this.itemQuantity = data.itemQuantity;
        this.itemSprite = data.itemSprite;
    }

    public void SaveData(ref GameData data)
    {
        data.itemName = this.itemName;
        data.itemQuantity = this.itemQuantity;
        data.itemSprite = this.itemSprite;
    }
}
