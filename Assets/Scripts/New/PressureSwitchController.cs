using UnityEngine;

public class PressureSwitchController : MonoBehaviour
{
    public GameObject door; // Reference to the door that should be opened
    public bool doorisOpened = false;
    public int maxObjectOnPlate = 2;
    private AudioSource audioSource;
    public AudioClip doorOpenSFX;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        CheckPressurePlates();
    }

    private void CheckPressurePlates()
    {
        int totalObjectsOnPlate = 0;

        // Find all PressureSwitch instances in the scene
        PressureSwitch[] pressureSwitches = FindObjectsOfType<PressureSwitch>();

        // Iterate through each PressureSwitch and add its ObjectOnPlate value to the total
        foreach (PressureSwitch pressureSwitch in pressureSwitches)
        {
            totalObjectsOnPlate += pressureSwitch.ObjectOnPlate;
        }

        // Compare the total with maxObjectOnPlate
        if (totalObjectsOnPlate == maxObjectOnPlate)
        {
            door.SetActive(false); // Set the door to inactive
            doorisOpened = true;
            audioSource.PlayOneShot(doorOpenSFX); 
        }
        else
        {
            door.SetActive(true); // Set the door to active
            doorisOpened = false;
        }
    }
}