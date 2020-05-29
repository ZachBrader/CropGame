using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    protected Item containedItem = null;
    private InventoryDisplay inventoryDisplay;
    private Inventory playerInventory;
    protected GameObject slotSelector;
    protected Image slotImage;

    private bool isOff = false;
    protected bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    public virtual void SetUp()
    {
        inventoryDisplay = GameObject.FindGameObjectWithTag("InventoryDisplay").GetComponent<InventoryDisplay>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        slotSelector = GameObject.FindGameObjectWithTag("InventorySelector");
        slotImage = GetComponent<Image>();
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem()
    {
        if (!isReady || containedItem == null)
        {
            return;
        }

        playerInventory.EquipItemFromInventory(containedItem);
    }

    public virtual void setItem(Item newItem)
    {
        if (!isReady)
        {
            return;
        }
        if (isOff || containedItem == null)
        {
            containedItem = newItem;
            slotImage.sprite = newItem.icon;
            slotImage.color = new Color(1f, 1f, 1f, 1f);
            isOff = false;
        }
    }

    public virtual void setItem()
    {
        if (!isReady)
        {
            return;
        }
        if (!isOff)
        {
            containedItem = null;
            slotImage.color = new Color(1f, 1f, 1f, 0f);
            isOff = true;
        }

    }

    public virtual void UpdateItemDescriptor()
    {
        slotSelector.SetActive(true);
        slotSelector.transform.position = transform.position;
        if (containedItem != null)
        {
            string newDescription = "Item Name: " + containedItem.itemName;

            inventoryDisplay.UpdateSelectorText(newDescription);
        }
    }

    public virtual void RemoveItemDescriptor()
    {
        slotSelector.SetActive(false);
        inventoryDisplay.UpdateSelectorText("---");
    }
}