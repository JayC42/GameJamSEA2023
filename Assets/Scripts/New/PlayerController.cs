using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CapsuleCollider2D collider;
    Rigidbody2D rb;
    //Animator animator;
    SquashAndStretch squashAndStretch;
    public CameraShake shake;
    public Transform headCheck;
    public float headCheckLength;

    private NextLevel nextLevel;
    private float yVelocity;
    private Vector2 moveInput;
    private int sceneNumber;
    private bool isDialogActive; // New variable to track if dialogue is active
    private PlayerDamageFlash damageFlash;
  
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

    [Header("Duck")]
    [SerializeField]
    Vector2 normalHeight;
    [SerializeField] 
    private float crouchHeight; 
    float yInput; 

    [Header("Coyote Time")]
    [SerializeField]
    private float coyoteTime = 0.3f;
    private float coyoteTimer;

    private AudioSource audioSource;
    public AudioClip[] sfx;


    [Header("Particles")]
    [SerializeField]
    private ParticleSystem jumpParticles;
    [SerializeField]
    private ParticleSystem landParticles;
    [SerializeField]
    private ParticleSystem moveParticles;
    [SerializeField]
    private ParticleSystem dashParticles;
    [SerializeField]
    private ParticleSystem deathParticles;
    [SerializeField]
    private GameObject protectiveBarrier;
    // Buff Timers
    private bool isImmune = false;
    private float immunityTimer = 0f;
    private SpriteRenderer renderer;

    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f); // Red color with 50% transparency
    public int SceneNumber { get => sceneNumber; set => sceneNumber = value; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        nextLevel = FindObjectOfType<NextLevel>();
        //animator = GetComponent<Animator>();
        squashAndStretch = GetComponent<SquashAndStretch>();
        damageFlash = GetComponent<PlayerDamageFlash>();
        maxHealth = Mathf.Min(maxHealth, 100f);
        currentHealth = maxHealth;
        normalHeight = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is paused
        if (PauseMenu.GameIsPaused || isDialogActive)
        {
            // Reset movement input when paused
            horizontal = 0f;
        }

        yInput = Input.GetAxisRaw("Vertical");
        //Crouch(); 
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
            PlaySFX(sfx[1]);
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
    public void PlaySFX(AudioClip sfx)
    {
        if (audioSource != null && sfx != null)
        {
            audioSource.PlayOneShot(sfx);
            Debug.Log("Playing SFX: " + sfx.name);
        }
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
        if (isDashing || isDialogActive) return;

        if (context.performed && IsGrounded())
        {
            PlaySFX(sfx[0]);
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
        if (!IsGrounded() || isDialogActive) return;

        if (context.performed && canDash)
        {
            if (!PauseMenu.GameIsPaused)
            {
                StartCoroutine(Dash(dashDistance));
            }
        }
    }
    public void Crouch()
    {
        if (!IsGrounded() || isDialogActive || PauseMenu.GameIsPaused) return;
        bool isHeadHitting = HeadDetect();

        // Save the current scale
        Vector2 currentScale = transform.localScale;
        if (yInput < 0 || isHeadHitting)
        {
            // Set the player's height to the crouch height
            transform.localScale = new Vector2(currentScale.x, crouchHeight);

            if (transform.localScale.y != crouchHeight)
                transform.localScale = new Vector2(normalHeight.x, crouchHeight);
        }
        else
        {
            if (transform.localScale.y != normalHeight.y)
                transform.localScale = normalHeight;
        }
        if ((isFacingRight && currentScale.x < 0) || (!isFacingRight && currentScale.x > 0))
        {
            transform.localScale = new Vector2(-currentScale.x, currentScale.y);
        }
    }
    private void OnDrawGizmos()
    {
        Vector2 from = headCheck.position;
        Vector2 to = new Vector2(headCheck.position.x, headCheck.position.y + headCheckLength);

        Gizmos.DrawLine(from, to);
    }
    private bool HeadDetect()
    {
        return Physics.Raycast(headCheck.position, Vector2.up, headCheckLength, groundLayer); 
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
    public void SetDialogActive(bool active)
    {
        isDialogActive = active;
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
        squashAndStretch.SquashStretch(2.2f, 0.8f, 0.3f);
        shake.ShakeCamera();
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Calculate the dash velocity based on the dash distance
        float dashVelocity = (dashDistance / dashingTime) * Mathf.Sign(transform.localScale.x);
        rb.velocity = new Vector2(dashVelocity, rb.velocity.y);
        this.dashParticles.Play();
        //AudioManager.instance.PlaySingleSFX(AudioManager.instance.sfxClips[2]);
        PlaySFX(sfx[2]);

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
            //PlaySFX(sfx[5]);
            StartCoroutine(HandleDeath());
        }
        else 
        {
            currentHealth -= amount;
            //print("taking dmg!");
        }

    }

    public void FlashRed()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Store the original color of the player
        Color originalColor = renderer.color;

        // Set the player color to the flash color
        renderer.color = flashColor;

        //PlaySFX(sfx[3]);

        // Wait for the specified flash duration
        yield return new WaitForSeconds(flashDuration);

        // Reset the player color to the original color
        renderer.color = originalColor;

        // Make sure ResetColor is called after the coroutine completes
        ResetColor();
    }
    public void ResetColor()
    {
        // Reset the player color to white
        renderer.color = Color.white;
    }
    public IEnumerator HandleDeath()
    {
        deathParticles.Play();
        this.enabled = false;
        this.GetComponentInChildren<Renderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(nextLevel.RestartCurrentLevel());
        deathParticles.Stop();

        //print("Player Respawn");
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void ApplyImmunity(float duration)
    {
        isImmune = true;
        immunityTimer = duration;
        // visual feedback toggle on
        protectiveBarrier.SetActive(true);
        PlaySFX(sfx[6]);
        //Debug.Log("Player is immune to fire for " + duration + " seconds.");
    }

    private void EndImmunity()
    {
        isImmune = false;
        // visual feedback toggle off
        protectiveBarrier.SetActive(false);
        PlaySFX(sfx[7]);
        //Debug.Log("Player's immunity has worn off.");
    }


    public bool IsImmune
    {
        get { return isImmune; }
    }

    
}
