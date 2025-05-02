using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveThis : MonoBehaviour
{
    public int dialogueToActivateAt;
    public DialogueManager dmScript;

    void Start()
    {
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
    }

    void Update()
    {
        if (dmScript.dialogueViewedSave == dialogueToActivateAt)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
