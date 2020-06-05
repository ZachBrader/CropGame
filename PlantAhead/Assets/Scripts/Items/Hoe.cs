﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(menuName = "Items/Hoe")]
public class Hoe : Item
{    
    public int Use(CustomTile selectedTile)
    {
        if(selectedTile != null) {
            if(selectedTile is TillableTile)
            {
                if(!(selectedTile as TillableTile).beenHoed && (selectedTile as TillableTile).plant == null)
                {
                    (selectedTile as TillableTile).Hoe();
                    return 100; //energyCost;
                }
            }
        }
        return 0;
    }
   
}
