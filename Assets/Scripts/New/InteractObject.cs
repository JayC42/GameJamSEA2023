using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public GameObject interactTooltip;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactTooltip.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactTooltip.SetActive(false);
        }
    }
}
