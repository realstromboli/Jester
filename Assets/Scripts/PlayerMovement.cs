using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    bool isRunning;

    public float maxYSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool doubleJump;
    public LayerMask magicLayer;
    public float magicRaycastDistance = 50f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform orientation;
    //private AudioSource playerAudio;
    public Animator playerAnimation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    private GameManager gmScript;
    public Camera pcScript;
    public float raycastDistance = 3;

    public bool activeGrapple;

    [Header("Item Stuff")]

    [SerializeField]
    private string itemName; // Changed to string

    [SerializeField]
    private int itemQuantity; // Changed to int

    [SerializeField]
    private Sprite itemSprite;

    public MovementState state;

    public enum MovementState
    {
        freeze,
        walking,
        running,
        air
    }

    public bool freeze;

    public static PlayerMovement instance
    {
        get; private set;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
        rb.freezeRotation = true;
        readyToJump = true;
        isRunning = false;
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        PlayerInput();
        SpeedControl();
        Run();
        StateHandler();
        ItemInteraction();

        //RaycastHit hit;
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //grounded = Physics.SphereCast(transform.position + Vector3.up * 5, 3, Vector3.down, out hit, playerHeight, whatIsGround);

        //handles drag per ground check
        if (grounded && !activeGrapple)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        Vector3 lolVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //playerAnimation.SetFloat("move_speed", lolVelocity.magnitude);

        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    playerAnimation.SetTrigger("Test Trigger");
        //}

        playerAnimation.SetFloat("Velocity", lolVelocity.magnitude);

        if (Input.GetKeyDown(KeyCode.F))
        {
            DisableMagicLayerObjects();
        }
    }

    //public void PlayAnimation()
    //{

    //    playerAnimation.Play("test anim");

    //}

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jumping
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        // Mode - Freeze
        if (freeze)
        {
            state = MovementState.freeze;
            desiredMoveSpeed = 0;
            rb.velocity = Vector3.zero;
        }

        // Mode - Running
        else if (grounded && Input.GetKey(runKey))
        {
            state = MovementState.running;
            desiredMoveSpeed = runSpeed;
        }

        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < runSpeed)
            {
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                desiredMoveSpeed = runSpeed;
            }
        }
    }

    private void MovePlayer()
    {
        // calculates movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    public void SpeedControl()
    {
        // limits speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //velocity limiter
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

        // limit y velocity
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
    }

    public bool exitingSlope;

    private void Jump()
    {
        exitingSlope = true;

        //resets y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        playerAnimation.SetTrigger("Jump Trigger");
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void Run()
    {
        if (Input.GetKey(runKey))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (isRunning == true)
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void FreezePlayer()
    {
        freeze = true;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    public void UnfreezePlayer()
    {
        freeze = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void LoadData(GameData data)
    {
        StartCoroutine(PosSetDelay(data.playerPosition));
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    private IEnumerator PosSetDelay(Vector3 position)
    {
        yield return new WaitForSeconds(0.15f);

        this.transform.position = position;
    }

    //public void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.tag == "Placeholder")
    //    {
    //        Debug.Log("XDDDDDDD");
    //        gmScript.slot1Full = true;

    //    }

    //    if (collider.tag == "Placeholder2")
    //    {
    //        Debug.Log("XDDDDDDD");
    //        gmScript.slot2Full = true;
    //    }

    //    if (collider.tag == "Placeholder3")
    //    {
    //        Debug.Log("XDDDDDDD");
    //        gmScript.slot3Full = true;
    //    }
    //}

    public bool hasJesterPower;

    public void ItemInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Placeholder"))
                {
                    gmScript.slot1Full = true;
                    Debug.Log("Slot 1 Filled");
                    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                }

                if (hit.collider.CompareTag("Placeholder2"))
                {
                    gmScript.slot2Full = true;
                    Debug.Log("Slot 2 Filled");
                    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                }

                if (hit.collider.CompareTag("Placeholder3"))
                {
                    gmScript.slot3Full = true;
                    Debug.Log("Slot 3 Filled");
                    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                }

                if (hit.collider.CompareTag("Placeholder4") && hasJesterPower == true)
                {
                    gmScript.slot4Full = true;
                    Debug.Log("Slot 4 Filled");
                    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                }
            }
        }
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity) / 3);

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryheight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryheight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private bool enableMovementOnNextTouch;
    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    private void DisableMagicLayerObjects()
    {
        // Enable all objects in the magic layer
        

        RaycastHit hit;
        if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, magicRaycastDistance, magicLayer))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Magic"))
            {
                EnableAllMagicLayerObjects();
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
                hit.collider.enabled = false;

                Debug.Log("Disabled magic layer object: " + hit.collider.name);

                StartCoroutine(ReenableMagicLayerObjects(hit.collider));
            }
        }
    }

    private void EnableAllMagicLayerObjects()
    {
        StopAllCoroutines(); // Stop any ongoing coroutine
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Magic"))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }
    }

    private IEnumerator ReenableMagicLayerObjects(Collider collider)
    {
        yield return new WaitForSeconds(20f);
        Renderer renderer = collider.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }
        collider.enabled = true;
    }
}
