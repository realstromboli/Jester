using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveReticle : MonoBehaviour
{
    public PlayerCamera pcScript;
    public PlayerMovement pmScript;
    public float raycastDistance = 12f;
    public float grappleRayDistance = 110f;
    public float anotherRayDistance = 1000000f;
    public GameObject reticleHand;
    public GameObject reticleDot;
    public Image reticleHandSprite;
    public Image reticleTalkSprite;
    public Image reticleGrappleSprite;
    public LayerMask whatIsGround;

    
    void Start()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        reticleHandSprite.enabled = false;
        reticleTalkSprite.enabled = false;
    }

    
    void Update()
    {
        SetReticleSprite();
    }

    public void SetReticleSprite()
    {
        RaycastHit hit;

        // First raycast for general interactions
        if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("GhostInteractable"))
            {
                reticleTalkSprite.enabled = true;
                reticleHandSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleDot.SetActive(false);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("JustShowHand") ||
                     hit.collider.gameObject.layer == LayerMask.NameToLayer("Ghost"))
            {
                reticleHandSprite.enabled = true;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleDot.SetActive(false);
            }
            else
            {
                reticleHandSprite.enabled = false;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleDot.SetActive(true);
            }
        }
        // Second raycast for grapple points
        else if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, grappleRayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("GrapplePoint") && pmScript.hasTrapezistPower)
            {
                reticleHandSprite.enabled = false;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = true;
                reticleDot.SetActive(false);
            }
            else
            {
                reticleHandSprite.enabled = false;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleDot.SetActive(true); // Reset to dot if not hitting a grapple point
            }
        }
        // Third raycast for other cases
        else if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, anotherRayDistance))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("GrapplePoint"))
            {
                reticleTalkSprite.enabled = false;
                reticleHandSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleDot.SetActive(true);
            }
        }
        // Default case when no raycast hits anything
        else
        {
            reticleTalkSprite.enabled = false;
            reticleHandSprite.enabled = false;
            reticleGrappleSprite.enabled = false;
            reticleDot.SetActive(true); // Ensure the reticle resets to a dot
        }
    }
}
