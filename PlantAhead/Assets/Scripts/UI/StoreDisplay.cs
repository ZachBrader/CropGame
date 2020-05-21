using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StoreDisplay : MonoBehaviour
{
    public Store store;

    public GameObject slotsParent;
    private List<Slot> allSlots;

    private List<Item> curStock;

    private bool isOpen;

    public TMP_Text ItemDescriptorText;

    // Start is called before the first frame update
    void Start()
    {
        allSlots = new List<Slot>();
        curStock = new List<Item>();

        // Maintain list of all slots
        foreach (Transform child in slotsParent.transform)
        {
            if (child.gameObject.tag == "Slot")
            {
                allSlots.Add(child.gameObject.GetComponent<Slot>());
            }
        }

        isOpen = slotsParent.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            checkStockAndUpdateVisuals();
        }
    }

    public void toggleDisplay()
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

    void checkStockAndUpdateVisuals()
    {
        curStock = store.GetStock();

        if (curStock == null)
        {
            Debug.Log("Not Instantiated Yet");
            return;
        }
        else
        {
            int count = 0;
            foreach (Item itemInInv in curStock)
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

    public void BuyItem(Item itemToBuy)
    {
        store.BuyItemFromStore(itemToBuy);
    }
}
