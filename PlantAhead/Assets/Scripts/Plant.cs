using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour{

    [Header("First plant stage")] 
    public Sprite stage1; 
    
    [Header("Second plant stage")] 
    public Sprite stage2;
    
    [Header("Third plant stage")] 
    public Sprite stage3;
    
    [Header("Fourth plant stage")]
    public Sprite stage4;


    private int plantStage = 0;
    private int waterLevel = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        
        changePlant();
        

    }

    /*
     * This method controls changing the plants visual image 
     *
     * 
     */
    void changePlant(){
        if (plantStage == 1){
            this.gameObject.GetComponent<SpriteRenderer>().sprite = stage1;
        }
        if (plantStage == 2){
            this.gameObject.GetComponent<SpriteRenderer>().sprite = stage2;
        }
        if (plantStage == 3){
            this.gameObject.GetComponent<SpriteRenderer>().sprite = stage3;
        }
        if (plantStage == 3){
            this.gameObject.GetComponent<SpriteRenderer>().sprite = stage4;
        }
    }
}
