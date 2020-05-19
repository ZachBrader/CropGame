using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Item : MonoBehaviour
{
    public string itemName;
    public string triggerName;
    public Tilemap tillableTiles;
    public int energyCost = 5;

    // Start is called before the first frame update
    void Start()
    {
        itemName = "None";
        triggerName = "None";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // this should return energy cost
    public virtual int Use(Vector3Int selectedTile)
    {
        Debug.Log("Using item");
        return energyCost;
    }
}
