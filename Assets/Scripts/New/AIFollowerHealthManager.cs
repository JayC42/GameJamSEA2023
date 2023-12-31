using System.Collections;
using UnityEngine;

public class AIFollowerHealthManager : MonoBehaviour
{
    public GameObject AIObject; 
    public float maxHealth = 100f;
    public float healthRegenRate = 2f; 
    public float healthDepletionRate = 1f;
    public float lightDepletionRate;
 
    public ParticleSystem regenParticle;    // Particle system for health regen
    public ParticleSystem fullyHealedParticle;    // Particle system for health 100
    public ParticleSystem deathParticle;    // Particle system for health 100
    private AudioSource audioSource;
    public AudioClip[] sfx;
    // Test
    private NextLevel nextLevel;
    private Transform lightSource;
    private SMask lightRange;
    public float currentHealth;
    private bool isInRegenZone = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Ensure maxHealth doesn't exceed 100
        maxHealth = Mathf.Min(maxHealth, 100f);
        currentHealth = maxHealth;
        lightSource = transform.Find("LightSource");
        lightRange = lightSource.GetComponent<SMask>();
        nextLevel = FindObjectOfType<NextLevel>();
        regenParticle.gameObject.SetActive(false);
        fullyHealedParticle.gameObject.SetActive(false);
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
            regenParticle.gameObject.SetActive(false);
            fullyHealedParticle.gameObject.SetActive(false);
        }
    }
    public void PlaySFX(AudioClip sfx)
    {
        if (audioSource != null && sfx != null)
        {
            audioSource.PlayOneShot(sfx);
            Debug.Log("Playing SFX: " + sfx.name);
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
        if (currentHealth >= maxHealth)
        {
            if (!regenParticle.isPlaying)
            {
                // Play fullyHealedParticle when health reaches maxHealth
                fullyHealedParticle.gameObject.SetActive(true);
                PlaySFX(sfx[1]);
            }

            regenParticle.gameObject.SetActive(false);
        }
        else
        {
            if (!fullyHealedParticle.isPlaying)
            {
                regenParticle.gameObject.SetActive(true);
                //PlaySFX(sfx[0]);
            }
            // Increase health
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);

            if (currentHealth <= 1) { lightSource.localScale = new Vector3(1.0f, 1.0f, 1.0f); }
            else if (currentHealth <= 20 && currentHealth > 10) { lightSource.localScale = new Vector3(1.5f, 1.5f, 1.5f); }
            else if (currentHealth <= 30 && currentHealth > 20) { lightSource.localScale = new Vector3(2.0f, 2.0f, 2.0f); }
            else if (currentHealth <= 40 && currentHealth > 30) { lightSource.localScale = new Vector3(2.5f, 2.5f, 2.5f); }
            else if (currentHealth <= 50 && currentHealth > 40) { lightSource.localScale = new Vector3(3.0f, 3.0f, 3.0f); }
            else if (currentHealth <= 60 && currentHealth > 50) { lightSource.localScale = new Vector3(3.5f, 3.5f, 3.5f); }
            else if (currentHealth <= 70 && currentHealth > 60) { lightSource.localScale = new Vector3(4.0f, 4.0f, 4.0f); }
            else if (currentHealth <= 80 && currentHealth > 70) { lightSource.localScale = new Vector3(4.5f, 4.5f, 4.5f); }
            else if (currentHealth <= 100 && currentHealth > 80) { lightSource.localScale = new Vector3(5.0f, 5.0f, 5.0f); }

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
            StartCoroutine(HandleDeath());
        }
    }
    public IEnumerator HandleDeath()
    {
        deathParticle.Play();

        this.enabled = false;
        this.GetComponentInChildren<Renderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(nextLevel.RestartCurrentLevel());
        deathParticle.Stop();
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
