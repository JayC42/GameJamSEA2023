using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartGame()
    {
        animator.SetTrigger("Press");
        StartCoroutine(StartButtonAnimation());
    }

    public void QuitGame()
    {
        animator.SetTrigger("Press");
        StartCoroutine(QuitButtonAnimation());
    }

    IEnumerator StartButtonAnimation()
    {
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator QuitButtonAnimation()
    {
        yield return new WaitForSeconds(1.4f);
        Application.Quit();
    }
}

   