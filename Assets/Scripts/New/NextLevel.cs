using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string nextSceneName;
    public AudioClip respawnSFX; 
    private AudioSource audioSource; 
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
        audioSource.PlayOneShot(respawnSFX);
        yield return SceneManager.LoadSceneAsync(currentSceneName, 0);
        // restart current level
        print("Level reset!");
    }
}
