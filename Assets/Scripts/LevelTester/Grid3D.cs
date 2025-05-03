using UnityEngine;
using System.Collections.Generic;
using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using BuilderTool.Enums;

public class Grid3D : Singleton<Grid3D>
{
    [SerializeField] private Tile3D _tilePreb;
    [SerializeField] private Wall3D _wallPreb;
    [SerializeField] private Wall3D _cornerPreb;
    [SerializeField] private Door3D _doorPreb;
    [SerializeField] private Pillar3D _pillarPreb;

    private Tile3D[,] _floor3D;

    private int _floorHeight => _floor3D.GetLength(0);
    private int _floorWidth => _floor3D.GetLength(1);

    public void Geneate3DGrid(){
        GenerateFloor(EditorField.Instance.Tiles);
        GenerateWall();
        GenerateBlock();
        GenerateObstacle();
    }

    private void GenerateFloor(List<EditorTile> tiles){
        _floor3D = new Tile3D[15, 15];

        for(int row = 0; row < _floorHeight; row++){
            for(int col = 0; col < _floorWidth; col++){
                var tile = tiles[row * _floorHeight + col];
                var tilePos = new Vector2(-_floorWidth / 2 + col, -_floorHeight / 2 + row);

                if(tile.TileType != ETileType.Empty)
                {
                    _floor3D[row, col] = Instantiate(_tilePreb, tilePos, Quaternion.identity);
                    _floor3D[row, col].InitTile(tile);
                    _floor3D[row, col].transform.SetParent(transform);
                }
            }
        }
    }

    private void GenerateWall(){
        for(int row = 0; row < _floorHeight; row++){
            for(int col = 0; col < _floorWidth; col++){
                if(!_floor3D[row, col]){
                    continue;
                }

                int index = row * _floorHeight + col;

                // Place walls/doors
                if(!CheckHaveTile(row - 1, col)){
                    PlaceWall(_floor3D[row, col], index, EDirection.Down);
                }

                if(!CheckHaveTile(row + 1, col)){
                    PlaceWall(_floor3D[row, col], index, EDirection.Up);
                }

                if(!CheckHaveTile(row, col - 1)){
                    PlaceWall(_floor3D[row, col], index, EDirection.Left);
                }

                if(!CheckHaveTile(row, col + 1)){
                    PlaceWall(_floor3D[row, col], index, EDirection.Right);
                }

                // Place corners
                if(!CheckHaveTile(row - 1, col - 1) && !CheckHaveTile(row - 1, col) && !CheckHaveTile(row, col - 1)){
                    PlaceCorner(_floor3D[row, col], EDirection.Down, EDirection.Left);
                }

                if(!CheckHaveTile(row - 1, col + 1) && !CheckHaveTile(row - 1, col) && !CheckHaveTile(row, col + 1)){
                    PlaceCorner(_floor3D[row, col], EDirection.Down, EDirection.Right);
                }

                if(!CheckHaveTile(row + 1, col - 1) && !CheckHaveTile(row + 1, col) && !CheckHaveTile(row, col - 1)){
                    PlaceCorner(_floor3D[row, col], EDirection.Up, EDirection.Left);
                }

                if(!CheckHaveTile(row + 1, col + 1) && !CheckHaveTile(row + 1, col) && !CheckHaveTile(row, col + 1)){
                    PlaceCorner(_floor3D[row, col], EDirection.Up, EDirection.Right);
                }
            }
        }
    }

    private bool CheckHaveTile(int row, int col){
        return col >= 0
               && col < _floorWidth
               && row >= 0
               && row < _floorHeight
               && _floor3D[row, col] != null;
    }

