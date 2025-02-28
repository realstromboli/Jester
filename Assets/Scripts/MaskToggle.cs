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
    public Animator playerAnimation;
    public GameObject maskIndicator;

    public int maskCount;
    public TextMeshProUGUI maskCountText;

    // Add a LayerMask field to specify the layer
    public LayerMask ghostLayer;

    void Start()
    {
        maskStatus = false;
        maskIndicator.gameObject.SetActive(false);
        playerAnimation = GetComponent<Animator>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        dpmScript = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        readyToPress = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && readyToPress == true)
        {
            maskToggle();
            playerAnimation.SetTrigger("Test Trigger");
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
            SetLayerVisibility(true);
        }
        else if (maskStatus == true)
        {
            maskStatus = false;
            SetLayerVisibility(false);
        }
        StartCoroutine(MaskCooldown());
        Debug.Log("LOL");
    }

    private void SetLayerVisibility(bool isVisible)
    {
        // Find all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object is on the specified layer
            if (((1 << obj.layer) & ghostLayer) != 0)
            {
                // Toggle the Renderer component
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = isVisible;
                }

                // Toggle the Collider component
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = isVisible;
                }
            }
        }
    }

    public IEnumerator MaskCooldown()
    {
        yield return new WaitForSeconds(3);
        readyToPress = true;
        Debug.Log("Mask Ready!");
    }

    public void LoadData(GameData data)
    {
        maskCount = data.maskCount;
    }

    public void SaveData(ref GameData data)
    {
        data.maskCount = maskCount;
    }
}
