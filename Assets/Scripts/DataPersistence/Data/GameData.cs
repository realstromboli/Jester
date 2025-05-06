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
    public bool slot10Full;
    public bool slot11Full;
    public bool slot12Full;

    public int dialogueViewedSave;
    public int jPosterPieceCount;
    public int tPosterPieceCount;
    public int mCardsCount;
    public string currentSceneName;

    public bool jesterCureTrigger;
    public bool interactedJesterPoster;
    public bool hasMask;
    public bool tPosterFixed;
    public bool hasJesterPower;
    public bool trapezistCureTrigger;
    public bool hasMagicianPower;
    public bool hasTrapezistPower;
    public bool magicianCureTrigger;
    public bool enabledGhostWorld1;

    public AudioClip savedSceneSong;
    private MusicController musicController;

    void Start()
    {
        musicController = GameObject.Find("Main Camera").GetComponent<MusicController>();
    }

    // The values defined in this constructor will be the default values
    // the game starts when there's no save file

    public GameData()
    {
        this.maskCount = 0;

        // Vector3(518, 12.2f, 212) for inside trailer
        // Vector3(2090, 75, 1347) for spririt world
        // Vector3(4259, 734, -422)
        // Vector3(-14, 0, 114) inside funhouse spawn
        // Vector3(-106,26,-276) indoor circus
        // Vector3(-27,14.5f,-116) TUT 1 
        // Vector3(-548,138,-124) TUT 2

        playerPosition = new Vector3(518, 12.2f, 212);
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
        slot10Full = false;
        slot11Full = false;
        slot12Full = false;

        dialogueViewedSave = 0;

        interactedJesterPoster = false;
        jesterCureTrigger = false;
        hasMask = false;
        tPosterFixed = false;
        hasJesterPower = false;
        trapezistCureTrigger = false;
        hasMagicianPower = false;
        hasTrapezistPower = false;
        magicianCureTrigger = false;
        enabledGhostWorld1 = false;
        jPosterPieceCount = 0;
        tPosterPieceCount = 0;
        mCardsCount = 0;

        //savedSceneSong = musicController.musicClips[0];
    }
}
