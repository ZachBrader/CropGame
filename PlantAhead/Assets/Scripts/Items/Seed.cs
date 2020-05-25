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
            if((selectedTile as TillableTile).beenHoed){
                Debug.Log("Planting at location " + plantLocation);

                var newPlant = Instantiate(plant, plantLocation, Quaternion.identity);
                (selectedTile as TillableTile).plant = newPlant.GetComponent<Plant>();
                (selectedTile as TillableTile).UnHoe(); // remove the hoe state
                return energyCost;
            }
            else
            {
                UIManager.Instance.ActionStatus.text = "Soil needs to be tilled to plant!";
                UIManager.Instance.displayX();
            }
        }
        Debug.Log("Failed to plant");
        return 0;
    }
}
