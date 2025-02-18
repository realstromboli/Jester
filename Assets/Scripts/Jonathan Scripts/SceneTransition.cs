using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private bool isFading = false;

    public float fadeSpeed = 1.5f;
    public GameObject fadeUI;
    public Color fadeUIColor;
    public string sceneToGoTo;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
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

        while (elapsedTime < fadeSpeed)
        {
            fadeObject.color = Color.Lerp(startColor, endColor, elapsedTime / fadeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeObject.color = endColor; // Ensure the final color is set
        isFading = false;
    }

    private void LoadWantedScene()
    {
        SceneManager.LoadScene(sceneToGoTo);
        StartCoroutine(FadeInToScene(fadeUI.GetComponent<UnityEngine.UI.Image>(), fadeUIColor));
    }
}
