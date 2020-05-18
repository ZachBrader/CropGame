using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 5;

    [HideInInspector]
    public List<GameObject> inventoryList;

    public GameObject testItem;

    // Holds the list of text elements to print out the inventory
    public GameObject inventoryUIParent;
    private List<GameObject> inventoryUIList;

    // Used to determine weather inventory is currently open for player
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        Transform[] allChildren = inventoryUIParent.GetComponentsInChildren<Transform>();
        inventoryUIList = new List<GameObject>();
        foreach (Transform child in allChildren)
        {
            inventoryUIList.Add(child.gameObject);
        }
        // Remove parent item -- temporary fix
        inventoryUIList.RemoveAt(0);

        inventoryList = new List<GameObject>();

        addToInventory(testItem);
    }

    // Update is called once per frame
    void Update()
    {
        // Show inventory if player presses the I button
        if (Input.GetKeyDown(KeyCode.I))
        {
            showInventory();
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            closeInventory();
        }
    }

    void showInventory()
    {
        print("Number of held items: " + inventoryList.Count);
        if (isOpen == false)
        {
            isOpen = true;
            Debug.Log("Showing Inventory");
            inventoryUIParent.SetActive(true);

            int count = 0; 
            foreach (GameObject heldItem in inventoryList)
            {
                inventoryUIList[count].GetComponent<Text>().text = "Item: " + heldItem.GetComponent<Item>().name + " -- " + count;
                count++;
            }
        }
    }

    void closeInventory()
    {
        if (isOpen == true)
        {
            isOpen = false;
            Debug.Log("Closing inventory");
            inventoryUIParent.SetActive(false);
        }
    }

    public void addToInventory(GameObject item)
    {
        if (inventoryList.Count >= inventorySize)
        {
            Debug.Log("List is full with " + inventoryList.Count + " items");
        }
        else
        {
            GameObject newItem = GameObject.Instantiate(item) as GameObject;
            //newItem.SetActive(false);
            newItem.transform.parent = transform;
            inventoryList.Add(newItem);
        }
    }

    List<GameObject> getInventory()
    {
        return inventoryList;
    }

    public GameObject getitem(int itemNumber)
    {
        return inventoryList[itemNumber];
    }
}
