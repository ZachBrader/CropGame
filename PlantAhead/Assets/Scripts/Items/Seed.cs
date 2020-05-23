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


    public override int Use(CustomTile selectedTile, Vector3 plantLocation)
    {

        if (selectedTile != null && (selectedTile as TillableTile).plant == null)
        {
            Debug.Log("Planting at location " + plantLocation);

            var newPlant = Instantiate(plant, plantLocation, Quaternion.identity);
            (selectedTile as TillableTile).plant = newPlant.GetComponent<Plant>();
            return energyCost;
        }
        Debug.Log("Failed to plant");
        return 0;
    }
}
