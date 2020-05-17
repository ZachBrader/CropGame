using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour{

    [Header("First plant stage")] public Sprite stage1;
    [Header("Second plant stage")] public Sprite stage2;
    [Header("Third plant stage")] public Sprite stage3;
    [Header("Fourth plant stage, if needed")] public Sprite stage4;
    [Header("Time per phase, in seconds")] public int timer;
    [Header("Is the item reharvestable?")] public bool reusable;

    //convey water level via sparkles or something

    public SpriteRenderer spriteRenderer; 
    private int plantStage = 0;
    private float waterLevel = 0f;
    private float stageTime = 0f;
    
    
    // Start is called before the first frame update
    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        grow();
        
    }

    /*
     * checks if plant has water
     */
    void waterLevelCheck(){
        if (waterLevel <= 0f){
            Destroy(this.gameObject);
        }

        if (waterLevel > 0){
            waterLevel -= 0.33f;
        }
    }

    /*
     * maximizes water value of plant
     */
    void waterPlant(){
        waterLevel = 1;
    }
    
    /*
     * grows plants
     */
    void grow(){
        if (plantStage == 0){
            stageTime += Time.deltaTime;

        }else if (plantStage == 1){
            stageTime += Time.deltaTime;
            
        }else if (plantStage == 2){
            stageTime += Time.deltaTime;
            
        }else if (plantStage == 3){
            stageTime += Time.deltaTime;
            
        }else if (plantStage == 4){
            stageTime += Time.deltaTime;
            
        }else if (plantStage == 5){
            
        }

        // change plant to next stage
        if (stageTime >= this.timer && plantStage != 5){
            plantStage++;
            stageTime = 0f;
            waterLevelCheck();
            changePlant();
        }
    }

    void plantStageUpdate(){
        if (plantStage != 5){
            plantStage++;
        }
    }

    /*
     * destroys the plant if it is not reusable 
     */
    void harvest(){

        if (reusable){
            this.spriteRenderer.sprite = stage2;
        }
        else{
            Destroy(this.gameObject);
        }
    }

    /*
     * This method controls changing the plants visual image 
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
