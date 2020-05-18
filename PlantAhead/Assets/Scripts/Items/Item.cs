using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    // Start is called before the first frame update
    void Start()
    {
        itemName = "None";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Use(Vector3 selectedTile)
    {
        Debug.Log("Using item");
    }
}
