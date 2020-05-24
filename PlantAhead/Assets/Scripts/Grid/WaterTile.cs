using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : CustomTile
{
    public Water waterSource;

    public WaterTile(Water water)
    {
        waterSource = water;
    }

    public int refillCan(int waterAmount)
    {
        return waterSource.getWater(waterAmount);
    }
    
}
