using UnityEngine;

public class WaterZoneScript : MonoBehaviour
{
    public float interactionRange = 2f;
    public float immunityDuration = 10f; // Duration of immunity in seconds

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            GrantImmunity();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void GrantImmunity()
    {
        // Assuming you have a Player script that handles immunity
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null)
        {
            player.ApplyImmunity(immunityDuration);
        }
    }
}