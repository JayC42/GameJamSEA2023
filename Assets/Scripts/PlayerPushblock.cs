using UnityEngine;

public class PlayerPushBlock : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        // Check if the player is colliding with an object tagged as "Block"
        if (collision.gameObject.CompareTag("Block"))
        {
            AudioManager.instance.PlaySingleSFX(AudioManager.instance.sfxClips[11]);
        }
    }
}
