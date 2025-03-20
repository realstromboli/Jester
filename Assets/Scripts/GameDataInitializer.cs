using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataInitializer : MonoBehaviour
{
    public GameData gameData;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure gameData is not null
        if (gameData == null)
        {
            gameData = new GameData();
        }

        // Set the current scene name to the scene with build index 5
        gameData.currentSceneName = SceneManager.GetSceneByBuildIndex(5).name;
    }

    public static GameDataInitializer instance
    {
        get; private set;
    }
}
