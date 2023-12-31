using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class DialogueSet
{
    public string[] dialogueLines;
}

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public List<DialogueSet> dialogueSets = new List<DialogueSet> { }; // List of dialogue arrays
    private int index;
    public bool dialogueStarted;
    public bool tutorialActive;
    public bool firstInteractionRequired;
    public bool secondInteractionRequired;

    public float wordSpeed;
    public bool playerIsClose;
    public Collider2D firstColliderToDisable; 
    PlayerController pc;
    TutorialText tt;

    private void Start()
    {
        // Make the NPC persist across scenes
        DontDestroyOnLoad(gameObject);

        tt = GetComponentInChildren<TutorialText>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pc.SceneNumber = 1;
        tutorialActive = true; 
    }

    void Update()
    {
        pc.SetDialogActive(dialogueStarted);
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !dialogueStarted)
        { 
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
                // Enable the PlayerController script when exiting dialogue
                EnablePlayerController(true);
            }
            else
            {
                dialogueStarted = true; 
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());

                // Disable the PlayerController script during dialogue
                EnablePlayerController(false);

                // Disable tutorial when dialogue is triggered
                if (tutorialActive)
                {
                    tt.tutorial.SetActive(false);
                    tutorialActive = false;
                    // disable first collider
                    firstColliderToDisable.enabled = false;
                }
            }
        }

        if (pc.SceneNumber <= dialogueSets.Count)
        {
            if (dialogueText.text == dialogueSets[pc.SceneNumber - 1].dialogueLines[index])
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    NextLine();
                }
                if (pc.SceneNumber == 1)
                {
                    AIFollower.introComplete = true;
                }
            }
        }

        // Re-enable tutorial when SceneNumber increases
        if (pc.SceneNumber > dialogueSets.Count && !tutorialActive)
        {
            tt.tutorial.SetActive(true);
            tutorialActive = true;
        }
    }
    void EnablePlayerController(bool enable)
    {
        // Assuming playerController is a reference to the PlayerController script
        if (pc != null)
        {
            pc.enabled = enable;
        }
    }
    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        string[] activeDialogueArray = dialogueSets[pc.SceneNumber - 1].dialogueLines;

        foreach (char letter in activeDialogueArray[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        string[] activeDialogueArray = dialogueSets[pc.SceneNumber - 1].dialogueLines;

        if (index < activeDialogueArray.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
            EnablePlayerController(true);
            dialogueStarted = false; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}