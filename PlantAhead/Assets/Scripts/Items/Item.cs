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
    public int cost = 5;

    public Sprite icon;


    // this should return energy cost
    public virtual int Use(CustomTile selectedTile, Vector3 plantLocation)
    {
        Debug.Log("Using item");
        return energyCost;
    }
}
