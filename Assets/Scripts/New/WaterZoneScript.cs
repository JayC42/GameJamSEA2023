using UnityEngine;

public class WaterZoneScript : MonoBehaviour
{
    [Range(10f, 60f)]
    public float immunityDuration = 30f; // Duration of immunity in seconds

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

        if (player != null && !player.IsImmune)
        {
            player.ApplyImmunity(immunityDuration);
        }
        else
        {
            Debug.Log("Player already has immunity.");
            // You can add additional logic or feedback if needed
        }
    }
}