    private void PlaceWall(Tile3D tile, int tileIndex, EDirection placeDir){
        var tileCollider = tile.GetComponentInChildren<BoxCollider>();
        float halfSize = tileCollider.bounds.max.x - tileCollider.bounds.center.x;

        Vector2 offset = placeDir switch {
            EDirection.Up => new Vector2(0, halfSize),
            EDirection.Down => new Vector2(0, -halfSize),
            EDirection.Left => new Vector2(-halfSize, 0),
            EDirection.Right => new Vector2(halfSize, 0),
            _ => Vector2.zero,
        };

        Quaternion rotation = placeDir switch {
            EDirection.Up => Quaternion.identity,
            EDirection.Down => Quaternion.Euler(new Vector3(0, 0, 180)),
            EDirection.Left => Quaternion.Euler(new Vector3(0, 0, 90)),
            EDirection.Right => Quaternion.Euler(new Vector3(0, 0, 270)),
            _ => Quaternion.identity,
        };

        var pos = tile.transform.position + (Vector3)offset;

        if(tile.TileType == ETileType.Door)
        {
            var doorTile = EditorField.Instance.Tiles[tileIndex];
            var color = (doorTile.CurTileAttribute as DoorTile).DoorDict[placeDir];

            if(color != EColor.Black)
            {
                var spawnedDoor = Instantiate(_doorPreb, pos, rotation, transform);
                spawnedDoor.InitDoor(color);
            }
            else{
                var spawnedWall = Instantiate(_wallPreb, pos, rotation, transform);
                spawnedWall.InitWall();
            }
        }
        else{
            var spawnedWall = Instantiate(_wallPreb, pos, rotation, transform);
            spawnedWall.InitWall();
        }
    }

    private void PlaceCorner(Tile3D tile, EDirection verticalDir, EDirection horizontalDir){
        var tileCollider = tile.GetComponentInChildren<BoxCollider>();
        float halfSize = tileCollider.bounds.max.x - tileCollider.bounds.center.x;

        Vector3 verticalOffset = verticalDir switch {
            EDirection.Up => new Vector3(0, halfSize),
            EDirection.Down => new Vector3(0, -halfSize),
            _ => Vector3.zero,
        };

        Vector3 horizontalOffset = horizontalDir switch {
            EDirection.Left => new Vector3(-halfSize, 0),
            EDirection.Right => new Vector3(halfSize, 0),
            _ => Vector3.zero,
        };

        var rotation = verticalDir == EDirection.Up && horizontalDir == EDirection.Left ? Quaternion.identity :
            verticalDir == EDirection.Up && horizontalDir == EDirection.Right ? Quaternion.Euler(new Vector3(0, 0, -90)) :
            verticalDir == EDirection.Down && horizontalDir == EDirection.Left ? Quaternion.Euler(new Vector3(0, 0, 90)) :
            Quaternion.Euler(new Vector3(0, 0, 180));

        Vector2 pos = tile.transform.position + verticalOffset + horizontalOffset;
        var spawnedCorner = Instantiate(_cornerPreb, pos, rotation, transform);
    }

    private void GenerateBlock(){
        foreach(var block in EditorField.Instance.BlockDict.Keys){
            int tileIndex = EditorField.Instance.Tiles.IndexOf(EditorField.Instance.BlockDict[block]);
            Vector3 offset = new(0, 0, -.2f);
            Vector3 pos = _floor3D[tileIndex / _floorHeight, tileIndex % _floorWidth].transform.position + offset;

            BlockSpawner3D.Instance.SpawnBlock(block, pos);
        }
    }

    // TODO: Generate mechanic here
    private void GenerateObstacle(){
        PlacePillar();
    }

    private void PlacePillar(){
        for(int row = 0; row < _floorHeight; row++){
            for(int col = 0; col < _floorWidth; col++){
                if(!_floor3D[row, col] || _floor3D[row, col].TileType != ETileType.BlockNode){
                    continue;
                }

                var tileIndex = row * _floorHeight + col;
                var tileAttribute = EditorField.Instance.Tiles[tileIndex].CurTileAttribute as BlockTile;
                var spawnPos = _floor3D[row, col].transform.position;
                
                var spawnedPillar = Instantiate(_pillarPreb, spawnPos, Quaternion.identity, transform);
                spawnedPillar.InitPillar(tileAttribute.Color);
            }
        }
    }
}
