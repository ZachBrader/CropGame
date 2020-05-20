using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class Actions : MonoBehaviour
{
    public GameObject itemSelectedText;
    public GameObject firstSelectedItem;
    public GameObject gridObject;
    public GameObject selectionSprite;

    public Grid grid;
    public Tilemap tillableTiles;
    private GameObject currentEquipped;
    public GameObject curSelectionSprite = null;
    [SerializeField]
    private Vector3Int curCellPosition;
    private Inventory playerInventory;

    public Movement movement;
    private GameManager gameManager;

    //public Image waterBar; in watercan.cs

    [SerializeField]
    private bool nearWater = false; // watercan.cs make it's own check; this may not be needed here

    [SerializeField]
    private bool canSleep = false;
    
    [Header("Plant")] public GameObject plant;

    // Start is called before the first frame update
    void Start(){
        Debug.Log("Actions Start");
        var temp = GameObject.FindGameObjectWithTag("GameManager");
        if (temp != null){
            gameManager = temp.GetComponent<GameManager>();
        }

        // EquipItem(firstSelectedItem);
        //grid = gridObject.GetComponent<Grid>();

        playerInventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();
        curSelectionSprite = GameObject.Instantiate(selectionSprite) as GameObject;
    }

    // Update is called once per frame
    void Update(){
        selectCell();
        // Used to interact with a tile
        if (Input.GetKeyDown(KeyCode.Space)){
            DoAction();
        }

        // Used to equip the currently selected item
        if (Input.GetKeyDown(KeyCode.Alpha0)){
            EquipItem(playerInventory.getitem(0));
        }

        // Temporary Fix to simulate sleeping
        // checks if you're in the house
        if (Input.GetKeyDown(KeyCode.Q) && canSleep){
            gameManager.EndDay();
        }
    }

    void DoAction(){
        if (currentEquipped == null){
            Debug.Log("No item currently equipped -- Cannot Preform Action");
            //return;
        }

        /*if (plant.tag.Equals("Plant")){
            Vector3Int cellPosition = grid.WorldToCell(transform.position);
            Instantiate(plant, new Vector3(cellPosition.x + 0.5f + movement.direction.x, cellPosition.y - 0.5f + movement.direction.y, cellPosition.z), Quaternion.identity);

        }*/

        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        // call use here for the tool

        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        if(tillableTile == null)
        {
            Debug.Log("null tile");
        }

        if(tillableTile != null && (tillableTile as TillableTile).plant == null){
            Debug.Log("attempting to plant at location " + cellPosition);
            var newPlant = Instantiate(plant, new Vector3(cellPosition.x + 0.5f, cellPosition.y - 0.5f, 0), Quaternion.identity);
            //newPlant.transform.position = new Vector3(cellPosition.x + 0.5f, cellPosition.y - 0.5f, 0);
            (tillableTile as TillableTile).plant = newPlant.GetComponent<Plant>();
        }
        
        //currentEquipped.GetComponent<Item>().Use(fixedCellLocation);
    }

    

    void EquipItem(GameObject item)
    {
        if (item == null)
        {
            currentEquipped = null;
            itemSelectedText.GetComponent<TextMeshPro>().text = "Item: None";
            Debug.Log("No item selected");
            return;
        }

        currentEquipped = item;
        print(item.GetComponent<Item>().itemName + " : " + item.name);
        itemSelectedText.GetComponent<TextMeshPro>().text = "Item: " + item.GetComponent<Item>().itemName;
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

        if(collison.CompareTag("House"))
        {
            canSleep = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collison) 
    {
        if(collison.CompareTag("Water"))
        {
            nearWater = false;
        }

        if(collison.CompareTag("House"))
        {
            canSleep = false;
        }
    }



}
