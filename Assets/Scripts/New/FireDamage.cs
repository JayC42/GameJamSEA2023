using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [Range(1f, 50f)]
    public float damageRate = 20f; // Damage rate per second
    private bool isInFireRange = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            isInFireRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            isInFireRange = false;
        }
    }

    private void Update()
    {
        // Check if the player is in the fire range
        if (isInFireRange)
        {
            // Assuming you have a Player script that handles immunity
            PlayerController player = FindObjectOfType<PlayerController>();

            if (player != null && !player.IsImmune)
            {
                // Damage the player continuously if not immune
                player.TakeDamage(damageRate * Time.deltaTime);
                player.FlashRed();
                //player.PlaySFX(player.sfx[3]); 
            }
        }
    }
}
