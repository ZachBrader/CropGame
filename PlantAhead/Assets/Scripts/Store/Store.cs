using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Inventory playerInventory;

    private List<Item> stock;
    private bool isReady = false;

    public Seed stockSeed;


    // Start is called before the first frame update
    void Start()
    {
        stock = new List<Item>();
        isReady = true;

        AddItemToStock(stockSeed);

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            BuyItemFromStore(stockSeed);
        }
    }

    public void AddItemToStock(Item itemToAdd)
    {
        Debug.Log("Adding " + itemToAdd.itemName + " to stock!");

        stock.Add(itemToAdd);

        foreach (Item itemInInv in stock)
        {
            Debug.Log("stock currently has: " + itemInInv.itemName);
        }
    }

    public void RemoveItemFromStock(Item itemToRemove)
    {
        Debug.Log("removing " + itemToRemove.itemName + " to stock!");

        stock.Remove(itemToRemove);

        foreach (Item itemInInv in stock)
        {
            Debug.Log("stock currently has: " + itemInInv.itemName);
        }
    }

    public void BuyItemFromStore(Item itemToBuy)
    {
        if (stock.Contains(itemToBuy))
        {
            playerInventory.AddItemToInventory(itemToBuy);
        }
    }
}
