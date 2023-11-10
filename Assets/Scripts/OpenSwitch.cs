using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSwitch : MonoBehaviour
{
    Animator animator;
    public bool open = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            animator.SetTrigger("Open");
            open = true;
        }
    }
}
