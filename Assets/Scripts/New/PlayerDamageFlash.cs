using UnityEngine;
using System.Collections;
public class PlayerDamageFlash : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer;

    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f); // Red color with 50% transparency

    private void Start()
    {
        // Assuming the player has a SpriteRenderer component
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

}
