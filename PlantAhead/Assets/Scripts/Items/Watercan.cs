using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watercan : Item
{
    // Start is called before the first frame update
    void Start()
    {
        this.name = "Watercan";
        print("Setting name to watercan");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(Vector3Int selectedTile)
    {
        Debug.Log("Watered " + selectedTile);
    }
}
