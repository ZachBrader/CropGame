using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    List<Item> playerInventory;
    bool isReady = false;

    private Actions player;
    public TMP_Text playerGoldTrackerText;

    public int startingGold = 0;
    private int curGold = 0;

    public int maxItems = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new List<Item>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actions>();
        ReceiveGold(startingGold);
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsReady()
    {
        return isReady;
    }

    public int GetGold()
    {
        return curGold;
    }

    public int SpendGold(int goldToSpend)
    {
        curGold -= goldToSpend;
        playerGoldTrackerText.text = "Gold: " + curGold + "g";
        return curGold;
    }

    public int ReceiveGold(int goldReceived)
    {
        curGold += goldReceived;
        playerGoldTrackerText.text = "Gold: " + curGold + "g";
        return curGold;
    }

    public void ConfirmPurchase(Item itemToAdd)
    {
        if (curGold >= itemToAdd.cost)
        {
            SpendGold(itemToAdd.cost);
            AddItemToInventory(itemToAdd);
            Debug.Log("Able to Purchase " + itemToAdd.itemName);
        }
        else
        {
            Debug.Log("Unable to buy item -- Too little gold");
        }
    }

    public void AddItemToInventory(Item itemToAdd)
    {
        if (playerInventory.Count + 1 <= maxItems)
        {
            playerInventory.Add(itemToAdd);

            if (playerInventory.Count == 1)
            {
                EquipItemFromInventory(itemToAdd);
            }
        }
    }

    public void RemoveItemFromInventory(Item itemToRemove)
    {
        playerInventory.Remove(itemToRemove);
    }


    public List<Item> GetPlayerInventory()
    {
        if (!isReady)
        {
            return null;
        }
        return playerInventory;
    }

    public void EquipItemFromInventory(Item itemToEquip)
    {
        if (playerInventory.Contains(itemToEquip))
        {
            Debug.Log("Equipping " + itemToEquip.itemName);
            player.EquipItem(itemToEquip);
        }
        else
        {
            Debug.Log("Player does not have item -- Unable to equip");
        }
    }
}
