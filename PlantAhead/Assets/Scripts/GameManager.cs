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

    // Keeps track of the day the player is currently on
    private int curDay;

    // When the player needs to meet their goal
    public int finalDate;

    public SuperMap myMap;
    public Tilemap dirtTiles;

    public List<Tilemap> waterTiles;

    public List<Water> waterSources;
    public CustomTile[,] tileGrid;

    // Start is called before the first frame update
    void Start()
    {
        
        GameObject.Find("UIOverlay/Panel").GetComponent<Image>().color = new Color(0,0,0,0);

        dayTrackerText.GetComponent<TMP_Text>().text = "Date: " + curDay + " / " + finalDate;

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
                    tileGrid[x, y] = new TillableTile();
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

    public void EndDay()
    {
        GameObject.Find("UIOverlay/Panel").GetComponent<Image>().color = new Color(0,0,0,255);
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

        dayTrackerText.GetComponent<TMP_Text>().text = "Date: " + curDay + " / " + finalDate;

        if (curDay == finalDate)
        {
            Debug.Log("Level Completed");
        }

        Invoke("clearPanel",1);
        
    }

    void clearPanel(){
        GameObject.Find("UIOverlay/Panel").GetComponent<Image>().color = new Color(0,0,0,0);
        
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
    

}
