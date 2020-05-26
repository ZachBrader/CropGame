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

    [Header("Water needed per stage of growth")] public int waterCost;

    public Animator animator;

    private float lastSparkle = 0f; // time since last sparkle
    private float timeBetweenSparkles = 5f; // how frequently to sparkle; update when watering and growing

    //convey water level via sparkles or something

    public SpriteRenderer spriteRenderer; 
    private int plantStage = 0;
    public int waterLevel = 1;
    private float stageTime = 0f;

    // Determines how much a plant is worth
    public int averagePlantValue;

    // is the plant a mushroom actually
    public bool isMushroom;
    
    
    // Start is called before the first frame update
    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();

        // TEST CODE
        averagePlantValue = 5;
        // TEST CODE
    }

    // Update is called once per frame
    void Update(){
        grow();

        if (waterLevel > 0) 
        {
            lastSparkle += Time.deltaTime;
            if(lastSparkle > timeBetweenSparkles)
            {
                lastSparkle = timeBetweenSparkles - lastSparkle;
                animator.SetTrigger("Water Sparkle");
            }
        }
        
    }

    /*
     * checks if plant has water
     */
    void waterLevelCheck(){
        if (waterLevel > 0){
            waterLevel -= waterCost;
        }
    }

    /*
     * maximizes water value of plant
     */
    public void waterPlant(int waterAmount){
        Debug.Log("Water successful");
        waterLevel += waterAmount;
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
        //if (stageTime >= this.timer && plantStage != 5){
        //    plantStage++;
        //    stageTime = 0f;
        //    waterLevelCheck();
        //   changePlant();
        //}
    }

    public bool plantStageUpdate(){
        Debug.Log("Plant grew!");
        if (waterLevel > 1){
            waterLevel -= 1;
            plantStage++;
            changePlant();
            return true;

        }else if (waterLevel > 0){
            plantStage++;
            changePlant();
            return false;

        }else{
            Destroy(this.gameObject);
            return false;
        }
    }

    /*
     * destroys the plant if it is not reusable 
     */
    public int harvest(){
        Debug.Log("Harvest successful");
        if (reusable)
        {
            this.spriteRenderer.sprite = stage2;
        }
        else
        {
            Destroy(this.gameObject);
        }

        return 5;
    }

    // This will return the amount of money the plant sells for
    public int ValuePlant()
    {
        return averagePlantValue * plantStage;
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

    public int GetPlantStage()
    {
        return plantStage;
    }
}
