using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Item
{

    // Which type of plant it will grow
    public GameObject plant;

    [HideInInspector]
    public int seedCount = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override int Use(Vector3Int selectedTile)
    {
        Debug.Log("Planted Seed at " + selectedTile);
        if (plant.tag.Equals("Plant"))
        {
            Instantiate(plant, selectedTile, Quaternion.identity);

        }
        return energyCost;
    }
}
