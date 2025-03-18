using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private const CursorLockMode locked = CursorLockMode.Locked;
    private CharacterController characterController;
    public GameObject player;
    // Start is called before the first frame update

    public float Speed = 5f;
    public float sensitivity = 2.0f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        characterController.Move(move*Time.deltaTime*Speed);

        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        transform.RotateAround(player.transform.position, Vector3.up, horizontalInput * sensitivity);
        transform.RotateAround(transform.position, transform.right, -verticalInput * sensitivity);

        Cursor.lockState = locked;
    }
}
