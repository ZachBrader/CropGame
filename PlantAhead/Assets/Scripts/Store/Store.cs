using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Store : MonoBehaviour
{
    public Inventory playerInventory;

    private List<Item> stock;
    private bool isReady = false;

    public Seed stockSeed;
    public Seed stockSeed1;
    public Seed stockSeed2;
    public Seed stockSeed3;
    public Seed stockSeed4;
    public Seed stockSeed5;
    public Seed stockSeed6;
    public Seed stockSeed7;

    

    // Start is called before the first frame update
    void Start(){
        
        stock = new List<Item>();
        isReady = true;

        if (stockSeed != null) { AddItemToStock(stockSeed); }
        if (stockSeed1 != null) { AddItemToStock(stockSeed1); }
        if (stockSeed2 != null) { AddItemToStock(stockSeed2); }
        if (stockSeed3 != null) { AddItemToStock(stockSeed3); }
        if (stockSeed4 != null) { AddItemToStock(stockSeed4); }
        if (stockSeed5 != null) { AddItemToStock(stockSeed5); }
        if (stockSeed6 != null) { AddItemToStock(stockSeed6); }
        if (stockSeed7 != null) { AddItemToStock(stockSeed7); }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddItemToStock(Item itemToAdd)
    {
        stock.Add(itemToAdd);
    }

    public void RemoveItemFromStock(Item itemToRemove)
    {
        stock.Remove(itemToRemove);
    }

    public void BuyItemFromStore(Item itemToBuy)
    {
        if (stock.Contains(itemToBuy))
        {
            playerInventory.ConfirmPurchase(itemToBuy);
            
        }
    }

    public List<Item> GetStock()
    {
        if (isReady)
        {
            return stock;
        }
        return null;
    }
}
