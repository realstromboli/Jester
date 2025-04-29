using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteAfterDelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "StartScreen")
        {
            StartCoroutine(Something());
        }
    }

    public IEnumerator Something()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
