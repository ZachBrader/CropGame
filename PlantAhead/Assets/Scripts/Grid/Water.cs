using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Water {
    
    public int WaterAmountMax;
    public int WaterAmount;

    public int getWater(int fillCanAmount) {
        if(WaterAmount >= fillCanAmount) {
            WaterAmount -= fillCanAmount;
            return fillCanAmount;
        }
        else if (WaterAmount < fillCanAmount && WaterAmount != 0) {
            var temp = WaterAmount;
            WaterAmount = 0;
            return temp;
        }
        else
        {
            UIManager.Instance.ActionStatus.text = "Water Source Empty!";
            UIManager.Instance.displayX();
            return 0;
        }
    }

    public void refillWaterSource() {
        WaterAmount = WaterAmountMax;
    }

}
