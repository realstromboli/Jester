using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;

    public Transform orientation;
    public Transform camHolder;
    public Transform playerObj;

    float xRotation;
    float yRotation;

    public GameManager gmScript;
    private DialogueManager dialogueManager;
    public GravitySwap gravitySwapScript;

    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        gravitySwapScript = GameObject.Find("Player").GetComponent<GravitySwap>();
    }

    void Update()
    {
        if (gmScript.isGameActive == true && !dialogueManager.makingDescision)  //UNCOMMENT LATER
        {
            //mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            if (gravitySwapScript.gravityReversed)
            {
                yRotation -= mouseX;
                xRotation += mouseY;
            }
            else
            {
                yRotation += mouseX;
                xRotation -= mouseY;
            }

            //rotate no more than 90 degrees
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rotates camera and player orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, gravitySwapScript.gravityReversed ? 180f : 0f);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            playerObj.rotation = Quaternion.Euler(0, yRotation, 0);

            //locks mouse and hides it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (gmScript.isGameActive == false || dialogueManager.makingDescision)  //UNCOMMENT LATER
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (dialogueManager == null)
        {
            dialogueManager = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        }
    }
}
