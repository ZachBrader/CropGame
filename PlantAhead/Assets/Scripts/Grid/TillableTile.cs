﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TillableTile : CustomTile
{
    public Plant plant = null;
    public bool beenHoed = false;

    public void Hoe()
    {
        beenHoed = true;
        //color = new Color(180/255f, 147/255f, 144/255f, 98/255f); 
    }
}