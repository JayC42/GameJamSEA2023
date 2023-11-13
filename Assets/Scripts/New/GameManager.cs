
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("General functions")]
    public static GameManager Instance; // Singleton instance

    public GameObject player;
    public GameObject gameOverUI;
    public GameObject levelCompleteUI;
    public GameObject mainMenuUI;
    public GameObject loadingScreen;
    private bool isLevelLoading = false;
    private bool isGamePaused = false; 

    private bool isPlayerAlive = true;

    private Vector3 CheckpointPosition;

    public static GameManager Inst;

    public PlayerController Player;
    public AIFollowerHealthManager AiCompanion;

    private string currentLevel;


    private void Awake()
    {
        // Ensure there is only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UpdateReferences();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
      
        #region Starting
        //ShowMainMenu();
        //this.currentLevel = SceneManager.GetActiveScene().name;
        #endregion
    }

    public void UpdateCheckpoint(Vector3 position)
    {
        this.CheckpointPosition = position;
    }
    public void ResetLevel()
    {
        base.StartCoroutine(this.ResetLevelRoutine());
    }
    public void LoadLevel(string levelName)
    {
        if (!isLevelLoading)
        {
            StartCoroutine(LoadLevelRoutine(levelName));
        }
    }
    private IEnumerator ResetLevelRoutine()
    {
        yield return SceneManager.LoadSceneAsync(currentLevel, 0);
        UpdateReferences();

        //if (Player != null && this.CheckpointPosition != Vector3.zero)
        //{
        //    Player.transform.position = CheckpointPosition;
        //}

        // Reset player's health to its maximum value
        if (Player != null)
        {
            Player.ResetHealth();
        }
        if (AiCompanion != null)
        {
            AiCompanion.ResetHealth();
        }

        yield break;
    }
    private IEnumerator LoadLevelRoutine(string levelName)
    {
        isLevelLoading = true;
        loadingScreen.SetActive(true); // Show loading screen

        yield return SceneManager.LoadSceneAsync(levelName, 0);

        loadingScreen.SetActive(false); // Hide loading screen
        UpdateReferences();
        CheckpointPosition = Vector3.zero;
        currentLevel = levelName;
        isLevelLoading = false;

        yield break;
    }
    private void UpdateReferences()
    {
        Player = FindObjectOfType<PlayerController>();
        AiCompanion = FindObjectOfType<AIFollowerHealthManager>();
        
        if (Player == null)
        {
            Debug.LogError("PlayerController not found in the scene!");
        }
        if (AiCompanion == null)
        {
            Debug.LogError("AI companion not found in the scene!");
        }
    }

    public void GameOver()
    {
        if (!isGamePaused)
        {
            PauseGame(); // Pause the game when the player dies
        }

        // Handle logic when the player dies
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // Show game over UI or any other relevant UI
        gameOverUI.SetActive(true);

        // Delay for a moment before reloading from the last checkpoint
        yield return new WaitForSeconds(2f);

        // Reload the level from the last checkpoint
        ResetLevel();

        // Unpause the game
        UnpauseGame();
    }
    private void PauseGame()
    {
        Time.timeScale = 0f; // Set the time scale to 0 to pause the game
        isGamePaused = true;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f; // Set the time scale back to 1 to unpause the game
        isGamePaused = false;
    }

    public void CheckWinCondition(string nextLevelName)
    {
        // check if the next level name is the final level
        if (nextLevelName == "FinalLevel")
        {
            // Handle winning condition, show level complete UI or any other relevant UI
            levelCompleteUI.SetActive(true);
        }
        else
        {
            // Load the next level
            LoadLevel(nextLevelName);
        }
    }
}
