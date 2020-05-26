using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public GameObject dayTrackerText;

    // Victory Variables

    private int curDay;
    public int finalDate;
    public int playerGoldGoal;

    public EndGameMenu endGameMenu;
    private Inventory playerInventory;

    public InventoryDisplay inventoryDisplay;
    public GameObject playerUI;
    public StoreDisplay storeDisplay;
    public InGameMenu inGameMenu;
    public bool mushroomLevel;
    public GameObject mushroom;
    public int startingMushrooms;
    private Movement playerMovement;
    private bool menuLocked = false;
    private bool isGameOver = false;
    private Actions playerActions;

    // END VICTORY VARIABLES

    public SuperMap myMap;
    public Tilemap dirtTiles;

    public List<Tilemap> waterTiles;

    public List<Water> waterSources;
    public CustomTile[,] tileGrid;

    public GameObject player;

    public TileSpriteManager tileManager;
    public List<TillableTile> DirtTileList = new List<TillableTile>();

    // Start is called before the first frame update
    void Start()
    {
        TileSpriteManager.Instance = tileManager;
        GameObject.Find("UIOverlay/Panel").GetComponent<Image>().color = new Color(0,0,0,0);

        dayTrackerText.GetComponent<TMP_Text>().text = "Date: " + curDay + " / " + finalDate;

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerInventory = player.GetComponent<Inventory>();
        playerMovement = player.GetComponent<Movement>();
        playerActions = player.GetComponent<Actions>();

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
        
    }

    // Update is called once per frame
    void Update()
    {
        // Able to exit application by pressing escape button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OpenMenu(string menuName)
    {
        if (menuLocked == true)
        {
            return;
        }

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

        if (menuName == "Night")
        {
            playerUI.SetActive(false);
        }

        if (menuName == "Day")
        {
            playerUI.SetActive(true);
        }
        playerMovement.canMove = playerUI.activeSelf;
    }

    public void EndDay(int penalty = 0)
    {
        StartCoroutine(SimulateNight());
        curDay++;

        //updates each plants stage, will add checks later to only have this done in specific 
        //conditions, this will also be where growth happens
        for(int x = 0; x < myMap.m_Width; x ++)
        {
            for(int y = 0; y < myMap.m_Height; y++){
                var thisTile = tileGrid[x, y];
                if ((thisTile as TillableTile) != null && (thisTile as TillableTile).plant != null){
                    (thisTile as TillableTile).plant.plantStageUpdate();
                }

            }
        }

        // reset the player health
        player.GetComponent<Actions>().refreshPlayer(penalty);

        // refil the water sources
        foreach(Water source in waterSources) 
        {
            source.refillWaterSource();
        }

        dayTrackerText.GetComponent<TMP_Text>().text = "Date: " + curDay + " / " + finalDate; 
        
    }

    IEnumerator SimulateNight()
    {
        playerActions.SetCanSleep(false);
        Image screenOverlay = GameObject.Find("UIOverlay/Panel").GetComponent<Image>();
        OpenMenu("Night");
        menuLocked = true;
        playerMovement.canMove = false;

        while (screenOverlay.color.a < 1)
        {
            screenOverlay.color = new Color(0, 0, 0, screenOverlay.color.a + Time.deltaTime);
            yield return null;
        }

        while (screenOverlay.color.a > 0)
        {
            screenOverlay.color = new Color(0, 0, 0, screenOverlay.color.a - Time.deltaTime);
            yield return null;
        }
        menuLocked = false;
        OpenMenu("Day");
        playerMovement.canMove = true;
        playerActions.SetCanSleep(true);
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
        for(int i = 0; i < startingMushrooms;)
        {
            int hit = Random.Range(0, DirtTileList.Count);
            if(DirtTileList[hit].plant != null)
            {
                //plant mush
                SpreadPlants(mushroom, DirtTileList[hit]);
                i++;
            }
            else
            {
                bool noMush = true;
                int temp = hit + 1;
                while(noMush && temp != hit)
                {
                    if(DirtTileList[temp].plant != null)
                    {
                        noMush = false;
                        SpreadPlants(mushroom, DirtTileList[hit]);
                        i++;
                    }
                    else
                    {
                        if(hit + 1 > DirtTileList.Count){
                            temp = -1;
                        }
                    }
                    temp++;
                }
            }
        }
    }

    void SpreadPlants(GameObject plant, TillableTile tile)
    {
        if(tile.plant == null){
            Vector3 offset = new Vector3(tile.tilePosition.x + 0.5f, tile.tilePosition.y - 0.5f, 0);
            var newPlant = Instantiate(plant, offset, Quaternion.identity);
            tile.plant = newPlant.GetComponent<Plant>();
        }
    }

    void MushroomSpawn(int maxMush = 5)
    {
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
                int temp = hit + 1;
                // don't run forever if there are no empty spots
                while(noMush || temp == hit)
                {
                    if(DirtTileList[temp].plant != null)
                    {
                        noMush = false;
                        SpreadPlants(mushroom, DirtTileList[hit]);
                        i++;
                    }
                   else
                    {
                        if(hit + 1 > DirtTileList.Count){
                            temp = -1;
                        }
                    }
                    temp++;
                }
                // all spots are full, don't bother trying anymore
                if(temp == hit)
                {
                    return;
                }
            }
        }
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

        for (int x = leftbound; x < rightbound; x++)
        {
            for(int y = lowerbound; y < upperbound; y++)
            {
                if(tileGrid[x, y] is TillableTile && tileGrid[x,y] != null)
                {
                    tilesInNeighborhood.Add(tileGrid[x, y] as TillableTile);
                }
            }
        }
        return tilesInNeighborhood;
    }

    private void EndGame()
    {
        menuLocked = false;
        OpenMenu("EndGameMenu");
        menuLocked = true;
        endGameMenu.setDateText(curDay + " / " + finalDate);
        endGameMenu.setGoldText(playerInventory.GetGold() + " / " + playerGoldGoal);
        if (HasPlayerWon())
        {
            endGameMenu.setVictoryText("Victory!");
            endGameMenu.ActivateObject(endGameMenu.nextLevelButton);
            Debug.Log("Player has won!");
        }
        else
        {
            endGameMenu.setVictoryText("Level Failed!");
            endGameMenu.DeactivateObject(endGameMenu.nextLevelButton);
            Debug.Log("Player has lost");
        }
    }
}
