using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    [Header("Camera List")]
    //public GameObject Cam1;
    //public GameObject Cam2;
    //public GameObject Cam3;
    //public GameObject Cam4;
    //public GameObject Cam5;
    //public GameObject Cam6;
    //public GameObject Cam7;
    //public GameObject Cam8;
    public Camera[] cameras;
    public int currentCameraIndex = 0;

    private bool isSwitchingMode = false;
    public float raycastDistance = 5f;
    public Camera playerCamera;

    public PlayerMovement pmScript;

    void Start()
    {
        //playerCamera = Camera.main;

        //// Ensure only the first camera is active at the start
        //for (int i = 0; i < cameras.Length; i++)
        //{
        //    cameras[i].gameObject.SetActive(i == 0);
        //}

        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSwitchingMode)
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("CCTV"))
                {
                    isSwitchingMode = true;
                    ActivateCameraMode();
                }
            }
        }

        if (isSwitchingMode)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchCamera(1); // Next camera
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwitchCamera(-1); // Previous camera
            }

            // Exit camera mode with the Escape key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactivateCameraMode();
            }
        }
    }

    void ActivateCameraMode()
    {
        isSwitchingMode = true;
        Debug.Log("Camera Mode Activated");
        playerCamera.gameObject.SetActive(false);

        // Activate the new camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
        pmScript.FreezePlayer();
    }

    private void DeactivateCameraMode()
    {
        isSwitchingMode = false;
        Debug.Log("Camera Mode Deactivated");

        // Disable all security cameras and reactivate the player's camera
        cameras[currentCameraIndex].gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        pmScript.UnfreezePlayer();
    }

    private void SwitchCamera(int direction)
    {
        // Disable the current security camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Calculate the new camera index
        currentCameraIndex = (currentCameraIndex + direction + cameras.Length) % cameras.Length;

        // Enable the new security camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
        Debug.Log("Switched to Camera: " + currentCameraIndex);
    }
}
