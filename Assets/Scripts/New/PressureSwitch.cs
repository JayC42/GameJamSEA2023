using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    public PressureSwitchController psc; 
    public GameObject[] requiredObjects; // Array of objects required to be on the pressure plate
    public bool allPlatesActive => objectsOnPlate == requiredObjects.Length;
    private int objectsOnPlate;
    private void Update()
    {
        if (allPlatesActive)
        {
            psc.doorisOpened = true;
        }
        else
        {
            psc.doorisOpened = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the entering object is one of the required objects
        if (ArrayContains(requiredObjects, other.gameObject))
        {
            objectsOnPlate++;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting object is one of the required objects
        if (ArrayContains(requiredObjects, other.gameObject))
        {
            objectsOnPlate--;
            GetComponent<SpriteRenderer>().color = Color.white; psc.doorisOpened = allPlatesActive;
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
