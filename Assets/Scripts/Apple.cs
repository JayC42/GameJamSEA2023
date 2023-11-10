using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public Inventory inventory;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.PickItem();
            inventory.gameObject.GetComponent<Inventory>().PickFood();
            Destroy(gameObject);
        }
    }
}
