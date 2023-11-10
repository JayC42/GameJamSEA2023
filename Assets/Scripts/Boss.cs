using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask player;
    private float cooldownTimer = Mathf.Infinity;
    private PlayerHealth currentHealth;

    Animator animator;
    Rigidbody2D rb;
    AudioManager audioManager;
    LastGate lastGate;
    public GameObject Player;
    public BossArea BossArea;
    private float speed = 3f;
    private float distance;
    private bool isAttack = false;

    [Header("BossHealth")]
    public int health;

    [Header("Invincible Time")]
    public float duration;
    public bool invincible;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        lastGate = FindObjectOfType<LastGate>().GetComponent<LastGate>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                attack();
            }
        }

        if (health <= 0)
        {
            Die();
            lastGate.GateOpen();
        }

        Vector3 scale = transform.localScale;

        distance = Vector2.Distance(transform.position, Player.transform.position);

        if (health > 0 && BossArea.playerIn && !isAttack)
        {
            animator.SetBool("Move", true);
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        if (Player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        if (health > 0 && !invincible)
        {
            animator.SetTrigger("Hit");
            health -= damage;

            if (health >= 1)
            {
                StartCoroutine(Invulnerability());
            }

            rb.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
        }
    }

    public void Die()
    {
        animator.SetBool("IsDead", true);
    }

    IEnumerator Invulnerability()
    {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
    }

    private void attack()
    {
        animator.SetTrigger("Attack");
        isAttack = true;
    }

    private void onMove()
    {
        isAttack = false;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, player);

        if (hit.collider != null)
        {
            currentHealth = hit.transform.GetComponent<PlayerHealth>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            currentHealth.TakeDamage(damage);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void BossAttackSFX()
    {
        audioManager.BossAttack();
    }

    public void BossDieSFX()
    {
        audioManager.BossDie();
    }

    public void BossHurtSFX()
    {
        audioManager.BossHurt();
    }
}
