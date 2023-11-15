using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextSceneName;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
    public IEnumerator RestartCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (string.IsNullOrEmpty(currentSceneName))
        {
            Debug.LogError("currentLevel is not set. Cannot reload level.");
            yield break;
        }

        yield return SceneManager.LoadSceneAsync(currentSceneName, 0);
    }
}
