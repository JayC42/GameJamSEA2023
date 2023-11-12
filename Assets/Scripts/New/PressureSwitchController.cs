using UnityEngine;

public class PressureSwitchController : MonoBehaviour
{
    public GameObject door; // Reference to the door that should be opened
    public bool doorisOpened = false;
    private void Update()
    {
        //CheckPressurePlates();

        if (doorisOpened) {  door.SetActive(false); }

    }

}