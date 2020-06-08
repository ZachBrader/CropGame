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
    private Actions playerActions;

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

    public GameObject notificationPrefab;
    public Vector3 initNotificationPosition;
    private Image equippedIconImage;
    private TMP_Text equippedText;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = player.GetComponent<Inventory>();
        playerActions = player.GetComponent<Actions>();
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

        Transform objParent = inventoryDisplayParent.transform.Find("ItemEquippedBackground");
        equippedIconImage = objParent.Find("ItemEquippedIcon").GetComponent<Image>();
        equippedText = objParent.Find("ItemEquippedText").GetComponent<TMP_Text>();

        objParent = inventoryDisplayParent.transform.Find("ItemDescriptor");
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

            if (playerActions.GetEquippedItem() != null)
            {
                equippedIconImage.sprite = playerActions.GetEquippedItem().icon;
                equippedText.text = playerActions.GetEquippedItem().itemName;
            }
        }
    }

    public void UpdateSelectorText(Item itemToShow)
    {
        if (itemToShow != null)
        {
            itemDescriptorText.text = itemToShow.itemName;
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

    public void EquipItemNotification(Item itemToEquip)
    {
        if (isOpen)
        {
            StartCoroutine(Notification(itemToEquip.itemName));
        }
    }

    IEnumerator Notification(string buyText)
    {
        GameObject notificationBackground = Instantiate(notificationPrefab);
        notificationBackground.transform.SetParent(inventoryDisplayParent.transform);
        notificationBackground.transform.localPosition = initNotificationPosition;

        TMP_Text notificationText = notificationBackground.transform.Find("NotificationText").GetComponent<TMP_Text>();
        notificationText.text = buyText;
        CanvasRenderer backgroundRender = notificationBackground.GetComponent<CanvasRenderer>();
        CanvasRenderer textRender = notificationText.GetComponent<CanvasRenderer>();

        while (backgroundRender.GetAlpha() > 0)
        {
            backgroundRender.SetAlpha(backgroundRender.GetAlpha() - (float)(Time.deltaTime));
            textRender.SetAlpha(textRender.GetAlpha() - (float)(Time.deltaTime));
            notificationBackground.transform.position = new Vector3(notificationBackground.transform.position.x, notificationBackground.transform.position.y - (float)(Time.deltaTime) * 100, notificationBackground.transform.position.z);
            yield return null;
        }
        Destroy(notificationText);
        Destroy(notificationBackground);
        yield return null;
    }
}
