#pragma warning disable 0414
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneTransition : MonoBehaviour
{
    private bool isFading = false;

    public float fadeSpeed = 1.5f;
    public GameObject fadeUI;
    public Color fadeUIColor;
    public string sceneToGoTo;
    public Vector3 playerTransferPosition;
    public Quaternion playerTransferRotation;

    public GameObject playerObj;
    public GameObject playerCam;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        playerObj = GameObject.Find("Player");
        playerCam = GameObject.Find("CameraHolder");
        fadeUI = GameObject.Find("FadeObject");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(FadeOutToScene(fadeUI.GetComponent<UnityEngine.UI.Image>(), fadeUIColor));
        }
    }

    /*public void FadeOutToScene(UnityEngine.UI.Image fadeObject, Color fadeColor)
    {
        isFading = true;
        Debug.Log($"Fade to {fadeColor}");
        //fadeObject.color = fadeColor;

        //fadeObject.CrossFadeAlpha(1.0f, fadeSpeed, false);

        //while (fadeObject.color.a < 1.0f)
        //{
        fadeObject.color = Color.Lerp(fadeObject.color, fadeColor, fadeSpeed * Time.deltaTime);
            //Debug.Log("fading");
        //}
        isFading = false;
    }*/

    public IEnumerator FadeOutToScene(UnityEngine.UI.Image fadeObject, Color fadeColor)
    {
        isFading = true;
        Color startColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        float elapsedTime = 0f;

        while (elapsedTime < fadeSpeed)
        {
            fadeObject.color = Color.Lerp(startColor, fadeColor, elapsedTime / fadeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeObject.color = fadeColor;

        LoadWantedScene();
        //isFading = false;
    }

    public IEnumerator FadeInToScene(UnityEngine.UI.Image fadeObject, Color fadeColor)
    {
        //isFading = true;

        Color startColor = fadeColor;
        Color endColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
        float elapsedTime = 0f;

        GameObject playerObj = GameObject.Find("Player");
        GameObject playerCam = GameObject.Find("CameraHolder");

        while (elapsedTime < fadeSpeed)
        {
            fadeObject.color = Color.Lerp(startColor, endColor, elapsedTime / fadeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeObject.color = endColor; // Ensure the final color is set
        isFading = false;
        Destroy(gameObject);
    }

    private void LoadWantedScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToGoTo);
        StartCoroutine(SceneLoadDelay());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Debug.Log("Scene detected");

        // Find the new fadeUI object in the new scene
        fadeUI = GameObject.Find("FadeObject");

        if (fadeUI != null)
        {
            StartCoroutine(FadeInToScene(fadeUI.GetComponent<UnityEngine.UI.Image>(), fadeUIColor));
        }
        else
        {
            Debug.LogError("FadeObject not found in the new scene.");
        }
    }

    public IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Scene Loaded and Player Position Set");

        // Set the player's position
        playerObj.transform.position = playerTransferPosition;

        // Set the player's rotation to -90 degrees on the Y-axis
        playerCam.transform.rotation = Quaternion.Euler(playerTransferRotation.eulerAngles.x, playerTransferRotation.eulerAngles.y, playerTransferRotation.eulerAngles.z);
    }
}
