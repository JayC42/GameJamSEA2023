using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAiChase : MonoBehaviour
{
    public GameObject player;

    private float speed = 3f;

    private float distance;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 10)
        {
            animator.SetBool("Move", true);
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        if (player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }

        transform.localScale = scale;
    }
}
