using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    AudioManager audioManager;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    public GameObject pauseMenuUI;
    void Update()
    {
        //if (!GameIsPaused)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Pause();
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.LoadScene("Start Scene");
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game....");
        Application.Quit();
    }
}
