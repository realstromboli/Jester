using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{

    [Header("References")]
    private PlayerMovement pmScript;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lineRenderer;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grappleCooldown;
    private float grappleCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;

    public ParticleSystem grappleFailParticles;

    void Start()
    {
        pmScript = GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grappleCdTimer > 0)
        {
            grappleCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grappleCdTimer > 0)
        {
            return;
        }

        grappling = true;

        pmScript.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);

            grappleFailParticles.Play();
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
        pmScript.playerAnimation.SetTrigger("grapple_trigger");
    }

    private void ExecuteGrapple()
    {
        pmScript.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0)
        {
            highestPointOnArc = overshootYAxis;
        }

        pmScript.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pmScript.freeze = false;
        
        grappling = false;

        grappleCdTimer = grappleCooldown;

        lineRenderer.enabled = false;

        pmScript.moveSpeed = 25f;
    }
}
