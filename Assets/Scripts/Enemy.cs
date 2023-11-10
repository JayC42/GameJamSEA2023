using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    AudioManager audioManager;

    public GameObject player;

    public int health;

    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 0)
        {
            Die();
            DestroyAfterDied();
        }
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        health -= damage;
        audioManager.EnemyHurt();

        if (health > 0)
        {
            if (player.transform.position.x > transform.position.x)
            {
                rb.AddForce(new Vector2(-3, 4), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(3, 4), ForceMode2D.Impulse);
            }
        }
    }

    public void DestroyAfterDied()
    {
        Destroy(this.gameObject, 1.5f);
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
    }
}
