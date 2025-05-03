using UnityEngine;
using BuilderTool.Enums;
using BuilderTool.LevelEditor;
using System.Collections.Generic;

public class Tile3D : MonoBehaviour{
    public ETileType TileType {get; private set;}

    public void InitTile(EditorTile tile2D){
        TileType = tile2D.TileType;
    }
}
