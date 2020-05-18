using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Tillable Tile")]
public class TillableTile : Tile
{
    public Plant plant;
}
