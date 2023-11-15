using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideZoneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject outlayer;
    void Start()
    {
        outlayer.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            outlayer.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            outlayer.SetActive(false);
        }
    }
}
