using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletThis : MonoBehaviour
{
    public int dialogueToDestroyAt;
    public DialogueManager dmScript;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        if (dmScript.dialogueViewedSave == dialogueToDestroyAt)
        {
            Destroy(gameObject);
        }
    }
}
