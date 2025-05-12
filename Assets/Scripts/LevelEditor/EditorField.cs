using BuilderTool.Editor;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BuilderTool.FileConvert.BlockInfoConverter;
using static BuilderTool.FileConvert.FieldInfoConverter;
using static BuilderTool.FileConvert.MechanicInfoConverter;

namespace BuilderTool.LevelEditor
{
    public class EditorField : Singleton<EditorField>
    {
        public const int FIELD_HEIGHT = 15;
        public const int FIELD_WIDTH = 15;

        [field: SerializeField] public List<EditorTile> Tiles { get; private set; }

        public Dictionary<EditorBlock, EditorTile> BlockDict { get; private set; } = new();
        public Dictionary<Tuple<ICanHaveMechanic, EDirection>, AMechanic> MechanicDict { get; private set; } = new();

        public void CreateNewField()
        {
            foreach (var tile in Tiles)
            {
                tile.InitTile();
            }

            BlockDict.Clear();
            MechanicDict.Clear();
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

        /// <summary>
        /// Get mechanic that assigned to block/tile
        /// </summary>
        /// <param name="obj">Block/tile to contain mechanic</param>
        /// <param name="dir">Direction of mechanic which always equals to None if object is block</param>
        /// <returns></returns>
        public AMechanic GetMechanicFromDict(ICanHaveMechanic obj, EDirection dir = EDirection.None){
            var key = new Tuple<ICanHaveMechanic, EDirection>(obj, dir);
            if(MechanicDict.ContainsKey(key)){
                return MechanicDict[key];
            }

            return null;
        }

        public void UpdateMechanic(AMechanic mechanic, ICanHaveMechanic obj, EDirection dir){
            var key = new Tuple<ICanHaveMechanic, EDirection>(obj, dir);
            // Debug.Log(MechanicDict.Count);
            if(MechanicDict.ContainsKey(key)){
                MechanicDict[key] = mechanic;
                return;
            }

            if(mechanic == null){
                return;
            }

            MechanicDict.Add(key, mechanic);
        }

        public void UpdateMechanicData(params List<MechanicData>[] mechanicDataLists){
            foreach(var mechanicDataList in mechanicDataLists){
                foreach(var mechanicData in mechanicDataList){
                    var dir = mechanicData.Direction;

                    ICanHaveMechanic obj;
                    if(dir == EDirection.None)
                    {
                        var tile = Tiles[mechanicData.PosIndex];
                        var block = BlockDict.FirstOrDefault(b => b.Value == tile).Key;
                        obj = block.GetComponent<EditorBlock>(); 
                    }
                    else{
                        var tile = Tiles[mechanicData.PosIndex];
                        obj = tile.GetComponentInChildren<DoorTile>();
                    }

                    AMechanic mechanic = null;
                    switch(mechanicData.MechanicType){
                        case EMechanic.Freeze:
                            mechanic = new Freeze(EMechanic.Freeze, dir, (mechanicData as FreezeMechanicData).TurnCount);
                            UpdateMechanic(mechanic, obj, dir);
                            break;

                        case EMechanic.Chained:
                            mechanic = new Chain(EMechanic.Chained, dir, (mechanicData as ChainMechanicData).TurnCount);
                            UpdateMechanic(mechanic, obj, dir);
                            break;
                    }

                    if(mechanic == null){
                        continue;
                    }

                    if(dir == EDirection.None)
                    {
                        BlockAttributeSelector.Instance.LoadBlockMechanic(mechanic, obj);
                    }
                    else{
                        TileAttributeSelector.Instance.LoadBlockMechanic(mechanic, obj);
                    }
                }
            }
        }

        private void LogDict(){
            foreach(var m in MechanicDict){
                Debug.Log(m.Key.Item1 + " " + m.Key.Item2 + " " + m.Value);
            }
        }

        private void Start()
        {
            CreateNewField();
        }
    }
}
