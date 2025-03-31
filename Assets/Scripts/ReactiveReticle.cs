using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactiveReticle : MonoBehaviour
{
    public PlayerCamera pcScript;
    public float raycastDistance = 12f; // Distance for the raycast
    public GameObject reticleHand;
    public GameObject reticleDot;
    public Image reticleHandSprite;
    public LayerMask whatIsGround; // LayerMask for the "whatIsGround" layer

    // Start is called before the first frame update
    void Start()
    {
        pcScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
        reticleHandSprite = reticleHand.GetComponent<Image>();
        reticleHandSprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetReticleSprite();
    }

    public void SetReticleSprite()
    {
        RaycastHit hit;

        if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, raycastDistance))
        {
            // Check if the hit collider's tag is "Untagged" and the layer is either "Default" or "whatIsGround"
            if (hit.collider.CompareTag("Untagged") || (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default") || ((1 << hit.collider.gameObject.layer) & whatIsGround) != 0))
            {
                reticleHandSprite.enabled = false; // Hide the hand reticle
                reticleDot.SetActive(true); // Show the dot reticle
            }
            else
            {
                reticleHandSprite.enabled = true; // Show the hand reticle
                reticleDot.SetActive(false); // Hide the dot reticle
            }
        }
        else
        {
            // If nothing is hit, default to showing the dot reticle and hiding the hand reticle
            reticleHandSprite.enabled = false;
            reticleDot.SetActive(true);
        }
    }
}
