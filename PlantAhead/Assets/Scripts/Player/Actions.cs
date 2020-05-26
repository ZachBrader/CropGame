using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class Actions : MonoBehaviour
{
    public TMP_Text itemSelectedText;
    //public GameObject gridObject;
    public GameObject selectionSprite;
    public Animator animator;
    public Grid grid;
    public Tilemap tillableTiles;

    float passOutPenalty = 0.25f;
    public int currentEnergy = 100;
    public int maxEnergy = 100;
    private Item currentEquipped;
    public GameObject curSelectionSprite = null;
    [SerializeField]
    private Vector3Int curCellPosition;

    private Movement movement;
    public InventoryDisplay inventoryDisplay;
    public StoreDisplay storeDisplay;
    public InGameMenu inGameMenu;
    public GameObject playerUI;
 
    private GameManager gameManager;

    [SerializeField]
    private bool canSleep = false;
    
    //[SerializeField] public GameObject plant;

    [SerializeField]
    public ScriptableObject waterCan;

    [SerializeField]
    public ScriptableObject hoe;
    [SerializeField] public AudioClip waterSound;

    private AudioSource source;
    
    // Start is called before the first frame update
    void Start(){
        Debug.Log("Actions Start");
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //playerInventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();
        curSelectionSprite = GameObject.Instantiate(selectionSprite) as GameObject;
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        selectCell();
        // Used to interact with a tile
        if (Input.GetKeyDown(KeyCode.Space)){
            DoAction();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Toggling In Game Menu");
            gameManager.OpenMenu("InGameMenu");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Toggling Inventory");
            gameManager.OpenMenu("Inventory");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Toggling Store");
            gameManager.OpenMenu("Store");
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

        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Attempting to use the hoe");
            Hoe();
        }

        // Player sleeps
        // checks if you're in the house
        if (Input.GetKeyDown(KeyCode.Q) && canSleep){
            gameManager.EndDay();
        }

        if(currentEnergy < 1){
            UIManager.Instance.ActionStatus.text = " You Passed Out!";
            gameManager.EndDay((int)(Math.Round(maxEnergy * passOutPenalty)));
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

        int energyCost = currentEquipped.Use(tillableTile, new Vector3(cellPosition.x + 0.5f, cellPosition.y - 0.5f, 0));
        setEnergyBar();
        
    }

    void Harvest()
    {
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        if ((tillableTile as TillableTile).plant != null){
            int energyCost = (tillableTile as TillableTile).plant.harvest();
            (tillableTile as TillableTile).plant = null;
            if(energyCost != 0)
            {
                animator.SetTrigger("Sickle");
                StartCoroutine(StopPlayerMovement(0.3333f));
                currentEnergy -= energyCost;
                setEnergyBar();
            }

        }

    }

    void Water()
    {
        source.PlayOneShot(waterSound, .5f);
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        int energyCost = (waterCan as Watercan).Use(tillableTile);
        if(energyCost != 0)
            {
                animator.SetTrigger("Water");
                StartCoroutine(StopPlayerMovement(0.3333f));
                currentEnergy -= energyCost;
                setEnergyBar();
            }
        
        
        
    }

    void Hoe()
    {
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        int energyCost = (hoe as Hoe).Use(tillableTile);
        if(energyCost != 0)
            {
                animator.SetTrigger("Hoe");
                StartCoroutine(StopPlayerMovement(0.3333f));
                currentEnergy -= energyCost;
                setEnergyBar();
            }
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

    }

    public void refreshPlayer(int penalty = 0)
    {
        currentEnergy = maxEnergy - penalty;
        setEnergyBar();
        
    }

    public void setEnergyBar()
    {
        // no div 0!!!
        if(maxEnergy < 1) {
            maxEnergy = 1;
        }

        UIManager.Instance.EnergyBar.fillAmount = (float) currentEnergy / (float) maxEnergy;
    }

    public void OnTriggerEnter2D(Collider2D collison) 
    {
        if(collison.CompareTag("House"))
        {
            canSleep = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collison) 
    {
        if(collison.CompareTag("House"))
        {
            canSleep = false;
        }
    }

    public void SetCanSleep(bool newSleep)
    {
        canSleep = newSleep;
    }

    private IEnumerator StopPlayerMovement(float time) 
    {
        movement.canMove = false;
        yield return new WaitForSeconds(time);
        movement.canMove = true;
    }

    

}
