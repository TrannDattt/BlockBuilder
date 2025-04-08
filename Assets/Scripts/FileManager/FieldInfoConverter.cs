using BuilderTool.Enums;
using BuilderTool.LevelEditor;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuilderTool.FileConvert
{
    public static class FieldInfoConverter
    {
        [Serializable]
        public class GroundTileData
        {
            public int TileIndex;
            public ETileType TileType;

            public GroundTileData(EditorTile tile)
            {
                TileType = ETileType.Ground;
                TileIndex = EditorField.Instance.Tiles.IndexOf(tile);
            }
        }

        [Serializable]
        public class DoorTileData
        {
            [Serializable]
            public class DoorData
            {
                public EColor DoorColor;

                public DoorData(EColor color)
                {
                    DoorColor = color;
                }
            }

            public int TileIndex;
            public DoorData UpDoor;
            public DoorData DownDoor;
            public DoorData LeftDoor;
            public DoorData RightDoor;
            public ETileType TileType;

            public DoorTileData(EditorTile tile)
            {
                TileIndex = EditorField.Instance.Tiles.IndexOf(tile);

                var doorTile = tile.CurTileAttribute as DoorTile;
                UpDoor = new DoorData(doorTile.DoorDict[EDirection.Up]); 
                DownDoor = new DoorData(doorTile.DoorDict[EDirection.Down]); 
                LeftDoor = new DoorData(doorTile.DoorDict[EDirection.Left]); 
                RightDoor = new DoorData(doorTile.DoorDict[EDirection.Right]);
                TileType = UpDoor.DoorColor != 0
                           || DownDoor.DoorColor != 0
                           || LeftDoor.DoorColor != 0
                           || RightDoor.DoorColor != 0 ? ETileType.Door : ETileType.Ground;
            }
        }

        [Serializable]
        public class BlockTileData
        {
            public int TileIndex;
            public EColor BlockColor;
            public ETileType TileType;

            public BlockTileData(EditorTile tile)
            {
                TileIndex = EditorField.Instance.Tiles.IndexOf(tile);
                BlockColor = (tile.CurTileAttribute as BlockTile).Color;
                TileType = ETileType.BlockNode;
            }
        }

        [Serializable]
        public class TileListWrapper
        {
            public List<Dictionary<string, object>> Tiles;
        }

        public static string ConvertTileToJson()
        {
            string json = "";
            var tiles = EditorField.Instance.Tiles;
            
            foreach(var tile in tiles)
            {
                switch (tile.CurTileAttribute)
                {
                    case EmptyTile:
                        break;

                    case GroundTile:
                        json += JsonUtility.ToJson(new GroundTileData(tile)) + ',';
                        break;

                    case DoorTile:
                        json += JsonUtility.ToJson(new DoorTileData(tile)) + ',';
                        json = Regex.Replace(json, @",?""(UpDoor|DownDoor|LeftDoor|RightDoor)"":\{""DoorColor"":0\}", "");
                        break;

                    case BlockTile:
                        json += JsonUtility.ToJson(new BlockTileData(tile)) + ',';
                        break;

                    default:
                        break;
                }
            }

            if (json.EndsWith(","))
            {
                json = json[..^1];
            }

            return json;
        }

        public static void ConvertJsonToTile(string json)
        {
            JArray tileArray = JArray.Parse(json);

            List<string> groundTileJson = new();
            List<string> doorTileJson = new();
            List<string> blockTileJson = new();

            foreach (var tile in tileArray)
            {
                int tileType = tile["TileType"]?.Value<int>() ?? -1;
                string tileJson = tile.ToString(Formatting.None);
                //Debug.Log(tileJson);

                if (tileType == 1)
                {
                    groundTileJson.Add(tileJson);
                }
                else if (tileType == 2)
                {
                    doorTileJson.Add(tileJson);
                }
                else
                {
                    blockTileJson.Add(tileJson);
                }
            }

            List<GroundTileData> groundTiles = ConvertJsonToList<GroundTileData>(groundTileJson);
            List<DoorTileData> doorTiles = ConvertJsonToList<DoorTileData>(doorTileJson);
            List<BlockTileData> blockTiles = ConvertJsonToList<BlockTileData>(blockTileJson);

            EditorField.Instance.UpdateTileData(groundTiles, doorTiles, blockTiles);
        }

        private static List<T> ConvertJsonToList<T>(List<string> jsonList)
        {
            List<T> data = new();
            foreach(var json in jsonList)
            {
                //Debug.Log(json);
                var newData = JsonUtility.FromJson<T>(json);
                data.Add(newData);
            }

            return data;
        }
    }
}
