using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveThis : MonoBehaviour
{
    public int dialogueToActivateAt;
    public DialogueManager dmScript;
    public GameObject targetParticleSystemObject;

    void Start()
    {
        
    }

    void Update()
    {
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        if (dmScript != null && dmScript.dialogueViewedSave >= dialogueToActivateAt)
        {
            // Activate all child particle systems
            StartParticleEffects();
        }
    }

    private void StartParticleEffects()
    {
        if (targetParticleSystemObject != null)
        {
            // Get the ParticleSystem component from the target object
            ParticleSystem ps = targetParticleSystemObject.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                // Ensure the particle system is set to loop
                var main = ps.main;
                main.loop = true;

                // Play the particle system
                ps.Play();
            }
        }
    }
}
