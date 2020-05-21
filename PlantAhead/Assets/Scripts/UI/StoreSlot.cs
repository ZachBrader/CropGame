﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : Slot
{
    //private Item containedItem = null;
    private StoreDisplay storeDisplay;
    //private Image slotImage;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    public override void SetUp()
    {
        storeDisplay = GameObject.FindGameObjectWithTag("Store").GetComponent<StoreDisplay>();
        slotImage = GetComponent<Image>();
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateItemDescriptor()
    {
        if (containedItem != null)
        {
            string newDescription = "Store Item Name: " + containedItem.itemName;

            storeDisplay.UpdateSelectorText(newDescription);
        }
    }

    public override void RemoveItemDescriptor()
    {
        storeDisplay.UpdateSelectorText("---");
    }

    public void BuyItem()
    {
        if (containedItem != null)
        {
            Debug.Log("Buying item");
            storeDisplay.BuyItem(containedItem);
        }
    }
}