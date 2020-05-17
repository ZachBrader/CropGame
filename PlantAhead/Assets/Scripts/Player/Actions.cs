﻿using System.Collections;
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
    [SerializeField]
    private Vector3Int curCellPosition;
    private Inventory playerInventory;

    private Movement movement;

    [SerializeField]
    private bool nearWater = false;

    // Start is called before the first frame update
    void Start()
    {
        EquipItem(firstSelectedItem);
        grid = gridObject.GetComponent<Grid>();

        playerInventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();
        curSelectionSprite = GameObject.Instantiate(selectionSprite) as GameObject;
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
        Vector3 offset = new Vector3(cellPosition.x + 0.5f + movement.direction.x, cellPosition.y - 0.5f + movement.direction.y, cellPosition.z);
        //print("Player position: " + transform.position);
        //print("Found Cell: " + cellPosition);

            curSelectionSprite.transform.position = offset;
            curCellPosition = cellPosition;

    }

    public void OnTriggerEnter2D(Collider2D collison) 
    {
        if(collison.CompareTag("Water"))
        {
            nearWater = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collison) 
    {
        if(collison.CompareTag("Water"))
        {
            nearWater = false;
        }
    }



}