using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    private ScreenFlash sf;
    private SpriteRenderer sr;
    private Color defaultColor;

    [Header("Player Health")]
    public int health;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Invincible Time")]
    public float duration;
    public bool invincible;

    [Header("Checkpoint")]
    public Checkpoint checkpoint;
    public GameObject Checkpoint;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sf = GetComponent<ScreenFlash>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        currentHealth = health;
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHealth);

        if (currentHealth > health)
        {
            currentHealth = health;
        }

        if (currentHealth <= 0)
        {
            GetComponent<PlayerController>().enabled = false;
            StartCoroutine(Die());
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0 && !invincible)
        {
            sf.FlashScreen();
            animator.SetTrigger("Hit");
            currentHealth -= damage;

            if(currentHealth >= 1)
            {
                StartCoroutine(Invulnerability());
            }

            rb.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
        }

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator Die()
    {
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(2);
        if (!checkpoint.active)
        {
            currentHealth = health;
            animator.SetBool("IsDead", false);
            transform.position = new Vector2(-12, 1);
            healthBar.SetHealth(currentHealth);
            GetComponent<PlayerController>().enabled = true;
        }
        else
        {
            currentHealth = health;
            animator.SetBool("IsDead", false);
            transform.position = new Vector2(88, -15);
            healthBar.SetHealth(currentHealth);
            GetComponent<PlayerController>().enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            if (currentHealth > 0)
            {
                TakeDamage(1);
            }
        }
    }

    IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(3, 9, true);
        sr.color = Color.yellow;
        invincible = true;
        yield return new WaitForSeconds(duration);
        sr.color = defaultColor;
        Physics2D.IgnoreLayerCollision(3, 9, false);
        invincible = false;
    }
}
