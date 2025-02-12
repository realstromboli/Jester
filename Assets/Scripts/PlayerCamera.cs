using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    
    void Update()
    {
        if (gmScript.isGameActive == true)
        {
            //mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            yRotation += mouseX;
            xRotation -= mouseY;

            //rotate no more than 90 degrees
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //rotates camera and player orientation
            camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            playerObj.rotation = Quaternion.Euler(0, yRotation, 0);

            //locks mouse and hides it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (gmScript.isGameActive == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
