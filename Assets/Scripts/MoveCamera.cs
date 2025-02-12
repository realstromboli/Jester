using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform cameraPosition;
    public Vector3 offset;

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

    }


    void Update()
    {
        transform.position = cameraPosition.position + offset;
    }
}
