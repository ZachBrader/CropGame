using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Item : ScriptableObject
{
    public string itemName = "none";
    public string triggerName = "none";
    public Tilemap tillableTiles;
    public int energyCost = 5;


    // this should return energy cost
    public virtual int Use(TileBase selectedTile)
    {
        Debug.Log("Using item");
        return energyCost;
    }
}
