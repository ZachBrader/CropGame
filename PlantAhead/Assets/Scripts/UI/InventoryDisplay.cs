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

    private GameObject inventoryDisplayParent;
    private List<Slot> allSlots;

    private List<Item> curPlayerInv;

    private bool isOpen;

    public Sprite restingImage;

    private Image itemIconImage;
    private TMP_Text itemDescriptorText;
    private TMP_Text heldText;
    private TMP_Text sellText;
    private TMP_Text reusableText;
    private TMP_Text waterLevelText;
    private TMP_Text spreadText;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = player.GetComponent<Inventory>();
        inventoryDisplayParent = transform.Find("InventoryParent").gameObject;
        allSlots = new List<Slot>();
        curPlayerInv = new List<Item>();

        // Maintain list of all slots
        Transform slotParent = inventoryDisplayParent.transform.Find("slotParent");
        foreach (Transform child in slotParent)
        {
            if (child.gameObject.tag == "Slot")
            {
                allSlots.Add(child.gameObject.GetComponent<Slot>());
            }
        }

        Transform objParent = inventoryDisplayParent.transform.Find("ItemDescriptor");
        itemDescriptorText = objParent.Find("ItemDescriptorText").GetComponent<TMP_Text>();
        itemIconImage = objParent.Find("ItemIconImage").GetComponent<Image>();
        heldText = objParent.Find("NumberHeld/HeldText").GetComponent<TMP_Text>();
        sellText = objParent.Find("ItemSellValue/SellText").GetComponent<TMP_Text>();
        reusableText = objParent.Find("Reusable/ReusableText").GetComponent<TMP_Text>();
        waterLevelText = objParent.Find("WaterLevels/WaterLevelsText").GetComponent<TMP_Text>();
        spreadText = objParent.Find("SpreadRate/SpreadText").GetComponent<TMP_Text>();


        isOpen = inventoryDisplayParent.activeSelf;
        checkInventoryAndUpdateVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            checkInventoryAndUpdateVisuals();
        }
    }

    public bool checkOpen()
    {
        return isOpen;
    }

    public void toggleDisplay()
    {
        if (isOpen == false)
        {
            showDisplay();
        }
        else
        {
            closeDisplay();
        }
    }

    public void showDisplay()
    {
        inventoryDisplayParent.SetActive(true);
        isOpen = true;
    }

    public void closeDisplay()
    {
        inventoryDisplayParent.SetActive(false);
        isOpen = false;
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

    public void UpdateSelectorText(Item itemToShow)
    {
        if (itemToShow != null)
        {
            itemDescriptorText.text = itemToShow.itemName;
            itemIconImage.color = new Color(0, 0, 0, 1);
            itemIconImage.sprite = itemToShow.icon;
            heldText.text = (itemToShow as Seed).seedCount.ToString();

            Plant refToPlant = (itemToShow as Seed).plant.GetComponent<Plant>();
            sellText.text = refToPlant.averagePlantValue.ToString();
            reusableText.text = refToPlant.reusable.ToString();
            waterLevelText.text = refToPlant.PerfectWaterAmount.ToString();
            spreadText.text = refToPlant.SpreadZone.ToString();
        }
        else
        {
            itemDescriptorText.text = "---";
            itemIconImage.sprite = restingImage;
            heldText.text = "-";
            sellText.text = "-";
            reusableText.text = "---";
            waterLevelText.text = "---";
            spreadText.text = "---";
        }
    }
}
