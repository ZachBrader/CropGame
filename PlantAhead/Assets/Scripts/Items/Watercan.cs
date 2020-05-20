using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Items/WateringCan")]
public class Watercan : Item
{
    [SerializeField]
    private bool nearWater = false;

    public Image waterBar;

    public int maxWaterinCan = 100;

    public int waterCurrentlyinCan = 0;

    public int waterPerUse = 1;
    

    // do a watering can action. If near water, this will try to fill the watering can
    public override int Use(TileBase selectedTile)
    {


        // fill the can if near water
        if(nearWater)
        {
            waterCurrentlyinCan = maxWaterinCan;
            // update water level amount
            return 0;
        }
            
        if(selectedTile != null){
            bool didWater;
            if(waterCurrentlyinCan >= waterPerUse) 
            {
                Debug.Log("Watered " + selectedTile);
                //  better hope you aren't wasting water and energy on not actually watering a plant
                waterCurrentlyinCan -= waterPerUse;
                didWater = true;
                // act on the plant here
                
                // only call if not null
                // if ((selectedTile as TillableTile).plant != null){
                //     (selectedTile as TillableTile).plant.waterPlant(waterPerUse);
                // }
            }
            else
            {
                Debug.Log("Watering Can is Empty");
                return 0;
                // notify player watering can out of water
            }
            // update the fill bar per use
            // make sure no div 0
            if(didWater) {
                
                if(maxWaterinCan == 0){
                    maxWaterinCan = 1;
                }
                waterBar.fillAmount = (float) waterCurrentlyinCan / (float) maxWaterinCan;
                return energyCost;
            }
        }
        

        return 0;
    }



    public void OnTriggerEnter2D(Collider2D collison) 
    {
        if(collison.CompareTag("Water"))
        {
            nearWater = true;
        }

    }

    public void OnTriggerExit2D(Collider2D collison) 
    {
        if(collison.CompareTag("Water"))
        {
            nearWater = false;
        }
    }


    
}
