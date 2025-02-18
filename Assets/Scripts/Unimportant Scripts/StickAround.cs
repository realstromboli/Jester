using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAround : MonoBehaviour
{

    // MAKE A COPY FOR EACH ITEM THAT NEEDS TO PERSIST BETWEEN SCENES
    // THIS SCRIPT IS FOR ITEMS THAT NEED TO PERSIST BUT DON'T HAVE OTHER CODE REALLY

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
