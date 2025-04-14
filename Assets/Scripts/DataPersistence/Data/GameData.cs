using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{

    public int maskCount = 0;
    public Vector3 playerPosition;

    public bool slot1Full;
    public bool slot2Full;
    public bool slot3Full;
    public bool slot4Full;
    public bool slot5Full;
    public bool slot6Full;
    public bool slot7Full;
    public bool slot8Full;
    public bool slot9Full;

    public int dialogueViewedSave;
    public string currentSceneName;

    public bool jesterCureTrigger;
    public bool interactedJesterPoster;

    public AudioClip savedSceneSong;

    // The values defined in this constructor will be the default values
    // the game starts when there's no save file

    public GameData()
    {
        this.maskCount = 0;

        // Vector3(518, 16, 212) for inside trailer
        // Vector3(2090, 75, 1347) for spririt world
        // Vector3(4259, 734, -422)
        // Vector3(-14, 0, 114) inside funhouse spawn

        playerPosition = new Vector3(518, 16, 212);
        // Initialize currentSceneName to an empty string
        currentSceneName = string.Empty;

        slot1Full = false;
        slot2Full = false;
        slot3Full = false;
        slot4Full = false;
        slot5Full = false;
        slot6Full = false;
        slot7Full = false;
        slot8Full = false;
        slot9Full = false;

        dialogueViewedSave = 0;

        interactedJesterPoster = false;
        jesterCureTrigger = false;

        savedSceneSong = GameObject.Find("Main Camera").GetComponent<MusicController>().musicClips[0];
    }
}
