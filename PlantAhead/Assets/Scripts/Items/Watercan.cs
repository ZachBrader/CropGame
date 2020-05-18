using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watercan : Item
{
    // Start is called before the first frame update
    void Start()
    {
        this.itemName = "Watercan";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Use(Vector3 selectedTile)
    {
        Debug.Log("Watered " + selectedTile);
    }
}
