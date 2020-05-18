using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public GameObject storeUi;
    public GameObject playerInventoryUIParent;
    public GameObject itemToSell;


    private List<GameObject> playerInventoryUIList;
    private bool isOpen;
    private GameObject player;
    private Inventory playerInventory;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<Inventory>();

        isOpen = storeUi.activeSelf;
        Debug.Log("Store is currently: " + isOpen);

        // Get player items
        Transform[] allChildren = playerInventoryUIParent.GetComponentsInChildren<Transform>();
        playerInventoryUIList = new List<GameObject>();
        foreach (Transform child in allChildren)
        {
            playerInventoryUIList.Add(child.gameObject);
        }
        print(playerInventoryUIList.Count);
        // Remove parent item -- temporary fix
        playerInventoryUIList.RemoveAt(0);
        playerInventoryUIList.RemoveAt(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Toggling store");
            toggleStore();
        }
    }

    void toggleStore()
    {
        if (isOpen)
        {
            closeStore();
        }
        else { showStore(); }
    }

    void showStore()
    {
        print("Number of held items: " + playerInventory.inventoryList.Count);
        if (isOpen == false)
        {
            isOpen = true;
            Debug.Log("Showing Inventory");
            storeUi.SetActive(true);

            int count = 0;
            foreach (GameObject heldItem in playerInventory.inventoryList)
            {
                Debug.Log(playerInventoryUIList[count].GetComponent<Text>().text);
                Debug.Log(heldItem.GetComponent<Item>().name);
                playerInventoryUIList[count].GetComponent<Text>().text = "Item: " + heldItem.GetComponent<Item>().name + " -- " + count;
                count++;
            }
        }
    }

    void closeStore()
    {
        if (isOpen == true)
        {
            isOpen = false;
            Debug.Log("Closing inventory");
            storeUi.SetActive(false);
        }
    }

    public void buyItem(GameObject item)
    {
        playerInventory.addToInventory(itemToSell);
    }
}
