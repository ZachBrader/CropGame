﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Items/WateringCan")]
public class Watercan : Item
{
    [SerializeField]
    private bool nearWater = false;
    
    public int maxWaterinCan = 10;

    public int waterCurrentlyinCan = 10;

    public int waterPerUse = 1;
    
    // do a watering can action. If near water, this will try to fill the watering can
    public int Use(CustomTile selectedTile)
    {
        if(selectedTile != null)
        {
            if(selectedTile is WaterTile)
            {
                // refill the can
                var fillAmount = (selectedTile as WaterTile).refillCan(maxWaterinCan - waterCurrentlyinCan);
                waterCurrentlyinCan += fillAmount;
                if(maxWaterinCan == 0){
                        maxWaterinCan = 1;
                }

                if (waterCurrentlyinCan >= maxWaterinCan)
                {
                    UIManager.Instance.SendNotification("Filled Watercan!");
                }
                UIManager.Instance.waterBar.fillAmount = (float) waterCurrentlyinCan / (float) maxWaterinCan;
                return 0;
            }
                
            //else if((selectedTile as TillableTile).plant != null){
            else {
                bool didWater = false;

                if(waterCurrentlyinCan >= waterPerUse) 
                {
                    //  better hope you aren't wasting water and energy on not actually watering a plant
                    waterCurrentlyinCan -= waterPerUse;
                    didWater = true;
                    // act on the plant here
                    
                    //only call if not null
                    if ((selectedTile as TillableTile).plant != null){
                        (selectedTile as TillableTile).plant.waterPlant(waterPerUse);
                        (selectedTile as TillableTile).plant.MakeSparkle();
                    }
                }
                else
                {
                    UIManager.Instance.SendNotification("Watering Can is Empty!");
                    UIManager.Instance.DisplayRedX();
                    return 0;
                    // notify player watering can out of water
                }
                // update the fill bar per use
                // make sure no div 0
                if (didWater) {
                    if(maxWaterinCan == 0){
                        maxWaterinCan = 1;
                    }
                    UIManager.Instance.waterBar.fillAmount = (float) waterCurrentlyinCan / (float) maxWaterinCan;
                    return energyCost;
                }
            }
        }

        return 0;
    }

    public void updateWaterBar()
    {
        if(maxWaterinCan == 0){
            maxWaterinCan = 1;
        }
        
        UIManager.Instance.waterBar.fillAmount = (float) waterCurrentlyinCan / (float) maxWaterinCan;
    }
    
}
