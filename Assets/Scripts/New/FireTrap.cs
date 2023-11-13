using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public float activationInterval = 5f; // Time interval for flames to be active
    public float cooldownInterval = 5f;   // Time interval for cooldown

    private bool isTrapActive = false;
    private float timer = 0f;

    private BoxCollider2D trapCollider;
    private ParticleSystem flameParticles;

    private void Start()
    {
        trapCollider = GetComponent<BoxCollider2D>();
        flameParticles = GetComponentInChildren<ParticleSystem>();
        if (flameParticles == null)
        {
            Debug.LogError("Flame particles not found. Make sure to attach a Particle System to the flamethrower trap.");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isTrapActive)
        {
            // Flames are active, check if it's time to start cooldown
            if (timer >= activationInterval)
            {
                DeactivateTrap();
            }
        }
        else
        {
            // Cooldown is active, check if it's time to start flames
            if (timer >= cooldownInterval)
            {
                ActivateTrap();
            }
        }
    }

    private void ActivateTrap()
    {
        isTrapActive = true;
        timer = 0f;

        // Trigger animation or any other visual effects
        // For example, you can play an animation if the flamethrower has a moving part
        // animator.SetTrigger("Activate");

        // Enable collider to affect the player
        trapCollider.enabled = true;

        // Start the flame particles
        flameParticles.Play();

        //Debug.Log("Flamethrower trap activated.");
    }

    private void DeactivateTrap()
    {
        isTrapActive = false;
        timer = 0f;

        // Trigger animation or any other visual effects
        // animator.SetTrigger("Deactivate");

        // Disable collider to stop affecting the player
        trapCollider.enabled = false;

        // Stop the flame particles
        flameParticles.Stop();

        //Debug.Log("Flamethrower trap deactivated.");
    }
}
