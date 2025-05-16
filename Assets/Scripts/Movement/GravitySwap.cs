using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravitySwap : MonoBehaviour
{
    private Rigidbody rb;
    public bool gravityReversed = false;
    public float gravityStrength = 60f;
    public GameObject cameraHolder;
    public GameObject playerObject;
    public LayerMask whatIsGround; // LayerMask for the "whatIsGround" layer
    public float raycastDistance = 50f; // Distance for the raycast
    public PlayerMovement pmScript; // Reference to the PlayerMovement script
    public DialogueTrigger dtScript; // Reference to the DialogueTrigger script

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's default gravity

        cameraHolder = GameObject.Find("CameraHolder");
        playerObject = GameObject.Find("PlayerObjHolder");

        pmScript = GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        // Apply custom gravity force
        Vector3 gravity = gravityReversed ? Vector3.up * gravityStrength : Vector3.down * gravityStrength;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && pmScript.hasMagicianPower == true)
        {
            if (CheckForGround())
            {
                pmScript.playerAnimation.SetTrigger("Gravity Trigger");
                pmScript.playerAudio.Stop();
                pmScript.playerAudio.PlayOneShot(pmScript.gravitySound, 1.0f);
                gravityReversed = !gravityReversed;
                UpdateTargetObjectRotation();
            }
        }
        
        childObject = GameObject.Find("Jump with Cane");
        childTransform = childObject.transform;
    }

    private bool CheckForGround()
    {
        // Determine the direction based on gravityReversed
        Vector3 direction = gravityReversed ? Vector3.down : Vector3.up;

        // Perform a raycast in the determined direction to check for objects on the "whatIsGround" layer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance, whatIsGround))
        {
            Debug.Log("Ground detected: " + hit.collider.name);
            return true;
        }
        else
        {
            Debug.Log("No ground detected");
            return false;
        }
    }

    public GameObject childObject;
    public Transform childTransform;

    public void UpdateTargetObjectRotation()
    {
        if (cameraHolder != null)
        {
            float zRotation = gravityReversed ? 180f : 0f;
            cameraHolder.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            playerObject.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
            
            float zOffset = gravityReversed ? 0.75f : -0.75f;
            float yOffset = gravityReversed ? 1f : -1f;

            if (childObject != null)
            {
                Vector3 childPosition = childTransform.localPosition;
                childPosition.z = zOffset;
                childPosition.y = yOffset;
                childTransform.localPosition = childPosition;
                childObject.transform.rotation = Quaternion.Euler(gravityReversed ? 180f : 0f, 0f, zRotation);
            }
        }
        dtScript = GameObject.Find("SpecialDialogueSpeaker").GetComponent<DialogueTrigger>();
        dtScript.startConvo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Death"))
        {
            gravityReversed = false;
            UpdateTargetObjectRotation();
        }
    }

    public void resetOrientation()
    {
        gravityReversed = false;
        UpdateTargetObjectRotation();
    }
}
