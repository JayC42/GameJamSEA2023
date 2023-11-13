using UnityEngine;

public class AIFollowerHealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public float healthRegenRate = 2f; 
    public float healthDepletionRate = 1f; 

    private float currentHealth;
    private bool isInRegenZone = false;

    private void Start()
    {
        currentHealth = maxHealth;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RegenZone"))
        {
            isInRegenZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
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
            // You may also want to update a health bar UI here
        }
    }

    private void DepleteHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth -= healthDepletionRate * Time.deltaTime;
            currentHealth = Mathf.Max(currentHealth, 0f);
            // You may also want to update a health bar UI here
        }
        else
        {
            // Player is out of health, you can handle this as needed (e.g., player death)
        }
    }
}
