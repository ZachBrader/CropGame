﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    #region Player Reference Variables
    private GameObject player;
    private Inventory playerInventory;
    private Movement playerMovement;
    private Actions playerActions;
    #endregion

    #region Player UI Variables
    public GameObject UI;

    private EndGameMenu endGameMenu;
    private InventoryDisplay inventoryDisplay;
    private GameObject playerUI;
    private StoreDisplay storeDisplay;
    private InGameMenu inGameMenu;
    private InGameMenu TasksBoard;


    private TMP_Text playerGoldTrackerText;
    private Image panel;
    private TMP_Text dayTrackerText;

    private bool menuLocked = false;
    private string curMenu = "Day";
    #endregion

    [HideInInspector]
    public int curDay;
    public int finalDate;
    public int playerGoldGoal;

    private bool isReady = false;


    public bool mushroomLevel;
    public GameObject mushroom;
    public int startingMushrooms;

    public SuperMap myMap;
    public Tilemap dirtTiles;

    public List<Tilemap> waterTiles;

    public List<Water> waterSources;
    public CustomTile[,] tileGrid;

    public TileSpriteManager tileManager;
    public List<TillableTile> DirtTileList = new List<TillableTile>();

    public AudioClip gameWonSound;
    public AudioClip gameLostSound;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start(){

        audioSource = GetComponent<AudioSource>();
        curDay = 1;
        TileSpriteManager.Instance = tileManager;

        #region Initialize Player Reference Variables
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<Inventory>();
        playerMovement = player.GetComponent<Movement>();
        playerActions = player.GetComponent<Actions>();
        #endregion

        #region Initialize UI Reference Variables
        endGameMenu = UI.transform.Find("EndGameMenu").GetComponent<EndGameMenu>();
        inventoryDisplay = UI.transform.Find("InventoryDisplay").GetComponent<InventoryDisplay>();
        inGameMenu = UI.transform.Find("InGameMenu").GetComponent<InGameMenu>();
        TasksBoard = UI.transform.Find("TasksBoard").GetComponent<TasksBoard>();
        storeDisplay = UI.transform.Find("StoreDisplay").GetComponent<StoreDisplay>();
        playerUI = UI.transform.Find("PlayerUI").gameObject;

        playerGoldTrackerText = UI.transform.Find("PlayerUI/GoldIcon/PlayerGoldTrackerText").GetComponent<TMP_Text>();
        panel = UI.transform.Find("Panel").GetComponent<Image>();
        panel.color = new Color(0, 0, 0, 0);
        panel.gameObject.SetActive(false);

        dayTrackerText = UI.transform.Find("PlayerUI/DayTrackerBackground/DayTrackerText").GetComponent<TMP_Text>();
        #endregion

        UpdateDateInformation();

        #region Initialize Grid
        // init the grid for interactions
        tileGrid = new CustomTile[myMap.m_Width, myMap.m_Height];
        for(int x = 0; x < myMap.m_Width; x ++)
        {
            for(int y = 0; y < myMap.m_Height; y++)
            {
                var thisTile = dirtTiles.GetTile(new Vector3Int(x, -y, 0));
                if( thisTile != null)
                {
                    // instantiate where there are dirtTiles
                    // note: dirt tiles do not contain the border tiles that touch grass
                    // this is important!
                    tileGrid[x, y] = new TillableTile()
                    {
                        tilemap = dirtTiles,
                        tilePosition = new Vector3Int(x, -y, 0)
                    };
                    DirtTileList.Add(tileGrid[x, y] as TillableTile);
                }

                for(int i = 0; i < waterTiles.Count; i++)
                {
                    thisTile = waterTiles[i].GetTile(new Vector3Int(x, -y, 0));
                    if( thisTile != null)
                    {
                        // instantiate where there are water tiles, with each layer hold a different water source
                        tileGrid[x, y] = new WaterTile(waterSources[i]);
                    }
                }
            }
        }
        #endregion

        if (mushroomLevel)
        {
            MushroomStart();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerGoldTrackerText != null)
        {
            playerGoldTrackerText.text = playerInventory.GetGold().ToString();
        }
    }

    public void OpenMenu(string menuName)
    {
        if (menuLocked == true)
        {
            return;
        }
        curMenu = menuName;

        #region Menu Logic
        if (menuName != "Store")
        {
            storeDisplay.closeDisplay();
        }
        else
        {
            storeDisplay.toggleDisplay();
            playerUI.SetActive(!storeDisplay.checkOpen());
        }

        if (menuName != "Inventory")
        {
            inventoryDisplay.closeDisplay();
        }
        else
        {
            inventoryDisplay.toggleDisplay();
            playerUI.SetActive(!inventoryDisplay.checkOpen());
        }

        if (menuName != "InGameMenu")
        {
            inGameMenu.closeDisplay();
        }
        else
        {
            inGameMenu.toggleDisplay();
            playerUI.SetActive(!inGameMenu.checkOpen());
        }

        if (menuName != "EndGameMenu")
        {
            endGameMenu.closeDisplay();
        }
        else
        {
            endGameMenu.toggleDisplay();
            playerUI.SetActive(!endGameMenu.checkOpen());
        }

        if (menuName != "TasksBoard")
        { 
            TasksBoard.closeDisplay();
        }
        else
        {
            TasksBoard.toggleDisplay();
            playerUI.SetActive(!TasksBoard.checkOpen());
        }

        if (menuName == "Night")
        {
            playerUI.SetActive(false);
        }

        if (menuName == "Day")
        {
            playerUI.SetActive(true);
        }
        #endregion

        if (playerUI.activeSelf == true)
        {
            curMenu = "Day";
        }
        playerMovement.canMove = playerUI.activeSelf;
    }

    public string GetCurrentMenu()
    {
        return curMenu;
    }

    public void EndDay(int penalty = 0)
    {
        StartCoroutine(SimulateNight(penalty));
    }

    void NightLogic(int penalty = 0)
    {
        curDay++;

        #region Update Map
        //updates each plants stage, will add checks later to only have this done in specific 
        //conditions, this will also be where growth happens
        for (int x = 0; x < myMap.m_Width; x++)
        {
            for (int y = 0; y < myMap.m_Height; y++)
            {
                var thisTile = tileGrid[x, y];
                if ((thisTile as TillableTile) != null)
                {
                    if ((thisTile as TillableTile).plant != null)
                    {
                        #region Mushroom Expansion
                        // mushrooms:
                        // expand no matter their water stage
                        // grow no matter what
                        if ((thisTile as TillableTile).plant.isMushroom)
                        {
                            int spread = (thisTile as TillableTile).plant.SpreadZone;
                            List<TillableTile> neighbors = checkNeighborhood(x, y, spread);
                            var weightedCurve = Random.Range(0, 99);
                            int numSpread;

                            if (weightedCurve < 40)
                            {
                                numSpread = 0;// no spread go home
                            }
                            else if (weightedCurve < 60)
                            {
                                // spread by 10% ROUNDED UP
                                numSpread = Mathf.CeilToInt(neighbors.Count * 0.1f);
                            }
                            else if (weightedCurve < 70)
                            {
                                // spread by 20% ROUNDED UP
                                numSpread = Mathf.CeilToInt(neighbors.Count * 0.2f);
                            }
                            else if (weightedCurve < 81)
                            {
                                // spread by 30% ROUNDED UP
                                numSpread = Mathf.CeilToInt(neighbors.Count * 0.3f);
                            }
                            else if (weightedCurve < 91)
                            {
                                // spread by 40% ROUNDED UP
                                numSpread = Mathf.CeilToInt(neighbors.Count * 0.4f);
                            }
                            else if (weightedCurve < 96)
                            {
                                // spread by 50% ROUNDED UP
                                numSpread = Mathf.CeilToInt(neighbors.Count * 0.5f);
                            }
                            else if (weightedCurve < 99)
                            {
                                // spread by 75% ROUNDED UP
                                numSpread = Mathf.CeilToInt(neighbors.Count * 0.75f);
                            }
                            else
                            {
                                //100% spread;
                                numSpread = neighbors.Count;
                            }

                            // loop until the list is empty or all mushrooms have been spread
                            while (neighbors.Count > 0 && numSpread > 0)
                            {
                                int hit = Random.Range(0, neighbors.Count);
                                if (neighbors[hit].plant == null)
                                {
                                    SpreadPlants(mushroom, neighbors[hit]);
                                    numSpread--;
                                }
                                // remove the index because it's not valid to use again
                                neighbors.RemoveAt(hit);
                            }

                            // update the plant stage if possible
                            (thisTile as TillableTile).plant.plantStageUpdate();
                        }
                        #endregion
                        else // not a mushroom
                        {
                            //grow current plant and if it has too much water expand it
                            if ((thisTile as TillableTile).plant.GetDayCreated() < curDay)
                            {
                                if ((thisTile as TillableTile).plant.plantStageUpdate())
                                {

                                    //get all neighbors of current plant
                                    List<TillableTile> neighbors = checkNeighborhood(x, y);

                                    //generate a random number that will be the one that is expanded to
                                    GameObject currentplant = (thisTile as TillableTile).plant.myPrefab;
                                    Plant plantRef = currentplant.GetComponent<Plant>();
                                    for (int i = 0; i < neighbors.Count; i++)
                                    {
                                        //expand and then update current plant
                                        if (Random.Range(0, 10) < plantRef.spreadRate)
                                        {
                                            SpreadPlants(currentplant, neighbors[i]);
                                        }
                                    }

                                }
                            }

                        }

                        // spawn mushrooms if this is a mushroom level... sometimes
                        if (mushroomLevel)
                        {
                            var roll = Random.Range(0, 100);
                            if (roll > 80)
                            {
                                MushroomSpawn(startingMushrooms);
                            }
                        }
                    }
                    else if ((thisTile as TillableTile).beenHoed)
                    {
                        Debug.Log("Unhoeing land");
                        (thisTile as TillableTile).UnHoe();
                    }
                }
            }
        }
        #endregion

        UpdateDateInformation();

        // reset the player health
        player.GetComponent<Actions>().refreshPlayer(penalty);

        // refil the water sources
        foreach (Water source in waterSources)
        {
            source.refillWaterSource();
        }

    }

    IEnumerator SimulateNight(int penalty = 0)
    {
        playerActions.SetActionLock(false);
        playerActions.SetCanSleep(false);
        panel.gameObject.SetActive(true);

        OpenMenu("Night");
        menuLocked = true;
        playerMovement.canMove = false;

        while (panel.color.a < 1)
        {
            panel.color = new Color(0, 0, 0, panel.color.a + (float)(Time.deltaTime / .5));
            yield return null;
        }

        NightLogic(penalty);

        while (panel.color.a > 0)
        {
            panel.color = new Color(0, 0, 0, panel.color.a - (float)(Time.deltaTime / .5));
            yield return null;
        }

        Debug.Log("Players should start moving");
        panel.gameObject.SetActive(false);
        menuLocked = false;
        playerActions.WakeUp();

        if (curDay == finalDate || HasPlayerWon())
        {
            EndGame();
            Debug.Log("Level Completed");
        }
        yield return null;
    }

    public CustomTile GetTile(Vector2Int location)
    {
        if(location.x < 0 || location.x > myMap.m_Width || -location.y < 0 || -location.y > myMap.m_Height) 
        {
            Debug.Log("Location: " + location + " is out of range");
            return null;
        }
        
        // return the tile
        // tile could be NULL YOU MUST CHECK on your own!!!
        return tileGrid[location.x, -location.y];
    }
    
    private bool HasPlayerWon()
    {
        // If player has made enough gold
        if (playerInventory.GetGold() >= playerGoldGoal)
        {
            return true;
        }
        return false;
    }

    void MushroomStart()
    {
        #region Mushroom Initialization
        for (int i = 0; i < startingMushrooms;)
        {
            int hit = Random.Range(0, DirtTileList.Count);
            if(DirtTileList[hit].plant == null)
            {
                //plant mush
                SpreadPlants(mushroom, DirtTileList[hit]);
                i++;
            }
            else
            {
                bool noMush = true;
                int temp = hit + 1;
                while(noMush && temp != hit && temp < DirtTileList.Count)
                {
                    if(DirtTileList[temp].plant == null)
                    {
                        noMush = false;
                        SpreadPlants(mushroom, DirtTileList[hit]);
                        i++;
                    }
                    
                    if(temp + 1 > DirtTileList.Count){
                        temp = -1;
                    }

                    temp++;
                }
            }
        }
        #endregion
    }

    void SpreadPlants(GameObject plant, TillableTile tile)
    {
        if(tile.plant == null){
            Vector3 offset = new Vector3(tile.tilePosition.x + 0.5f, tile.tilePosition.y - 0.5f, 0);
            var newPlant = Instantiate(plant, offset, Quaternion.identity);
            tile.plant = newPlant.GetComponent<Plant>();
            tile.plant.myPrefab = plant;
            tile.plant.resetPlant();
            tile.plant.SetDayCreated(curDay);
            (tile as TillableTile).UnHoe();
        }
    }

    void MushroomSpawn(int maxMush = 5)
    {
        #region Mushroom Logic
        // determine how many to try and plant
        maxMush = Random.Range(0, 6);

        for(int i = 0; i < maxMush;)
        {
            int hit = Random.Range(0, DirtTileList.Count);
            // plant the mush if there's no plant on that tile
            if(DirtTileList[hit].plant != null)
            {
                SpreadPlants(mushroom, DirtTileList[hit]);
                i++;
            }

            // something else is already there, go into retry logic
            else
            {
                bool noMush = true;
                int temp = hit;
                
                // don't run forever if there are no empty spots
                while(noMush)
                {
                    if(temp + 1 >= DirtTileList.Count){
                        temp = -1;
                    }

                    temp++;

                    if(temp == hit)
                    {
                        // all spots are full, don't bother trying anymore
                        return;
                    }

                    if(DirtTileList[temp].plant == null)
                    {
                        noMush = false;
                        SpreadPlants(mushroom, DirtTileList[hit]);
                        i++;
                    }
                    
                }
                

            }
        }
        #endregion
    }

    // returns all tillable tiles in the neighborhood
    // then for spawning new plants you can just do a random number index and spawn it if that place has no plant
    private List<TillableTile> checkNeighborhood(int my_x, int my_y, int squaresAway = 1)
    {
        List <TillableTile> tilesInNeighborhood = new List<TillableTile>();
        // get the bounds
        int leftbound = (my_x - squaresAway >= 0 ? my_x - squaresAway : 0);
        int rightbound = (my_x + squaresAway < myMap.m_Width ? my_x + squaresAway : myMap.m_Width);
        int upperbound = (my_y + squaresAway < myMap.m_Height ? my_y + squaresAway : myMap.m_Height);
        int lowerbound = (my_y - squaresAway >= 0 ? my_y - squaresAway : 0);

        for (int x = leftbound; x <= rightbound; x++)
        {
            for(int y = lowerbound; y <= upperbound; y++)
            {
                if(tileGrid[x, y] is TillableTile && tileGrid[x,y] != null)
                {
                    tilesInNeighborhood.Add(tileGrid[x, y] as TillableTile);
                }
            }
        }
        return tilesInNeighborhood;
    }

    private void UpdateDateInformation()
    {
        dayTrackerText.text = "Current Day: " + curDay + "  -  Days Left: " + (finalDate - curDay);
    }


    private void EndGame()
    {
        playerActions.SetCanSleep(false);
        menuLocked = false;
        OpenMenu("EndGameMenu");
        menuLocked = true;
        endGameMenu.setDateText(curDay + " / " + finalDate);
        endGameMenu.setGoldText(playerInventory.GetGold() + " / " + playerGoldGoal);
        if (HasPlayerWon())
        {

            audioSource.PlayOneShot(gameWonSound);
            endGameMenu.setVictoryText("Victory!");
            endGameMenu.ActivateObject(endGameMenu.nextLevelButton);
            Debug.Log("Player has won!");
        }
        else
        {
            audioSource.PlayOneShot(gameLostSound);
            endGameMenu.setVictoryText("Level Failed!");
            endGameMenu.DeactivateObject(endGameMenu.nextLevelButton);
            Debug.Log("Player has lost");
        }
    }
}
