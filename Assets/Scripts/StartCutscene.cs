using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("cutscene", true);
            Invoke(nameof(StopCutscene), 3);
        }
    }

    private void StopCutscene()
    {
        animator.SetBool("cutscene", false);
    }
}
