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
    public Image reticleGrappleSprite2;
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

        
        if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("GhostInteractable"))
            {
                reticleTalkSprite.enabled = true;
                reticleHandSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleGrappleSprite2.enabled = false;
                reticleDot.SetActive(false);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("JustShowHand") ||
                     hit.collider.gameObject.layer == LayerMask.NameToLayer("Ghost") ||
                     hit.collider.gameObject.layer == LayerMask.NameToLayer("tPoster"))
            {
                reticleHandSprite.enabled = true;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleGrappleSprite2.enabled = false;
                reticleDot.SetActive(false);
            }
            else
            {
                reticleHandSprite.enabled = false;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleGrappleSprite2.enabled = false;
                reticleDot.SetActive(true);
            }
        }

        else if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, grappleRayDistance))
        {
            if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("GrapplePoint") ||
                 hit.collider.gameObject.layer == LayerMask.NameToLayer("Ghost Grapple") ||
                 hit.collider.gameObject.layer == LayerMask.NameToLayer("Inverse")) && pmScript.hasTrapezistPower)
            {
                
                if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("GrapplePoint") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Ghost Grapple") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Inverse")) &&
                    hit.collider.gameObject.transform.position.y > transform.position.y + 20)
                {
                    
                    reticleHandSprite.enabled = false;
                    reticleTalkSprite.enabled = false;
                    reticleGrappleSprite.enabled = false;
                    reticleGrappleSprite2.enabled = true;
                    reticleDot.SetActive(false);
                }
                else
                {
                    
                    reticleHandSprite.enabled = false;
                    reticleTalkSprite.enabled = false;
                    reticleGrappleSprite.enabled = true;
                    reticleGrappleSprite2.enabled = false;
                    reticleDot.SetActive(false);
                }
            }
            else
            {
                reticleHandSprite.enabled = false;
                reticleTalkSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleGrappleSprite2.enabled = false;
                reticleDot.SetActive(true);
            }
        }
        
        else if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, anotherRayDistance))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("GrapplePoint"))
            {
                reticleTalkSprite.enabled = false;
                reticleHandSprite.enabled = false;
                reticleGrappleSprite.enabled = false;
                reticleGrappleSprite2.enabled = false;
                reticleDot.SetActive(true);
            }
        }
        
        else
        {
            reticleTalkSprite.enabled = false;
            reticleHandSprite.enabled = false;
            reticleGrappleSprite.enabled = false;
            reticleGrappleSprite2.enabled = false;
            reticleDot.SetActive(true);
        }
    }
}
