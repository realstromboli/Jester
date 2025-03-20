using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwap : MonoBehaviour
{
    private Rigidbody rb;
    public bool gravityReversed = false;
    public float gravityStrength = 40f;
    public GameObject cameraHolder; // Reference to the target GameObject
    public LayerMask whatIsGround; // LayerMask for the "whatIsGround" layer
    public float raycastDistance = 50f; // Distance for the raycast

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable Unity's default gravity

        cameraHolder = GameObject.Find("CameraHolder");
    }

    void FixedUpdate()
    {
        // Apply custom gravity force
        Vector3 gravity = gravityReversed ? Vector3.up * gravityStrength : Vector3.down * gravityStrength;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CheckForGroundAbove())
            {
                gravityReversed = !gravityReversed;
                UpdateTargetObjectRotation();
            }
        }
    }

    private bool CheckForGroundAbove()
    {
        // Perform a raycast upwards to check for objects on the "whatIsGround" layer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, raycastDistance, whatIsGround))
        {
            Debug.Log("Ground detected above: " + hit.collider.name);
            return true;
        }
        else
        {
            Debug.Log("No ground detected above");
            return false;
        }
    }

    private void UpdateTargetObjectRotation()
    {
        if (cameraHolder != null)
        {
            float zRotation = gravityReversed ? 180f : 0f;
            cameraHolder.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
    }
}
