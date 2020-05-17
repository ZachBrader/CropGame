using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Item
{
    // Start is called before the first frame update
    void Start()
    {
        this.name = "Seed";
        print("Setting name to seed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(Vector3Int selectedTile)
    {
        Debug.Log("Planted Seed at " + selectedTile);
    }
}
