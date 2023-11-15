using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SkipIntro : MonoBehaviour
{
    [SerializeField] GameObject SkipCutSceneButton;
    private void Start()
    {
        SkipCutSceneButton.SetActive(false);
        StartCoroutine(ShowButtonAfterDelay());
    }
    private IEnumerator ShowButtonAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SkipCutSceneButton.SetActive(true); 
    }
    public void SkipCutScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
