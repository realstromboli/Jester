using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public Vector3 offset;

    public GravitySwap gravitySwapScript; // Reference to the GravitySwap script

    public static MoveCamera instance
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
        // Find the GravitySwap script on the player
        gravitySwapScript = GameObject.Find("Player").GetComponent<GravitySwap>();
    }

    void Update()
    {
        // Update the offset based on the gravityReversed value
        if (gravitySwapScript.gravityReversed)
        {
            offset = new Vector3(0, -3, 0.5f);
        }
        else
        {
            offset = new Vector3(0, 3, 0.5f);
        }

        // Update the camera position
        transform.position = cameraPosition.position + offset;
    }
}
