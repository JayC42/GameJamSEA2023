using UnityEngine;

public class AIFollowerHealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public float healthRegenRate = 2f; 
    public float healthDepletionRate = 1f;
    public float lightDepletionRate;
 
    public ParticleSystem regenParticle;    // Particle system for health regen
    public ParticleSystem fullyHealedParticle;    // Particle system for health 100

    // Test
    private NextLevel nextLevel;
    private Transform lightSource;
    private SMask lightRange;
    public float currentHealth;
    private bool isInRegenZone = false;

    private void Start()
    {
        // Ensure maxHealth doesn't exceed 100
        maxHealth = Mathf.Min(maxHealth, 100f);
        currentHealth = maxHealth;
        lightSource = transform.Find("LightSource");
        lightRange = lightSource.GetComponent<SMask>();
        nextLevel = FindObjectOfType<NextLevel>();

    }

    private void Update()
    {
        if (isInRegenZone)
        {
            RegenerateHealth();
        }
        else
        {
            DepleteHealth();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("RegenZone"))
        {
            isInRegenZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RegenZone"))
        {
            isInRegenZone = false;
        }
    }

    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            regenParticle.Play();
        }
        else if (currentHealth == maxHealth)
        {
            // Play particle effect when HP is 100
            if (!regenParticle.isPlaying)
            {
                regenParticle.Stop();
                fullyHealedParticle.Play();
            }
        }
    }
    private void DepleteHealth()
    {
        if (currentHealth > 0)
        {
            float healthLoss = healthDepletionRate * Time.deltaTime;
            currentHealth -= healthLoss;
            currentHealth = Mathf.Max(currentHealth, 0f);

            if (currentHealth <= 80 && currentHealth > 70) { lightSource.localScale = new Vector3(4.5f, 4.5f, 4.5f); }
            else if (currentHealth <= 70 && currentHealth > 60) { lightSource.localScale = new Vector3(4.0f, 4.0f, 4.0f); }
            else if (currentHealth <= 60 && currentHealth > 50) { lightSource.localScale = new Vector3(3.5f, 3.5f, 3.5f); }
            else if (currentHealth <= 50 && currentHealth > 40) { lightSource.localScale = new Vector3(3.0f, 3.0f, 3.0f); }
            else if (currentHealth <= 40 && currentHealth > 30) { lightSource.localScale = new Vector3(2.5f, 2.5f, 2.5f); }
            else if (currentHealth <= 30 && currentHealth > 20) { lightSource.localScale = new Vector3(2.0f, 2.0f, 2.0f); }
            else if (currentHealth <= 20 && currentHealth > 10) { lightSource.localScale = new Vector3(1.5f, 1.5f, 1.5f); }
            else if (currentHealth <= 10 && currentHealth > 1) { lightSource.localScale = new Vector3(1.0f, 1.0f, 1.0f); }
            else if (currentHealth <= 1) 
            { 
                lightSource.localScale = new Vector3(0, 0, 0); 
            }

            // Flicker light range variable gradually
            //lightRange.flickTime += lightDepletionRate * Time.deltaTime;
            //lightRange.flickTime = Mathf.Clamp(lightRange.flickTime, 0.05f, 0.5f);

        }
        else if (currentHealth <= 0)
        {
            // restart current level
            StartCoroutine(nextLevel.RestartCurrentLevel());
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
