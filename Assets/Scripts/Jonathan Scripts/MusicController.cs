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

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioListener = GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Start()
    {
        audioSource.volume = 0.5f;
        //audioSource.Play();
        //audioSource.loop = true;

        sceneSong = musicClips[0];
        audioSource.clip = sceneSong;
        audioSource.Play();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Debug.Log("Song scene load trigger");
        if (SceneManager.GetActiveScene().name == "StartScreen" || SceneManager.GetActiveScene().name == "CreditsScene")
        {
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[0];
        }
        else if (SceneManager.GetActiveScene().name == "Inside Trailer" || SceneManager.GetActiveScene().name == "Indoor Circus")
        {
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[1];
        }
        else if (SceneManager.GetActiveScene().name == "Inside Fun House" || SceneManager.GetActiveScene().name == "Mirrored Maze")
        {
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[2];
        }
        else if (SceneManager.GetActiveScene().name == "Parkour 1" || SceneManager.GetActiveScene().name == "Parkour 2")
        {
            prevSceneSong = audioSource.clip;
            sceneSong = musicClips[3];
        }
        Debug.Log($"Song change: Song: {sceneSong}, Prev Song: {prevSceneSong}");

        if (sceneSong != prevSceneSong)
        {
            StartCoroutine(FadeMusicOut());
        }
    }

    private IEnumerator FadeMusicOut()
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

        yield return null;
    }

    private void PickNewSong()
    {
        StopCoroutine(FadeMusicOut());

        audioSource.clip = sceneSong;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

    //public void LoadData(GameData data)
    //{
        //sceneSong = data.savedSceneSong;
    //}

    public void SaveData(ref GameData data)
    {
        data.savedSceneSong = sceneSong;

    }
}
