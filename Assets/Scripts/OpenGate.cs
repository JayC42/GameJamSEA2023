using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    Animator animator;
    public OpenSwitch openSwitch;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    public void Update()
    {
        if (openSwitch.open)
        {
            Open();
        }
    }

    public void Open()
    {
        animator.SetTrigger("Open");
    }
}
