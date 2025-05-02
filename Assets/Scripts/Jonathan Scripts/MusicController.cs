using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioListener audioListener;
    private GameData data;

    public AudioClip[] musicClips;

    private AudioClip prevSceneSong;
    private AudioClip sceneSong;

    private static MusicController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioListener = GetComponent<AudioListener>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        audioSource.volume = 0.4f;
        //audioSource.Play();
        //audioSource.loop = true;

        sceneSong = musicClips[0];
        audioSource.clip = sceneSong;
        audioSource.Play();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(/*UnityEngine.SceneManagement.*/Scene scene, LoadSceneMode mode)
    {
        if (scene.name != SceneManager.GetActiveScene().name)
        {
            return;
        }

        Debug.Log("Song scene load trigger");
        if (SceneManager.GetActiveScene().name == "StartScreen" || SceneManager.GetActiveScene().name == "CreditsScene")
        {
            Debug.Log("0" + SceneManager.GetActiveScene().name);
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[0];
        }
        else if (SceneManager.GetActiveScene().name == "Inside Trailer" || SceneManager.GetActiveScene().name == "Indoor Circus" || SceneManager.GetActiveScene().name == "3INDOORCIRC")
        {
            Debug.Log("1" + SceneManager.GetActiveScene().name);
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[1];
        }
        else if (SceneManager.GetActiveScene().name == "Inside Fun House" || SceneManager.GetActiveScene().name == "Mirrored Maze")
        {
            Debug.Log("2" + SceneManager.GetActiveScene().name);
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[2];
        }
        else if (SceneManager.GetActiveScene().name == "Parkour 1" || SceneManager.GetActiveScene().name == "Parkour 2")
        {
            Debug.Log("3" + SceneManager.GetActiveScene().name);
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[3];
        }
        Debug.Log($"Song change: Song: {sceneSong}, Prev Song: {prevSceneSong}");

        if (sceneSong != prevSceneSong)
        {
            StartCoroutine(FadeMusicOut());
        }
    }

    /*private IEnumerator FadeMusicOut()
    {
        Debug.Log("Fading song");
        //StopAllCoroutines();

        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.05f;
            if (audioSource.volume < 0)
            {
                audioSource.volume = 0f;
            }
            yield return new WaitForSeconds(0.08f);
        }
        audioSource.Stop();

        PickNewSong();

        yield return null;
    }*/

    private Coroutine fadeCoroutine;

    private IEnumerator FadeMusicOut()
    {
        Debug.Log("Fading song");

        // Ensure only one instance of the coroutine runs
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeMusicOutRoutine());
        yield return fadeCoroutine;
    }

    private IEnumerator FadeMusicOutRoutine()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.05f;
            if (audioSource.volume < 0)
            {
                audioSource.volume = 0f;
            }
            yield return new WaitForSeconds(0.08f);
        }
        audioSource.Stop();

        PickNewSong();
    }

    private void PickNewSong()
    {
        Debug.Log("Picking new song");
        StopCoroutine(FadeMusicOut());

        audioSource.clip = sceneSong;
        audioSource.volume = 0.4f;
        if (sceneSong == musicClips[1])
        {
            audioSource.spatialBlend = 1f;
        }
        else
        {
            audioSource.spatialBlend = 0f;
        }
        audioSource.Play();
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //public void LoadData(GameData data)
    //{
        //sceneSong = data.savedSceneSong;
    //}

    //public void SaveData(ref GameData data)
    //{
        //data.savedSceneSong = sceneSong;

    //}
}
