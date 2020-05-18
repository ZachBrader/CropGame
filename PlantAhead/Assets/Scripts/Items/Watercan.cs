using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Watercan : Item
{
    [SerializeField]
    private bool nearWater = false;

    public Image waterBar;

    public int maxWaterinCan = 100;

    public int waterCurrentlyinCan = 0;

    public int waterPerUse = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.itemName = "Watercan";
        this.triggerName = "Water";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // do a watering can action. If near water, this will try to fill the watering can
    public override int Use(Vector3 selectedTile)
    {
        // fill the can if near water
        if(nearWater)
        {
            waterCurrentlyinCan = maxWaterinCan;
            // update water level amount

            return 0;
        }
        else if(waterCurrentlyinCan >= waterPerUse) 
        {
            Debug.Log("Watered " + selectedTile);
            waterCurrentlyinCan -= waterPerUse;
            // act on the plant here
            
        }
        else
        {
            // notify player watering can out of water
        }
        // update the fill bar per use
        // make sure no div 0
        if(maxWaterinCan == 0){
            maxWaterinCan = 1;
        }

        waterBar.fillAmount = (float) waterCurrentlyinCan / (float) maxWaterinCan;

        return energyCost;
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
