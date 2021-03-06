﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Items/Seed")]
public class Seed : Item
{

    // Which type of plant it will grow
    public GameObject plant;

    //[HideInInspector]
    public int seedCount = 1;


    public override int Use(CustomTile selectedTile, Vector3 plantLocation)
    {

        if (selectedTile != null && (selectedTile as TillableTile).plant == null)
        {
            if((selectedTile as TillableTile).beenHoed){
                var newPlant = Instantiate(plant, plantLocation, Quaternion.identity);
                (selectedTile as TillableTile).plant = newPlant.GetComponent<Plant>();
                (selectedTile as TillableTile).plant.myPrefab = plant;
                (selectedTile as TillableTile).UnHoe(); // remove the hoe state

                seedCount -= 1;
                if (seedCount <= 0)
                {
                    Inventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

                    playerInventory.RemoveItemFromInventory(this);
                }
                return energyCost;
            }
            else
            {
                UIManager.Instance.SendNotification("Till Soil Before Planting!");
                //UIManager.Instance.ActionStatus.text = "Soil needs to be tilled to plant!";
                //UIManager.Instance.displayX();
            }
        }
        return 0;
    }
}
