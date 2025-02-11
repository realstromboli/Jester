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

    public static MoveCamera instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one PlayerCamera in scene");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
