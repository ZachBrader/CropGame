using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StoreDisplay : MonoBehaviour
{
    private Inventory playerInventory;
    public Store store;
    public TMP_Text playerGold;
    public GameObject notificationPrefab;
    public Vector3 initNotificationPosition;

    public GameObject storeDisplayParent;
    private List<Slot> allSlots;

    private List<Item> curStock;

    private bool isOpen;

    public Sprite restingImage;

    private Image itemIconImage;
    private TMP_Text itemDescriptorText;
    private TMP_Text costText;
    private TMP_Text sellText;
    private TMP_Text reusableText;
    private TMP_Text waterLevelText;
    private TMP_Text spreadText;
    
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.20f;
        allSlots = new List<Slot>();
        curStock = new List<Item>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
        // Maintain list of all slots
        Transform slotParent = storeDisplayParent.transform.Find("slotParent");
        foreach (Transform child in slotParent)
        {
            if (child.gameObject.tag == "Slot")
            {
                allSlots.Add(child.gameObject.GetComponent<Slot>());
            }
        }

        isOpen = storeDisplayParent.activeSelf;
        storeDisplayParent.SetActive(true);

        Transform objParent = storeDisplayParent.transform.Find("ItemDescriptor");
        itemDescriptorText = objParent.Find("ItemDescriptorText").GetComponent<TMP_Text>();
        itemIconImage = objParent.Find("ItemIconImage").GetComponent<Image>();
        costText = objParent.Find("ItemCost/CostText").GetComponent<TMP_Text>();
        sellText = objParent.Find("ItemSellValue/SellText").GetComponent<TMP_Text>();
        reusableText = objParent.Find("Reusable/ReusableText").GetComponent<TMP_Text>();
        waterLevelText = objParent.Find("WaterLevels/WaterLevelsText").GetComponent<TMP_Text>();
        spreadText = objParent.Find("SpreadRate/SpreadText").GetComponent<TMP_Text>();

        checkStockAndUpdateVisuals();
        storeDisplayParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            checkStockAndUpdateVisuals();
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
        storeDisplayParent.SetActive(true);
        isOpen = true;
    }

    public void closeDisplay()
    {


        storeDisplayParent.SetActive(false);
        isOpen = false;
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
            playerGold.text = playerInventory.GetGold() + " g";
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

    public void UpdateSelectorText(Item itemToShow)
    {
        if (itemToShow != null)
        {
            itemDescriptorText.text = itemToShow.itemName;
            itemIconImage.sprite = itemToShow.icon;
            costText.text = itemToShow.cost.ToString();

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
            costText.text = "-";
            sellText.text = "-";
            reusableText.text = "---";
            waterLevelText.text = "---";
            spreadText.text = "---";
        }
    }

    public void BuyItem(Item itemToBuy)
    {
        if (store.BuyItemFromStore(itemToBuy))
        {
            audioSource.Play();
            string buyText = "-" + itemToBuy.cost + " Gold";
            StartCoroutine(Notification(buyText));
        }
    }

    IEnumerator Notification(string buyText)
    {
        GameObject notificationBackground = Instantiate(notificationPrefab);
        notificationBackground.transform.SetParent(storeDisplayParent.transform);
        notificationBackground.transform.localPosition = initNotificationPosition;

        TMP_Text notificationText = notificationBackground.transform.Find("NotificationText").GetComponent<TMP_Text>();
        notificationText.text = buyText;
        CanvasRenderer backgroundRender = notificationBackground.GetComponent<CanvasRenderer>();
        CanvasRenderer textRender = notificationText.GetComponent<CanvasRenderer>();

        while (backgroundRender.GetAlpha() > 0)
        {
            backgroundRender.SetAlpha(backgroundRender.GetAlpha() - (float)(Time.deltaTime) );
            textRender.SetAlpha(textRender.GetAlpha() - (float)(Time.deltaTime));
            notificationBackground.transform.position = new Vector3(notificationBackground.transform.position.x, notificationBackground.transform.position.y - (float)(Time.deltaTime) * 100, notificationBackground.transform.position.z);
            yield return null;
        }
        Destroy(notificationText);
        Destroy(notificationBackground);
        yield return null;
    }
}
