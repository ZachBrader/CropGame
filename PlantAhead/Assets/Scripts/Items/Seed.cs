using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Items/Seed")]
public class Seed : Item
{

    // Which type of plant it will grow
    public GameObject plant;

    [HideInInspector]
    public int seedCount = 1;


    public override int Use(TileBase selectedTile)
    {

        // maybe make sure the tile is a tillable tile??? idk????
        // check if tile has been hoed
        Debug.Log("Planted Seed at " + selectedTile);
        if (plant.tag.Equals("Plant"))
        {
            Instantiate(plant, (selectedTile as Tile).gameObject.transform.position, Quaternion.identity);

        }
        return energyCost;
    }
}
