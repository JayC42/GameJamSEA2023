using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public GameObject tutorial;
    private NPC npc;

    private void Start()
    {
        npc = GetComponentInParent<NPC>(); 
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && npc.tutorialActive)
        {
            tutorial.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorial.SetActive(false);
        }
    }
}
