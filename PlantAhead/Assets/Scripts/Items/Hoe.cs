using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : Item
{    
    // Start is called before the first frame update
    void Start()
    {
        this.itemName = "Hoe";
        this.triggerName = "Hoe";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(Vector3 selectedTile)
    {
        Debug.Log("Hoe'd " + selectedTile);
    }
}
