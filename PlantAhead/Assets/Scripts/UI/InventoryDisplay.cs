using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour
{

    public GameObject player;
    private Inventory playerInventory;

    public GameObject slotsParent;
    private List<Slot> allSlots;

    private List<Item> curPlayerInv;

    private bool isOpen;

    public TMP_Text ItemDescriptorText;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = player.GetComponent<Inventory>();
        allSlots = new List<Slot>();
        curPlayerInv = new List<Item>();

        // Maintain list of all slots
        foreach (Transform child in slotsParent.transform)
        {
            if (child.gameObject.tag == "Slot")
            {
                allSlots.Add(child.gameObject.GetComponent<Slot>());
            }
        }

        isOpen = slotsParent.activeSelf;

        // Temporary fix -- Need to instantiate slots
        if (!isOpen)
        {
            toggleInventory();
            toggleInventory();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            checkInventoryAndUpdateVisuals();
        }
    }

    public void toggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen == false)
        {
            slotsParent.SetActive(false);
        }
        else
        {
            slotsParent.SetActive(true);
        }
    }

    void checkInventoryAndUpdateVisuals()
    {
        curPlayerInv = playerInventory.GetPlayerInventory();

        if (curPlayerInv == null)
        {
            Debug.Log("Not Instantiated Yet");
            return;
        }
        else
        {
            int count = 0;
            foreach (Item itemInInv in curPlayerInv)
            {
                allSlots[count].setItem(itemInInv);
                count++;
            }
            for (int i = count; i < allSlots.Count; i++)
            {
                allSlots[i].setItem();
            }
        }
    }

    public void UpdateSelectorText(string newDescriptor)
    {
        ItemDescriptorText.text = newDescriptor;
    }
}
