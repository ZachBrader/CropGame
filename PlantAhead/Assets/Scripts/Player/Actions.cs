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
    private Inventory playerInventory;
    public InventoryDisplay inventoryDisplay;
    public StoreDisplay storeDisplay;
    public InGameMenu inGameMenu;
    public GameObject playerUI;

    public GameObject guideParent;
    public TMP_Text guideText;
 
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
        playerInventory = GetComponent<Inventory>();
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
            gameManager.OpenMenu("InGameMenu");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameManager.OpenMenu("Inventory");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameManager.OpenMenu("Store");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Harvest();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Water();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
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
            return;
        }
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);

        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));
        if (tillableTile.GetType() != typeof(TillableTile)) { return; }

        int energyCost = currentEquipped.Use(tillableTile, new Vector3(cellPosition.x + 0.5f, cellPosition.y - 0.5f, 0));
        setEnergyBar();
        
    }

    void Harvest()
    {
        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));

        if (tillableTile == null) { return; }
        if (tillableTile.GetType() != typeof(TillableTile)) { return; }
        Plant plantToHarvest = (tillableTile as TillableTile).plant;
        if (plantToHarvest != null){
            playerInventory.ReceiveGold(plantToHarvest.ValuePlant());
            int energyCost = plantToHarvest.harvest();
            plantToHarvest = null;

            (tillableTile as TillableTile).beenHoed = false;
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

        if (tillableTile == null) { return; }

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
        if (tillableTile == null) { return; }
        if (tillableTile.GetType() != typeof(TillableTile)) { return; }

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

        if (canSleep)
        {
            guideParent.SetActive(true);
            guideText.text = "Press Q to sleep";
            return;
        }
        var tillableTile = gameManager.GetTile(new Vector2Int((int)offset.x, (int)offset.y));

        if (tillableTile == null)
        {
            guideParent.SetActive(false);
            return;
        }
        else
        {
            guideParent.SetActive(true);
        }

        if (tillableTile.GetType() == typeof(WaterTile))
        {
            guideText.text = "Press F to refill water!";
        }
        else if (tillableTile.GetType() == typeof(TillableTile))
        {
            if ((tillableTile as TillableTile).plant != null)
            {
                if ((tillableTile as TillableTile).plant.waterLevel < 1)
                {
                    guideText.text = "Press F to water plant";
                }
                else if ((tillableTile as TillableTile).plant.GetPlantStage() > 3)
                {
                    guideText.text = "Press G to harvest plant";
                }
            }
            else
            {
                if ((tillableTile as TillableTile).beenHoed)
                {
                    guideText.text = "Press SPACE BAR to plant an equipped seed here!";
                }
                else
                {
                    guideText.text = "Press R to hoe land!";
                }
            }
        }
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

    public Item GetEquippedItem()
    {
        return currentEquipped;
    }
    

}
