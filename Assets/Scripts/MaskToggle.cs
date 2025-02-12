using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaskToggle : MonoBehaviour, IDataPersistence
{

    public bool maskStatus;
    public bool readyToPress;
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;
    public GameObject maskIndicator;

    public int maskCount = 0;
    public TextMeshProUGUI maskCountText;

    void Start()
    {
        maskStatus = false;
        maskIndicator.gameObject.SetActive(false);
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        dpmScript = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        readyToPress = true;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && readyToPress == true)
        {
            maskToggle();
            readyToPress = false;
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            maskCount = maskCount + 1;
        }

        if (maskStatus == false)
        {
            maskIndicator.gameObject.SetActive(false);

        }
        else if (maskStatus == true)
        {
            maskIndicator.gameObject.SetActive(true);
        }

        maskCountText.text = "" + maskCount;
    }

    public void maskToggle()
    {
        if (maskStatus == false)
        {
            maskStatus = true;
        }
        else if (maskStatus == true)
        {
            maskStatus = false;
        }
        StartCoroutine(MaskCooldown());
        Debug.Log("LOL");
    }

    public IEnumerator MaskCooldown()
    {
        yield return new WaitForSeconds(3);
        readyToPress = true;
        Debug.Log("Mask Ready!");
    }

    public void LoadData(GameData data)
    {
        this.maskCount = data.maskCount;
    }

    public void SaveData(ref GameData data)
    {
        data.maskCount = this.maskCount;
    }
}
