using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskToggle : MonoBehaviour
{

    public bool maskStatus;
    public bool readyToPress;
    public PlayerMovement pmScript;
    public GameObject maskIndicator;

    void Start()
    {
        maskStatus = false;
        maskIndicator.gameObject.SetActive(false);
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        readyToPress = true;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && readyToPress == true)
        {
            maskToggle();
            readyToPress = false;
        }

        if (maskStatus == false)
        {
            maskIndicator.gameObject.SetActive(false);

        }
        else if (maskStatus == true)
        {
            maskIndicator.gameObject.SetActive(true);
        }
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
}
