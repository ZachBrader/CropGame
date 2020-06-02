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

    public TMP_Text ItemDescriptorText;

    // Start is called before the first frame update
    void Start()
    {
        allSlots = new List<Slot>();
        curStock = new List<Item>();
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        // Maintain list of all slots
        Transform slot;
        foreach (Transform child in storeDisplayParent.transform)
        {
            if (child.gameObject.tag == "Slot")
            {
                allSlots.Add(child.gameObject.GetComponent<Slot>());
            }
        }

        isOpen = storeDisplayParent.activeSelf;
        storeDisplayParent.SetActive(true);
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

        //backgroundRender.SetAlpha(0);
        //textRender.SetAlpha(0);
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

    public void UpdateSelectorText(string newDescriptor)
    {
        ItemDescriptorText.text = newDescriptor;
    }

    public void BuyItem(Item itemToBuy)
    {
        store.BuyItemFromStore(itemToBuy);
        string buyText = "Bought " + itemToBuy.itemName + " for " + itemToBuy.cost + " gold"; 
        StartCoroutine(Notification(buyText));
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
        yield return null;
    }
}
