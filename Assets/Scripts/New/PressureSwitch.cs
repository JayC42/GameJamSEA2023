using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    public PressureSwitchController psc; 
    public GameObject[] requiredObjects; // Array of objects that is p to be on the pressure plate
    [SerializeField] private int objectOnPlate;
    public int ObjectOnPlate => objectOnPlate;
    public bool plateIsActive => objectOnPlate == 1;
    private void Update()
    {
        if (plateIsActive)
        {
            psc.doorisOpened = true;
        }
        else
        {
            psc.doorisOpened = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering object is one of the required objects
        if (ArrayContains(requiredObjects, other.gameObject))
        {
            objectOnPlate++;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object is one of the required objects
        if (ArrayContains(requiredObjects, other.gameObject))
        {
            objectOnPlate--;
            GetComponent<SpriteRenderer>().color = Color.white; 
        }
    }

    bool ArrayContains(GameObject[] array, GameObject obj)
    {
        // Check if the array contains the specified object
        foreach (GameObject item in array)
        {
            if (item == obj)
            {
                return true;
            }
        }
        return false;
    }
}
