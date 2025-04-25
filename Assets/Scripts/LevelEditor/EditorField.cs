using BuilderTool.Editor;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BuilderTool.FileConvert.BlockInfoConverter;
using static BuilderTool.FileConvert.FieldInfoConverter;

namespace BuilderTool.LevelEditor
{
    public class EditorField : Singleton<EditorField>
    {
        public const int FIELD_HEIGHT = 15;
        public const int FIELD_WIDTH = 15;

        [field: SerializeField] public List<EditorTile> Tiles { get; private set; }

        public Dictionary<EditorBlock, EditorTile> BlockDict { get; private set; } = new();

        public void CreateNewField()
        {
            foreach (var tile in Tiles)
            {
                tile.InitTile();
            }

            BlockDict.Clear();
        }

        public void UpdateTileData(List<GroundTileData> groundTileDatas, List<DoorTileData> doorTileDatas, List<BlockTileData> blockTileDatas)
        {
            foreach (var groundTileData in groundTileDatas)
            {
                Tiles[groundTileData.TileIndex].ChangeTileType(ETileType.Ground);
            }

            foreach (var doorTileData in doorTileDatas)
            {
                int index = doorTileData.TileIndex;
                Tiles[index].ChangeTileType(ETileType.Door);
                (Tiles[index].CurTileAttribute as DoorTile).UpdateAttribute(doorTileData);
            }

            foreach (var blockTileData in blockTileDatas)
            {
                int index = blockTileData.TileIndex;
                Tiles[index].ChangeTileType(ETileType.BlockNode);
                (Tiles[index].CurTileAttribute as BlockTile).UpdateAttribute(blockTileData);
            }
        }

        public void UpdateBlockData(List<BlockData> blockDatas)
        {
            foreach (var blockData in blockDatas)
            {
                var tile = Tiles[blockData.PosIndex];

                var newBlock = BlockPooling.Instance.GetBlock(blockData.Shape, new(tile.transform.position.x, tile.transform.position.y, -1));
                newBlock.UpdateBlockData(blockData);

                UpdateBlockPos(newBlock, tile);
            }
        }

        public void UpdateBlockPos(EditorBlock block, EditorTile tile)
        {
            //Vector3 worldPos = tile.transform.position;
            //float x = worldPos.x;
            //float y = worldPos.y;
            //Debug.Log($"World Position of the tile: x = {x}, y = {y}");
            
            int tileIndex = Tiles.IndexOf(tile);
            Debug.Log("[UpdateBlockPos] Tile Index: " + tileIndex + ", Block: " + block.Shape + ", Tile: " + tile.TileType + ", Position: (" + tile.GridX + "," + tile.GridY + ")");

            if (BlockDict.ContainsKey(block))
            {
                BlockDict[block] = tile;

            }
            else
            {
                BlockDict.Add(block, tile);
            }
        }

        public void RemoveBlock(EditorBlock block)
        {
            if (!BlockDict.ContainsKey(block))
            {
                return;
            }
            BlockDict.Remove(block);
        }

        private void Start()
        {
            CreateNewField();
            AssignGridCoordinatesToTiles(); 
            DebugTileGridInfo();
            //for (int y = 0; y < FIELD_HEIGHT; y++)
            //{
            //    for (int x = 0; x < FIELD_WIDTH; x++)
            //    {
            //        int index = y * FIELD_WIDTH + x;
            //        if (index < Tiles.Count)
            //        {
            //            Tiles[index].GridX = x;
            //            Tiles[index].GridY = y;
            //        }
            //    }
            //}
        }
        public void ExportBlockDataToLevelHolder()
        {
            List<Block3DData> block3DList = new();

            foreach (var entry in BlockDict)
            {
                EditorBlock block = entry.Key;
                EditorTile tile = entry.Value;

                Vector3 blockPosition = new Vector3(tile.GridX, -tile.GridY, 0);
                block3DList.Add(new Block3DData(block.Shape, blockPosition));
            }

            // Gửi danh sách block vào LevelDataHolder
            LevelDataHolder.Instance.SetBlock3DList(block3DList);

            Debug.Log($"[Export] Đã export {block3DList.Count} block sang LevelDataHolder");
        }

        public void DebugTileGridInfo()
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                var tile = Tiles[i];
                //Debug.Log($"[Tile Info] Index: {i}, Grid: ({tile.GridX},{tile.GridY}), WorldPos: {tile.transform.position}");
            }
        }

        private void AssignGridCoordinatesToTiles()
        {
            float cellSize = 1f;

            for (int y = 0; y < FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < FIELD_WIDTH; x++)
                {
                    int index = y * FIELD_WIDTH + x;
                    if (index < Tiles.Count)
                    {
                        Tiles[index].GridX = x;
                        Tiles[index].GridY = y;

                        //float worldX = x * cellSize;
                        //float worldY = -y * cellSize;
                        //Tiles[index].transform.position = new Vector3(worldX, worldY, 0);

                        //Debug.Log($"[AssignGrid] Tile index {index} - Grid({x},{y}) -> WorldPos({worldX},{worldY})");
                    }
                }
            }
        }
    }
}
