using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour{

    [Header("First plant stage")] public Sprite stage1;
    [Header("Second plant stage")] public Sprite stage2;
    [Header("Third plant stage")] public Sprite stage3;
    [Header("Fourth plant stage, if needed")] public Sprite stage4;
    [Header("Time per phase, in seconds")] public int timer;


    public SpriteRenderer spriteRenderer; 
    private int plantStage = 0;
    private int waterLevel = 0;
    
    
    // Start is called before the first frame update
    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update(){
        
        changePlant();
        

    }

    /*
     * checks if plant has water
     */
    void waterLevelCheck(){
        if (waterLevel == 0){
            //kill plant
        }

        if (waterLevel == 0){
            
        }
    }
    
    /*
     * grows plants
     */
    void grow(){
        
        
    }

    /*
     * This method controls changing the plants visual image 
     *
     * 
     */
    void changePlant(){
        
        if (plantStage == 1){
            this.spriteRenderer.sprite = stage1;
        }
        if (plantStage == 2){
            this.spriteRenderer.sprite = stage2;
        }
        if (plantStage == 3){
            this.spriteRenderer.sprite = stage3;
        }
        if (plantStage == 4){
            this.spriteRenderer.sprite = stage4;
        }
    }
}
