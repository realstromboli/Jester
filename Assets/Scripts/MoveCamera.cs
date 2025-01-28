using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform cameraPosition;
    public Vector3 offset;

    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = cameraPosition.position + offset;
    }
}
