using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Item> playerInventory;
    bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new List<Item>();
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(Item itemToAdd)
    {
        Debug.Log("Adding " + itemToAdd.itemName + " to inventory!");

        playerInventory.Add(itemToAdd);

        foreach(Item itemInInv in playerInventory)
        {
            Debug.Log("Inventory currently has: " + itemInInv.itemName);
        }
    }

    public void RemoveItemFromInventory(Item itemToRemove)
    {
        Debug.Log("removing " + itemToRemove.itemName + " to inventory!");

        playerInventory.Remove(itemToRemove);

        foreach (Item itemInInv in playerInventory)
        {
            Debug.Log("Inventory currently has: " + itemInInv.itemName);
        }
    }


    public List<Item> GetPlayerInventory()
    {
        if (!isReady)
        {
            return null;
        }
        return playerInventory;
    }
}
