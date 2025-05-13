using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsAppear : MonoBehaviour
{
    private DialogueManager dmScript;

    void Start()
    {
        
    }

    void Update()
    {
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        if (dmScript != null && dmScript.dialogueViewedSave >= 19)
        {
            // Set all child objects to active
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}
