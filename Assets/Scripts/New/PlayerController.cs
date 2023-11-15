using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    //Animator animator;
    SquashAndStretch squashAndStretch;
    public CameraShake shake;

    private float yVelocity;

    private Vector2 moveInput;
    private int sceneNumber;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Raycasts")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [Header("Basic Movement")]
    private float horizontal;
    private float speed = 8f;

    [SerializeField]
    private float minJumpStrength = 5f;
    [SerializeField]
    private float extraJumpStrength = 10f;
    [SerializeField]
    private float jumpHoldLength = 0.5f;
    
    private float jumpHoldTimer;

    [SerializeField]
    private float jumpBufferTime = 0.3f;

    private float jumpBufferTimer;

    private bool holdingJump;
    [SerializeField]
    private float jumpingPower = 6f;
    private bool isFacingRight = true;
    private bool wasGrounded;

    [Header("Wall Jump")]
    [SerializeField]
    private bool isWallSliding;
    [SerializeField]
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField]
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField]
    private float wallJumpingDuration = 0.4f;
    [SerializeField]
    private Vector2 wallJumpingPower = new Vector2(8f, 8f);

    [Header("Dash")]
    [SerializeField]
    private float dashDistance = 5f;
    [SerializeField]
    private float dashingPower = 24f;
    [SerializeField]
    private float dashingTime = 0.2f;
    [SerializeField]
    private float dashingCooldown = 1.5f;
    private bool canDash = true;
    private bool isDashing; 

    [Header("Coyote Time")]
    [SerializeField]
    private float coyoteTime = 0.3f;
    private float coyoteTimer;


    [Header("Particles")]
    [SerializeField]
    private ParticleSystem jumpParticles;
    [SerializeField]
    private ParticleSystem landParticles;
    [SerializeField]
    private ParticleSystem moveParticles;
    [SerializeField]
    private ParticleSystem dashParticles;

    // Buff Timers
    private bool isImmune = false;
    private float immunityTimer = 0f;

    public int SceneNumber { get => sceneNumber; set => sceneNumber = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        squashAndStretch = GetComponent<SquashAndStretch>();

        maxHealth = Mathf.Min(maxHealth, 100f);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is paused
        if (PauseMenu.GameIsPaused)
        {
            // Reset movement input when paused
            horizontal = 0f;
            return;
        }
        //BaseAnimations();
        #region Buff
        // Check if immunity has expired
        if (isImmune)
        {
            immunityTimer -= Time.deltaTime;

            if (immunityTimer <= 0)
            {
                EndImmunity();
            }
        }
        #endregion 
        #region Movement
        if (!wasGrounded && IsGrounded())
        {
            this.landParticles.Play();
        }
        wasGrounded = IsGrounded();

        if (isDashing) return;

        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        WallSlide();

        if (IsWalled())
        {
            //animator.SetBool("WallSlide", true);
            //animator.SetFloat("Speed", 0);
        }
        else
        {
            //animator.SetBool("WallSlide", false);
        }

        WallJump();

        #endregion
    }
    private void BaseAnimations()
    {
        //animator.SetFloat("Speed", Mathf.Abs(horizontal));
        //animator.SetBool("IsGrounded", IsGrounded());
        //animator.SetFloat("yVelocity", rb.velocity.y);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        // Check if the player is currently dashing
        if (isDashing) return;

        if (context.performed && IsGrounded())
        {
            if (!PauseMenu.GameIsPaused)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                //animator.SetTrigger("Jump");
                if (jumpingPower < 6f)
                {
                    squashAndStretch.SquashStretch(0.9f, 1.3f, 0.3f);
                    this.jumpParticles.Play();
                }
                else
                {
                    squashAndStretch.SquashStretch(0.9f, 1.4f, 1f);
                }
            }
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            if (!PauseMenu.GameIsPaused)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                //animator.SetTrigger("Jump");
            }
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    public void Dash(InputAction.CallbackContext context)
    {
        // Check if the player is currently jumping
        if (!IsGrounded()) return;

        if (context.performed && canDash)
        {
            if (!PauseMenu.GameIsPaused)
            {
                StartCoroutine(Dash(dashDistance));
            }
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!PauseMenu.GameIsPaused)
        {
            horizontal = context.ReadValue<Vector2>().x;
            this.moveParticles.Play();
        }
        else
        {
            // Reset movement input when paused
            horizontal = 0f;
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && rb.velocity.y != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private IEnumerator Dash(float dashDistance)
    {
        squashAndStretch.SquashStretch(1.3f, 0.8f, 0.3f);
        shake.ShakeCamera();
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Calculate the dash velocity based on the dash distance
        float dashVelocity = (dashDistance / dashingTime) * Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(dashVelocity, rb.velocity.y);
        this.dashParticles.Play();
        yield return new WaitForSeconds(dashingTime);

        // Stop dashing
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; 
    }
    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0)
        {
            // restart current level
        }
        else 
        {
            currentHealth -= amount;
        }

    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    // Call this method when the player reaches a checkpoint
    private void ReachCheckpoint()
    {
        // Get the player's current position
        Vector3 currentPosition = transform.position;

        // Update the checkpoint in the GameManager
        GameManager.Instance.UpdateCheckpoint(currentPosition);

        Debug.Log("Checkpoint reached!");
    }

    // Call this method when the player reaches the end of a level
    private void GoToNextLevel(string nextLevelName)
    {
        // Check the win condition using the GameManager
        GameManager.Instance.NextLevel(nextLevelName);

        Debug.Log("End of level reached!");
    }

    public void ApplyImmunity(float duration)
    {
        isImmune = true;
        immunityTimer = duration;

        // You might want to play a particle effect or apply any visual feedback here
        // For example: PlayParticleEffect();

        Debug.Log("Player is immune to fire for " + duration + " seconds.");
    }

    private void EndImmunity()
    {
        isImmune = false;

        // You might want to stop any visual effects or animations associated with immunity here
        // For example: StopParticleEffect();

        Debug.Log("Player's immunity has worn off.");
    }

    public bool IsImmune
    {
        get { return isImmune; }
    }

    
}
