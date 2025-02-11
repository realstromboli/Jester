using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAround : MonoBehaviour
{

    public static StickAround instance
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
        
    }
}
