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

    // Add LayerMask fields to specify the layers
    public LayerMask ghostLayer;
    public LayerMask magicLayer;
    public LayerMask ghostInteractableLayer; // New LayerMask for GhostInteractable layer

    void Start()
    {
        maskStatus = false;
        maskIndicator.gameObject.SetActive(false);
        playerAnimation = GameObject.Find("PlayerObjHolder").GetComponent<Animator>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        dpmScript = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();

        readyToPress = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && readyToPress && pmScript.hasMask == true)
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
            SetLayerVisibility(true);
            playerAnimation.SetTrigger("Mask On Trigger");
        }
        else if (maskStatus == true)
        {
            maskStatus = false;
            SetLayerVisibility(false);
            playerAnimation.SetTrigger("Mask Off Trigger");
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
            // Check if the object is on the ghost layer
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

            //// Check if the object is on the magic layer
            //if (((1 << obj.layer) & magicLayer) != 0)
            //{
            //    // Toggle the Renderer component
            //    Renderer renderer = obj.GetComponent<Renderer>();
            //    if (renderer != null)
            //    {
            //        renderer.enabled = isVisible;
            //    }

            //    // Toggle the Collider component
            //    Collider collider = obj.GetComponent<Collider>();
            //    if (collider != null)
            //    {
            //        collider.enabled = isVisible;
            //    }
            //}

            // Check if the object is on the ghost interactable layer
            if (((1 << obj.layer) & ghostInteractableLayer) != 0)
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
