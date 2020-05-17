using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actions : MonoBehaviour
{
    public GameObject itemSelectedText;
    public GameObject firstSelectedItem;
    public GameObject gridObject;
    public GameObject selectionSprite;

    private Grid grid;
    private GameObject currentEquipped;
    private GameObject curSelectionSprite = null;
    private Vector3Int curCellPosition;
    private Inventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        EquipItem(firstSelectedItem);
        grid = gridObject.GetComponent<Grid>();

        playerInventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        selectCell();
        // Used to interact with a tile
        if (Input.GetKeyDown(KeyCode.L))
        {
            DoAction();
        }

        // Used to equip the currently selected item
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            EquipItem(playerInventory.getitem(0));
        }
    }

    void DoAction()
    {
        if (currentEquipped == null)
        {
            Debug.Log("No item currently equipped -- Cannot Preform Action");
            return;
        }
        currentEquipped.GetComponent<Item>().Use(curCellPosition);
    }

    void EquipItem(GameObject item)
    {
        if (item == null)
        {
            currentEquipped = null;
            itemSelectedText.GetComponent<Text>().text = "Item: None";
            Debug.Log("No item selected");
            return;
        }

        currentEquipped = item;
        itemSelectedText.GetComponent<Text>().text = "Item: " + item.GetComponent<Item>().name;
        Debug.Log("Equipped " + item.GetComponent<Item>().name);
    }

    void selectCell()
    {
        Vector3Int cellPosition = grid.WorldToCell(transform.position);

        //print("Player position: " + transform.position);
        //print("Found Cell: " + cellPosition);

        if (cellPosition != curCellPosition)
        {
            Destroy(curSelectionSprite);
            curSelectionSprite = GameObject.Instantiate(selectionSprite) as GameObject;
            curSelectionSprite.transform.position = cellPosition;

            curCellPosition = cellPosition;
        }
    }

}
