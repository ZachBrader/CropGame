using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class Actions : MonoBehaviour
{

    private GameManager gameManager;

    private TMP_Text itemSelectedText;
    private Image equippedItemImage;
    private TMP_Text equippedItemCount;
    private GameObject guideParent;
    private TMP_Text guideText;

    private Sprite RedX;

    private Movement movement;
    private Inventory playerInventory;

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
    private bool canSleep = false;
    private bool actionLock = false;
    private bool isPassedOut = false;
    
    //[SerializeField] public GameObject plant;

    [SerializeField]
    public ScriptableObject waterCan;

    [SerializeField]
    public ScriptableObject hoe;
    
    [SerializeField] public AudioClip waterSound;
    [SerializeField] public AudioClip harvestSound;
    [SerializeField] public AudioClip sellSound;
    [SerializeField] public AudioClip hoSound;
    
    private AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        playerInventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();

        curSelectionSprite = GameObject.Instantiate(selectionSprite) as GameObject;

        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        waterCan = Instantiate(waterCan);
        (waterCan as Watercan).updateWaterBar();

        #region Set UI Elements
        GameObject UI = gameManager.UI;
        itemSelectedText = UI.transform.Find("PlayerUI/ItemSelectedBackground/SelectedItemText").GetComponent<TMP_Text>();
        equippedItemImage = UI.transform.Find("PlayerUI/ItemSelectedBackground/ItemImage").GetComponent<Image>();
        equippedItemCount = UI.transform.Find("PlayerUI/ItemSelectedBackground/ItemImage/ItemsInInventoryText").GetComponent<TMP_Text>();
        guideParent = UI.transform.Find("PlayerUI/GameHintBackground").gameObject;
        guideText = guideParent.transform.Find("GameHintText").GetComponent<TMP_Text>();
        RedX = transform.Find("RedX").gameObject.GetComponent<SpriteRenderer>().sprite;
        #endregion
    }

    // Update is called once per frame
    void Update(){

        selectCell();

        #region User Actions
        if (!actionLock && !isPassedOut)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DoAction();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Harvest();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Water();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Hoe();
            }

            // Player sleeps
            // checks if you're in the house
            if (Input.GetKeyDown(KeyCode.Q) && canSleep)
            {
                gameManager.EndDay();
            }
        }
        #endregion

        #region Inventory Selection
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.OpenMenu("Day");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameManager.OpenMenu("InGameMenu");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gameManager.OpenMenu("Inventory");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gameManager.OpenMenu("Store");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gameManager.OpenMenu("TasksBoard");
        }
        #endregion

        if (currentEnergy < 1 && !isPassedOut) {
            isPassedOut = true;
            UIManager.Instance.SendNotification("You Passed Out!");
            gameManager.EndDay((int)(Math.Round(maxEnergy * passOutPenalty)));
        }

        UpdateEquippedItemCount();
    }

    void DoAction()
    {
        if (currentEquipped == null)
        {
            UIManager.Instance.SendNotification("No Seed Equipped!");
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

            (tillableTile as TillableTile).UnHoe();
            if(energyCost != 0)
            {
                UIManager.Instance.SendStatusUpdate("Successfully Harvested!");
                animator.SetTrigger("Sickle");
                StartCoroutine(StopPlayerMovement(0.3333f));
                currentEnergy -= energyCost;
                setEnergyBar();
            }
            source.PlayOneShot(harvestSound);

        }

    }

    void Water()
    {
        //source.PlayOneShot(waterSound, .5f);
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
        (waterCan as Watercan).updateWaterBar();
        source.PlayOneShot(waterSound);
    }

    void Hoe()
    {

        Vector3 selectorPos = new Vector3(transform.position.x + movement.direction.x, transform.position.y + movement.direction.y, 0);
        Vector3Int cellPosition = grid.WorldToCell(selectorPos);
        
        var tillableTile = gameManager.GetTile(new Vector2Int(cellPosition.x, cellPosition.y));
        if (tillableTile == null) { return; }
        if (tillableTile.GetType() != typeof(TillableTile)) { return; }

        int energyCost = (hoe as Hoe).Use(tillableTile);
        
        if(energyCost != 0){
                animator.SetTrigger("Hoe");
                StartCoroutine(StopPlayerMovement(0.3333f));
                currentEnergy -= energyCost;
                setEnergyBar();
        }
        source.PlayOneShot(hoSound);
    }

    #region Equip Item Functions
    public Item GetEquippedItem()
    {
        return currentEquipped;
    }

    public void UpdateEquippedItemCount()
    {
        if (currentEquipped != null)
        {
            equippedItemCount.text = (currentEquipped as Seed).seedCount.ToString();
        }
    }

    public void EquipItem(Item itemToEquip)
    {
        if (itemToEquip == null)
        {
            currentEquipped = null;
            itemSelectedText.text = "---";
            equippedItemCount.text = "";
            equippedItemImage.sprite = RedX;
            return;
        }

        currentEquipped = itemToEquip;
        itemSelectedText.text = itemToEquip.itemName;
        equippedItemImage.sprite = itemToEquip.icon;
        equippedItemCount.text = (currentEquipped as Seed).seedCount.ToString();
    }
    #endregion

    void selectCell()
    {
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        Vector3 offset = new Vector3(cellPosition.x + 0.5f + movement.direction.x, cellPosition.y - 0.5f + movement.direction.y, cellPosition.z);
        curSelectionSprite.transform.position = offset;

        #region Guide Text Logic
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
                else
                {
                    guideParent.SetActive(false);
                }
            }
            else
            {
                if ((tillableTile as TillableTile).beenHoed && currentEquipped != null)
                {
                    guideText.text = "Press SPACE BAR to plant an equipped seed here!";
                }
                else if ((tillableTile as TillableTile).beenHoed && currentEquipped == null)
                {
                    guideText.text = "Purchase more seeds or equip one from inventory to plant here!";
                }
                else if ((tillableTile as TillableTile).plant == null)
                {
                    guideText.text = "Press R to hoe land!";
                }
                else
                {
                    guideParent.SetActive(false);
                }
            }
        }
        #endregion
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
        else if(collison.CompareTag("Animal"))
        {
            UIManager.Instance.DisplayHearts();
        }
    }

    public void OnTriggerExit2D(Collider2D collison) 
    {
        if(collison.CompareTag("House"))
        {
            canSleep = false;
        }
    }

    public void WakeUp()
    {
        StartCoroutine(WakeUpLogic());
    }

    IEnumerator WakeUpLogic()
    {
        yield return new WaitForSeconds(.5f);
        gameManager.OpenMenu("Day");
        movement.canMove = true;
        SetActionLock(false);
        SetPassedOut(false);
        yield return null;
    }

    public void SetCanSleep(bool newSleep)
    {
        canSleep = newSleep;
    }

    public void SetActionLock(bool actionStatus)
    {
        actionLock = actionStatus;
    }

    public void SetPassedOut(bool passedOutStatus)
    {
        isPassedOut = passedOutStatus;
    }

    private IEnumerator StopPlayerMovement(float time) 
    {
        movement.canMove = false;
        yield return new WaitForSeconds(time);
        if (!isPassedOut)
        {
            movement.canMove = true;
        }
    }   

}
