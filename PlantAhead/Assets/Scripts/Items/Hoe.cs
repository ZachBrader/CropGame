using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : Item
{    
    // Start is called before the first frame update
    void Start()
    {
        this.name = "Hoe";
        print("Setting name to hoe");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(Vector3Int selectedTile)
    {
        Debug.Log("Hoe'd " + selectedTile);
    }
}
