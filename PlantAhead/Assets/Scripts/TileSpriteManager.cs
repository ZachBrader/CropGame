using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile Sprite Manager")]
public class TileSpriteManager : ScriptableObject
{
    public static TileSpriteManager Instance;
    public Tile hoedTile;

    public Tile dirtTile;

}
