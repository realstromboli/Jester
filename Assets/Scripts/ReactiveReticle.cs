using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveReticle : MonoBehaviour
{
    public PlayerCamera pcScript;
    public float raycastDistance = 12f;
    public GameObject reticleHand;
    public GameObject reticleDot;
    public Image reticleHandSprite;
    public Image reticleTalkSprite;
    public LayerMask whatIsGround;

    
    void Start()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
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
            
            if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("Default") || ((1 << hit.collider.gameObject.layer) & whatIsGround) != 0))
            {
                reticleHandSprite.enabled = false;
                reticleTalkSprite.enabled = false;
                reticleDot.SetActive(true);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") || hit.collider.gameObject.layer == LayerMask.NameToLayer("GhostInteractable"))
            {
                reticleTalkSprite.enabled = true;
                reticleHandSprite.enabled = false;
                reticleDot.SetActive(false);
            }
            else
            {
                reticleHandSprite.enabled = true;
                reticleTalkSprite.enabled = false;
                reticleDot.SetActive(false);
            }
        }
        else
        {
            reticleTalkSprite.enabled = false;
            reticleHandSprite.enabled = false;
            reticleDot.SetActive(true);
        }
    }
}
