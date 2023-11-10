using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool IsOpen = false;
    public GameObject inventoryUI;
    public PlayerHealth playerHealth;
    public Text FoodNumber;
    public int FoodAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        FoodNumber.text = FoodAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!IsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    public void Open()
    {
        inventoryUI.SetActive(true);
        IsOpen = true;
    }

    public void Close()
    {
        inventoryUI.SetActive(false);
        IsOpen = false;
    }

    public void EatFood()
    {
        if(FoodAmount > 0)
        {
            playerHealth.gameObject.GetComponent<PlayerHealth>().currentHealth += 1;
            FoodAmount--;
            FoodNumber.text = FoodAmount.ToString();
        }
    }

    public void PickFood()
    {
        FoodAmount += 1;
        FoodNumber.text = FoodAmount.ToString();
    }
}
