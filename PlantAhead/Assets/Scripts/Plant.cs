using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Plant : MonoBehaviour{

    [Header("First plant stage")] public Sprite stage1;
    [Header("Second plant stage")] public Sprite stage2;
    [Header("Third plant stage")] public Sprite stage3;
    [Header("Fourth plant stage, if needed")] public Sprite stage4;
    [Header("Time per phase, in seconds")] public int timer;
    [Header("Is the item reharvestable?")] public bool reusable;

    [Header("Water needed per stage of growth")] public int waterCost;

    public Animator animator;
    public Animator animator2;

    private float lastSparkle = 0f; // time since last sparkle
    private float timeBetweenSparkles;
    public float MaxTimeBetweenSparkles = 5f; // how frequently to sparkle; update when watering and growing

    public GameObject myPrefab;
    //convey water level via sparkles or something

    public SpriteRenderer spriteRenderer; 
    public int plantStage = 0;
    public int waterLevel = 1;
    private float stageTime = 0f;

    // Determines how much a plant is worth
    public int averagePlantValue;

    // is the plant a mushroom actually
    public bool isMushroom;
    
    public int EnergyCost; // how much energy does it cost to remove the thing

    public int SpreadZone; // how large of a radius can it spread?

    [SerializeField]
    private float valueModifier;
    public int PerfectWaterAmount;

    private bool isDead = false;
    public int spreadRate;
    private int dayCreated = -1;

    
    // Start is called before the first frame update
    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        valueModifier = 1;
        timeBetweenSparkles = MaxTimeBetweenSparkles;

    }

    // Update is called once per frame
    void Update(){
        
        if (waterLevel > 0 && waterLevel < 2 * PerfectWaterAmount) 
        {
            lastSparkle += Time.deltaTime;
            if(lastSparkle > timeBetweenSparkles)
            {
                lastSparkle = timeBetweenSparkles - lastSparkle;
                animator.SetTrigger("Water Sparkle");
            }
        }
        // make it VERY clear that it's ready to be harvested
        if(plantStage == 4 && !isDead){
            animator2.SetBool("PlantisReady", true);
        }
    }

    // used to sparkle on water
    public void MakeSparkle(){
        animator.SetTrigger("Water Sparkle");
    }

    /*
     * maximizes water value of plant
     */
    public void waterPlant(int waterAmount) {
        waterLevel += waterAmount;
        
        var temp = ((waterLevel) / (float) PerfectWaterAmount);
        Debug.Log(temp);
        if (temp <= 1) // sparkle aggresively
        {
            timeBetweenSparkles = ((1 - temp) * MaxTimeBetweenSparkles);
        }
        else
        {   
            if(temp >= 2) // it's dead jim
            {
                Debug.Log("Plant killed while watering!");
                KillPlant();
            } 
            else
            {
                // it's sick and sad
                timeBetweenSparkles = MaxTimeBetweenSparkles;
                spriteRenderer.color = new Color(0.4518f, 0.5094f, .1033f, 1.0f);
            }
        }
    }

    public bool plantStageUpdate(){
        
        timeBetweenSparkles = MaxTimeBetweenSparkles;
        spriteRenderer.color = Color.white;
        timeBetweenSparkles = 0f;
        
        if (waterLevel > 1) {
            var waterbonus = PerfectWaterAmount - waterLevel;
            //watered more than once and less than or equal to perfectly
            if (waterbonus > 0) {
                // adjust what the plant is worth
                valueModifier += (waterbonus / (float) PerfectWaterAmount) * 0.2f;
                waterLevel = 0;
                plantStage++;
                changePlant();
                return true;
            }
            // watered it a little too much
            else if (waterbonus + PerfectWaterAmount > 1) 
            {
                valueModifier += ((waterbonus + PerfectWaterAmount) / (float) PerfectWaterAmount) * 0.1f;
                waterLevel = 0;
                plantStage++;
                changePlant();
                return true;
            }
            else // watered it way too mch
            {
                KillPlant();
                return false;
            }
        }
        
        else if (waterLevel > 0 || isMushroom){
            plantStage++;
            changePlant();
            waterLevel = 0;
            return false;

        }
        else
        {
            KillPlant();
            return false;
        }
    }

    /*
     * destroys the plant if it is not reusable 
     */
    public int harvest(){
        animator2.SetBool("PlantisReady", false);
        if (reusable)
        {
            //harvest reusable too early and it will be destroyed
            if (plantStage > 2) {
                plantStage = 2;
                this.spriteRenderer.sprite = stage2;
            }
            else {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        if(isMushroom)
        {
            return EnergyCost + plantStage - 1 * 2; // more developed mushrooms are harder to remove
        }
        return EnergyCost;
    }

    // This will return the amount of money the plant sells for
    public int ValuePlant(){
        if (isMushroom || isDead){
            return 0;
        }

        if (reusable){

            //ignore first three stages for returning money because it would make plant immortal
            if (plantStage > 2){
                System.Random rand = new System.Random();
                return (int)averagePlantValue * plantStage / 4 + rand.Next(-3, 3);
                //return (int)Math.Pow(plantStage-2, 1.5 + (averagePlantValue / 10));
            }
            else{
                return 0;
            }
        }
        else{
            System.Random rand = new System.Random();
            return (int)averagePlantValue * plantStage / 4 + rand.Next(-3, 3);
            // return (int)Math.Pow(plantStage, 1.5 + (averagePlantValue / 10));
        }
        
        return Mathf.RoundToInt(averagePlantValue * plantStage * valueModifier);
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

    private void KillPlant()
    {
        animator2.SetBool("PlantisReady", false);
        spriteRenderer.color = Color.black;
        isDead = true;
    }

    public void resetPlant(){
        this.spriteRenderer.sprite = stage1;
    }

    public int GetPlantStage()
    {
        return plantStage;
    }

    public bool getIsDeadStatus()
    {
        return isDead;
    }

    public int GetDayCreated()
    {
        return dayCreated;
    }

    public void SetDayCreated(int dayMade)
    {
        dayCreated = dayMade;
    }
}
