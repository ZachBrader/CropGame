using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector]
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        name = "None";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Use(Vector3Int selectedTile)
    {
        Debug.Log("Using item");
    }
}
