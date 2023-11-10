using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class EndingWord : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;



    private int index;



    private bool isTyping = false;



    private bool dialogueFinished = false;



    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                isTyping = false;
            }
            else
            {
                if (dialogueFinished)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
                }
                else if (textComponent.text == lines[index])
                {
                    NextLine();
                }
            }
        }
    }



    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }



    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;



        if (index == lines.Length - 1)
        {
            dialogueFinished = true;
        }
    }



    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueFinished = true;
        }
    }
}