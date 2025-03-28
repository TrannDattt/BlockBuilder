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
        private const int FIELD_HEIGHT = 15;
        private const int FIELD_WIDTH = 15;

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
            foreach(var groundTileData in groundTileDatas)
            {
                Tiles[groundTileData.TileIndex].ChangeTileType(ETileType.Ground);
            }

            foreach(var doorTileData in doorTileDatas)
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
            foreach(var blockData in blockDatas)
            {
                var tile = Tiles[blockData.PosIndex];
                var newBlock = BlockPooling.Instance.GetBlock(blockData.Shape, new(tile.transform.position.x, tile.transform.position.y, -1));
                newBlock.UpdateBlockData(blockData);

                UpdateBlockPos(newBlock, tile);
            }
        }

        public void UpdateBlockPos(EditorBlock block, EditorTile tile)
        {
            if(BlockDict.ContainsKey(block))
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
        }
    }
}
