using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    public bool isRunning;

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

    public Vector3 respawnLocation;
    Vector3 moveDirection;
    public Rigidbody rb;
    private GameManager gmScript;
    public Camera pcScript;
    public DialogueManager dmScript;
    public GravitySwap gravitySwapScript;
    public DialogueTrigger dtScript;
    public AllDialogueTriggerRepeatable adtrScript;
    public float raycastDistance = 3;

    public bool activeGrapple;

    [Header("Item Stuff")]

    [SerializeField]
    private string itemName; // Changed to string

    [SerializeField]
    private int itemQuantity; // Changed to int

    [SerializeField]
    private Sprite itemSprite;

    [Header("Etc")]
    public bool hasJesterPower;
    public bool hasTrapezistPower;
    public bool hasMagicianPower;

    public bool jesterCureTrigger;
    public bool trapezistCureTrigger;
    public bool magicianCureTrigger;

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
        playerAnimation = GameObject.Find("PlayerObjHolder").GetComponent<Animator>();
        rb.freezeRotation = true;
        readyToJump = true;
        isRunning = false;
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        gravitySwapScript = GameObject.Find("Player").GetComponent<GravitySwap>();
        mtScript = GameObject.Find("Player").GetComponent<MaskToggle>();
        cardCountText.gameObject.SetActive(false);

        hasJesterPower = false;
        hasTrapezistPower = false;
        hasMagicianPower = false;
    }

    void Update()
    {
        PlayerInput();
        SpeedControl();
        Run();
        StateHandler();
        ItemInteraction();
        CheckAndDisablePosterPieces();

        //RaycastHit hit;
        //ground check
        if (gravitySwapScript.gravityReversed)
        {
            // Perform ground check with reversed gravity (raycast upwards)
            grounded = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, whatIsGround);
        }
        else if (!gravitySwapScript.gravityReversed)
        {
            // Perform ground check with normal gravity (raycast downwards)
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        }

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

        playerAnimation.SetFloat("Velocity", lolVelocity.magnitude);

        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        if (gmScript.isGameActive && hasMask)
        {
            mask.SetActive(false);
        }
    }

    public void AnimationManager()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }
    }

    //private void LateUpdate()
    //{
    //    // Follow the camera with an offset based on gravityReversed
    //    Vector3 cameraPosition = pcScript.transform.position;
    //    float xOffset = gravitySwapScript.gravityReversed ? 0.75f : -0.75f; // Adjust the offset values as needed
    //    transform.position = new Vector3(cameraPosition.x + xOffset, transform.position.y, transform.position.z);
    //}

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Swap left and right movement axes if gravity is reversed
        if (gravitySwapScript.gravityReversed)
        {
            horizontalInput = -horizontalInput;
        }

        // Jumping
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
            playerAnimation.SetBool("Grounded", true);
        }

        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            playerAnimation.SetBool("Grounded", true);
        }

        else
        {
            state = MovementState.air;
            playerAnimation.SetBool("Grounded", false);

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

    public void Jump()
    {
        exitingSlope = true;

        // Resets y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Determine the jump direction based on gravityReversed
        Vector3 jumpDirection = gravitySwapScript.gravityReversed ? Vector3.down : Vector3.up;

        // Apply the jump force in the determined direction
        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

        playerAnimation.SetTrigger("Jump Trigger");
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void Run()
    {
        if (Input.GetKeyDown(runKey) && isRunning == false)
        {
            isRunning = true;
        }
        
        else if (Input.GetKeyDown(runKey) && isRunning == true)
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
        Vector3 raycastDirection = gravitySwapScript.gravityReversed ? Vector3.up : Vector3.down;

        if (Physics.Raycast(transform.position, raycastDirection, out slopeHit, playerHeight * 0.5f + 0.3f))
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

        this.jesterCureTrigger = data.jesterCureTrigger;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;

        data.jesterCureTrigger = this.jesterCureTrigger;
    }

    private IEnumerator PosSetDelay(Vector3 position)
    {
        yield return null;

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

    public bool hasMask;
    public GameObject mask;
    public bool tPosterFixed;
    public MaskToggle mtScript;
    public int tPosterPieceCount;
    public int mCardsCount;
    public TextMeshProUGUI cardCountText;
    public Material tPosterMaterial;
    private Renderer tPosterRenderer;
    public TMP_Text jesterText;

    public void ItemInteraction()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && gmScript.isGameActive && !dmScript.dialogueActive)
        {
            RaycastHit hit;

            if (Physics.Raycast(pcScript.transform.position, pcScript.transform.forward, out hit, raycastDistance))
            {
                //if (hit.collider.CompareTag("Placeholder"))
                //{
                //    gmScript.slot1Full = true;
                //    Debug.Log("Slot 1 Filled");
                //    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                //}

                //if (hit.collider.CompareTag("Placeholder2"))
                //{
                //    gmScript.slot2Full = true;
                //    Debug.Log("Slot 2 Filled");
                //    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                //}

                //if (hit.collider.CompareTag("Placeholder3"))
                //{
                //    gmScript.slot3Full = true;
                //    Debug.Log("Slot 3 Filled");
                //    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                //}

                //if (hit.collider.CompareTag("Placeholder4")/* && hasJesterPower == true*/)
                //{
                //    gmScript.slot4Full = true;
                //    Debug.Log("Slot 4 Filled");
                //    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                //}

                if (hit.collider.CompareTag("Mask")/* && hasJesterPower == true*/)
                {
                    mask = GameObject.Find("Jester Mask");
                    hasMask = true;
                    mtScript.maskToggle();
                    mtScript.readyToPress = false;
                    Destroy(hit.collider.gameObject);
                    GameObject[] allObjects = FindObjectsOfType<GameObject>();
                    

                    foreach (GameObject obj in allObjects)
                    {
                        // Check if the object is on the ghost interactable layer
                        if (obj.CompareTag("Jester"))
                        {
                            // Toggle the Renderer component
                            Renderer renderer = obj.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.enabled = true;
                            }

                            // Toggle the Collider component
                            Collider collider = obj.GetComponent<Collider>();
                            if (collider != null)
                            {
                                collider.enabled = true;
                            }
                        }
                    }

                    
                    //jesterParticles.SetActive(true);
                }

                // Check if the item is on an interactable layer
                if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") || hit.collider.gameObject.layer == LayerMask.NameToLayer("GhostInteractable") || hit.collider.CompareTag("Trapezist") || hit.collider.CompareTag("Magician")) && gmScript.isGameActive && !dmScript.dialogueActive)
                {
                    //DialogueTriggerRepeatable dialogueTriggerRepeatable = hit.collider.GetComponent<DialogueTriggerRepeatable>();
                    //AllDialogueTriggerRepeatable adtrScript = hit.collider.GetComponent<AllDialogueTriggerRepeatable>();
                    DialogueTrigger dialogueTrigger = hit.collider.GetComponent<DialogueTrigger>();
                    QuestionDialogueTrigger questionDialogueTrigger = hit.collider.GetComponent<QuestionDialogueTrigger>();
                    DialogueTriggerRepeatable1 dialogueTriggerRepeatable1 = hit.collider.GetComponent<DialogueTriggerRepeatable1>();
                    QuestionDialogueTrigger1 questionDialogueTrigger1 = hit.collider.GetComponent<QuestionDialogueTrigger1>();
                    DialogueTriggerRepeatable2 dialogueTriggerRepeatable2 = hit.collider.GetComponent<DialogueTriggerRepeatable2>();
                    QuestionDialogueTrigger2 questionDialogueTrigger2 = hit.collider.GetComponent<QuestionDialogueTrigger2>();
                    DialogueTrigger1 dialogueTrigger1 = hit.collider.GetComponent<DialogueTrigger1>();
                    DialogueTrigger2 dialogueTrigger2 = hit.collider.GetComponent<DialogueTrigger2>();
                    DialogueTrigger3 dialogueTrigger3 = hit.collider.GetComponent<DialogueTrigger3>();

                    Debug.Log("Dialogue hit interactable");
                    if (dialogueTrigger1 != null)
                    {
                        dialogueTrigger1.startConvo();
                    }
                    else if (dialogueTrigger != null)
                    {
                        dialogueTrigger.startConvo();
                    }
                    else if (dialogueTrigger2 != null)
                    {
                        dialogueTrigger2.startConvo();
                    }
                    else if (dialogueTrigger3 != null)
                    {
                        dialogueTrigger3.startConvo();
                    }
                    else if (questionDialogueTrigger != null)
                    {
                        questionDialogueTrigger.startConvo();
                    }
                    else if (dialogueTriggerRepeatable1 != null)
                    {
                        dialogueTriggerRepeatable1.startConvo();
                    }
                    else if (questionDialogueTrigger1 != null)
                    {
                        questionDialogueTrigger1.startConvo();
                    }
                    else if (dialogueTriggerRepeatable2 != null)
                    {
                        dialogueTriggerRepeatable2.startConvo();
                    }
                    else if (questionDialogueTrigger2 != null)
                    {
                        questionDialogueTrigger2.startConvo();
                    }
                }

                if (hit.collider.CompareTag("Jester") || hit.collider.CompareTag("Trapezist") || hit.collider.CompareTag("Magician"))
                {
                    playerAnimation.SetTrigger("Ghost Interaction Trigger");
                    jesterText = GameObject.Find("LOLText").GetComponent<TMP_Text>();
                    jesterText.text = "Antonio Colombo";
                }

                // door scene transition behavior
                if ((hit.collider.CompareTag("Door") || hit.collider.CompareTag("Net")) && hasJesterPower && !dmScript.dialogueActive)
                {
                    SceneTransition sceneTransition = hit.collider.GetComponent<SceneTransition>();
                    Debug.Log("Door hit interactable");
                    playerAnimation.SetTrigger("Pickup Trigger");

                    if (hit.collider.CompareTag("Net"))
                    {
                        dmScript.dialogueViewedSave++;
                    }

                    if (sceneTransition != null)
                    {
                        StartCoroutine(sceneTransition.FadeOutToScene(sceneTransition.fadeUI.GetComponent<UnityEngine.UI.Image>(), sceneTransition.fadeUIColor));
                    }

                    //hit.collider.gameObject.SetActive(false); // Deactivate the item
                }

                if (hit.collider.CompareTag("JesterPoster"))
                {
                    DialogueTrigger dialogueTrigger = hit.collider.GetComponent<DialogueTrigger>();
                    if (dialogueTrigger != null)
                    {
                        playerAnimation.SetTrigger("Pickup Trigger");
                        if (dmScript.dialogueViewedSave >= 1)
                        {
                            jesterCureTrigger = true;
                            gmScript.slot1Full = true;
                            //dmScript.dialogueViewedSave++;
                        }

                        dialogueTrigger.startConvo();
                    }
                }

                if (hit.collider.CompareTag("tPosterPiece") && (dmScript.dialogueViewedSave == 6 || dmScript.dialogueViewedSave == 7))
                {


                    playerAnimation.SetTrigger("Pickup Trigger");
                    Destroy(hit.collider.gameObject);

                    tPosterPieceCount++;

                    if (tPosterPieceCount == 1)
                    {
                        dtScript = GameObject.Find("HiddenDialogueSpeaker2").GetComponent<DialogueTrigger>();
                        dtScript.startConvo();
                    }

                    if (tPosterPieceCount == 2)
                    {
                        tPosterFixed = true;
                        dtScript = GameObject.Find("HiddenDialogueSpeaker3").GetComponent<DialogueTrigger>();
                        dtScript.startConvo();
                        tPosterRenderer = GameObject.Find("TrapezistPoster").GetComponent<Renderer>();
                        tPosterRenderer.material = tPosterMaterial;
                    }
                }

                if (hit.collider.CompareTag("TrapezistPoster"))
                {
                    DialogueTrigger dialogueTrigger = hit.collider.GetComponent<DialogueTrigger>();
                    if (dialogueTrigger != null)
                    {
                        playerAnimation.SetTrigger("Pickup Trigger");

                        if (dmScript.dialogueViewedSave >= 5)
                        {

                            //dmScript.dialogueViewedSave++;
                            dialogueTrigger.startConvo();
                        }
                    }

                    if (dmScript.dialogueViewedSave == 8 && tPosterFixed && mtScript.maskStatus == true)
                    {

                        dtScript = GameObject.Find("HiddenDialogueSpeaker4").GetComponent<DialogueTrigger>();
                        dtScript.startConvo();
                        gmScript.slot2Full = true;
                        trapezistCureTrigger = true;

                    }

                    if (dmScript.dialogueViewedSave == 11 && tPosterFixed)
                    {
                        Destroy(dialogueTrigger);
                        dtScript = GameObject.Find("HiddenDialogueSpeaker6").GetComponent<DialogueTrigger>();
                        dtScript.startConvo();
                        StartCoroutine(WaitForSeconds());
                        dmScript.correctAnswersCount = 0;
                    }

                    IEnumerator WaitForSeconds()
                    {
                        yield return new WaitForSeconds(0.1f);
                        enabledGhostWorld1 = true;
                    }

                    if (dmScript.dialogueViewedSave == 12 && enabledGhostWorld1 == true)
                    {
                        SceneTransition sceneTransition = hit.collider.GetComponent<SceneTransition>();
                        Debug.Log("Entering Ghost World");
                        if (sceneTransition != null)
                        {
                            StartCoroutine(sceneTransition.FadeOutToScene(sceneTransition.fadeUI.GetComponent<UnityEngine.UI.Image>(), sceneTransition.fadeUIColor));
                            enabledGhostWorld1 = false;
                        }
                    }
                }

                if (hit.collider.CompareTag("MagicianCards") && dmScript.dialogueViewedSave >= 14)
                {

                    Destroy(hit.collider.gameObject);

                    playerAnimation.SetTrigger("Pickup Trigger");

                    mCardsCount++;

                    cardCountText.text = "Magician Cards: " + mCardsCount + "/6";

                    cardCountText.gameObject.SetActive(true);

                    StartCoroutine(HUDPopUp());

                    if (mCardsCount == 6)
                    {
                        dtScript = GameObject.Find("HiddenDialogueSpeaker7").GetComponent<DialogueTrigger>();
                        dtScript.startConvo();
                        gmScript.slot3Full = true;
                        magicianCureTrigger = true;
                    }
                }

                if (hit.collider.CompareTag("MagicianPoster") && dmScript.dialogueViewedSave >= 17 && hasMagicianPower == true)
                {
                    SceneTransition sceneTransition = hit.collider.GetComponent<SceneTransition>();
                    Debug.Log("Entering Ghost World");
                    if (sceneTransition != null)
                    {
                        StartCoroutine(sceneTransition.FadeOutToScene(sceneTransition.fadeUI.GetComponent<UnityEngine.UI.Image>(), sceneTransition.fadeUIColor));
                    }
                }
            }
        }
    }

    public IEnumerator HUDPopUp()
    {
        yield return new WaitForSeconds(2f);
        cardCountText.gameObject.SetActive(false);
    }

    public bool enabledGhostWorld1;

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity) / 3);

        // If gravity is reversed, set the velocities to negative
        if (gravitySwapScript.gravityReversed)
        {
            velocityY = -velocityY * 5f;
        }

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

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            // Save the position of the collided object as the respawn location
            respawnLocation = collision.transform.position;
            Debug.Log("Respawn location saved: " + respawnLocation);
        }
        
        if (collision.gameObject.CompareTag("Death"))
        {
            // Set the player's position to the respawn location
            transform.position = respawnLocation;
            Debug.Log("Player respawned at: " + respawnLocation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            // Save the position of the collided object as the respawn location
            respawnLocation = other.transform.position;
            Debug.Log("Respawn location saved: " + respawnLocation);
        }
        
        if (other.CompareTag("Death"))
        {
            // Set the player's position to the respawn location
            transform.position = respawnLocation;
            Debug.Log("Player respawned at: " + respawnLocation);
        }
    }

    public void CheckAndDisablePosterPieces()
    {
        if (tPosterFixed)
        {
            GameObject[] posterPieces = GameObject.FindGameObjectsWithTag("tPosterPiece");
            foreach (GameObject piece in posterPieces)
            {
                piece.SetActive(false);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetRespawnLocationAfterDelay());
    }

    private IEnumerator SetRespawnLocationAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        respawnLocation = transform.position;
        Debug.Log("Respawn location set to: " + respawnLocation);
    }
}
