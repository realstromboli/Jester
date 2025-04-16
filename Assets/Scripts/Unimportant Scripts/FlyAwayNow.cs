using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAwayNow : MonoBehaviour
{
    public DialogueManager dmScript;
    public GameObject jester;
    public float moveSpeed = 8f;

    void Start()
    {
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
    }

    void Update()
    {
        if (dmScript == null)
        {
            dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();
        }

        FlyAwayyy();
    }

    public void FlyAwayyy()
    {
        if (dmScript.dialogueViewedSave >= 2)
        {
            StartCoroutine(FlyDelay());
        }
    }

    public IEnumerator FlyDelay()
    {
        yield return new WaitForSeconds(2f);

        jester.transform.rotation = Quaternion.Euler(0, 90, 0);

        jester.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
