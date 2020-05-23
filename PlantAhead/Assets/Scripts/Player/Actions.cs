using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class Actions : MonoBehaviour
{
    public TMP_Text itemSelectedText;
    public Item firstSelectedItem;
    public GameObject gridObject;
    public GameObject selectionSprite;

    public Grid grid;
    public Tilemap tillableTiles;
    private Item currentEquipped;
    public GameObject curSelectionSprite = null;
    [SerializeField]
    private Vector3Int curCellPosition;

    private Movement movement;
    public InventoryDisplay inventoryDisplay;
    public StoreDisplay storeDisplay;
    private GameManager gameManager;

    //public Image waterBar; in watercan.cs

    [SerializeField]
    private bool nearWater = false; // watercan.cs make it's own check; this may not be needed here

    [SerializeField]
    private bool canSleep = false;
    
    [SerializeField] public GameObject plant;

    [SerializeField]
    public ScriptableObject waterCan;
    // Start is called before the first frame update
    void Start(){
        Debug.Log("Actions Start");
        var temp = GameObject.FindGameObjectWithTag("GameManager");
        if (temp != null)
        {
            gameManager = temp.GetComponent<GameManager>();
        }

        // EquipItem(firstSelectedItem);
        //grid = gridObject.GetComponent<Grid>();

        //playerInventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();
        curSelectionSprite = GameObject.Instantiate(selectionSprite) as GameObject;
        EquipItem(firstSelectedItem);
    }

    // Update is called once per frame
    void Update(){
        selectCell();
        // Used to interact with a tile
        if (Input.GetKeyDown(KeyCode.Space)){
            DoAction();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Toggling Inventory");
            inventoryDisplay.toggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Toggling Store");
            storeDisplay.toggleDisplay();
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Attempting to harvest");
            Harvest();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Attempting to water or get water");
            Water();
        }

        // Player sleeps
        // checks if you're in the house
        if (Input.GetKeyDown(KeyCode.Q) && canSleep){
            gameManager.EndDay();
        }
    }

    void DoAction()
    {
        if (currentEquipped == null)
        {
            Debug.Log("No item currently equipped -- Cannot Preform Action");
            return;
        }
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);

        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        currentEquipped.Use(tillableTile, new Vector3(cellPosition.x + 0.5f, cellPosition.y - 0.5f, 0));
    }

    void Harvest()
    {
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        if ((tillableTile as TillableTile).plant != null){
            (tillableTile as TillableTile).plant.harvest();
            
        }
    }

    void Water()
    {
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        (waterCan as Watercan).Use((tillableTile as TillableTile));

    }
    

    public void EquipItem(Item itemToEquip)
    {
        if (itemToEquip == null)
        {
            currentEquipped = null;
            itemSelectedText.text = "Item: None";
            Debug.Log("No item selected");
            return;
        }

        currentEquipped = itemToEquip;
        print(itemToEquip.itemName + " : " + itemToEquip.itemName);
        itemSelectedText.text = "Item: " + itemToEquip.itemName;
        Debug.Log("Equipped " + itemToEquip.itemName);
    }

    void selectCell()
    {
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        Vector3 offset = new Vector3(cellPosition.x + 0.5f + movement.direction.x, cellPosition.y - 0.5f + movement.direction.y, cellPosition.z);

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
