//using UnityEngine;
//using System.Collections.Generic;
//using System.IO;
//using System;
//using Newtonsoft.Json;
//using BuilderTool.FileConvert;
//using BuilderTool.Enums;

//public class LevelLoader3D : MonoBehaviour
//{
//    [Serializable]
//    public class LevelDataJSON
//    {
//        public LevelInfo LevelInfo;
//        public List<TileDataJSON> Tiles;
//        public List<BlockDataJSON> Blocks;
//    }

//    [Serializable]
//    public class LevelInfo
//    {
//        public string Id;
//        public int Time;
//        public int Difficulty;
//    }

//    [Serializable]
//    public class TileDataJSON
//    {
//        public int TileIndex;
//        public int TileType;
//    }

//    [Serializable]
//    public class BlockDataJSON
//    {
//        public int Shape;
//        public int PrimaryColor;
//        public int SecondaryColor;
//        public bool ContainKey;
//        public bool ContainStar;
//        public int PosIndex;
//    }

//    [Serializable]
//    public class TileData
//    {
//        public string Type; // "empty", "ground", "block"
//        public string BlockId; // Định danh của block (nếu có)

//        public TileData(string type, string blockId = "")
//        {
//            Type = type;
//            BlockId = blockId;
//        }
//    }

//    [SerializeField] private string levelFilePath = "Assets/Save_Levels/Bao_01.dat";
//    [SerializeField] private Grid3D grid3DComponent;
//    [SerializeField] private int gridWidth = 10; // Chiều rộng của grid 2D
//    [SerializeField] private int gridHeight = 6; // Chiều cao của grid 2D
//    [SerializeField] private int groundTileType = 1; // Giả định TileType = 1 là ground
//    [SerializeField]
//    private Dictionary<int, string> blockShapeToId = new Dictionary<int, string>()
//    {
//        { 3, "default" } // Ví dụ: Shape 3 tương ứng với BlockId "default"
//        // Thêm các mapping khác nếu cần
//    };

//    public void LoadLevel()
//    {
//        if (grid3DComponent == null)
//        {
//            grid3DComponent = GetComponent<Grid3D>();
//            if (grid3DComponent == null)
//            {
//                grid3DComponent = gameObject.AddComponent<Grid3D>();
//            }
//        }

//        List<List<TileData>> levelData = ReadLevelDataFromFile(levelFilePath);
//        grid3DComponent.LoadLevelFromData(levelData);
//    }

//    public void LoadLevelFromPath(string path)
//    {
//        levelFilePath = path;
//        LoadLevel();
//    }

//    public List<List<TileData>> ReadLevelDataFromFile(string filePath)
//    {
//        List<List<TileData>> tilesData = new List<List<TileData>>();

//        try
//        {
//            string json = File.ReadAllText(filePath);
//            LevelDataJSON levelDataJSON = JsonConvert.DeserializeObject<LevelDataJSON>(json);

//            if (levelDataJSON != null)
//            {
//                // Khởi tạo grid 2D
//                for (int y = 0; y < gridHeight; y++)
//                {
//                    tilesData.Add(new List<TileData>());
//                    for (int x = 0; x < gridWidth; x++)
//                    {
//                        tilesData[y].Add(new TileData("empty")); // Mặc định là empty
//                    }
//                }

//                // Đặt ground tiles
//                if (levelDataJSON.Tiles != null)
//                {
//                    foreach (var tile in levelDataJSON.Tiles)
//                    {
//                        if (tile.TileType == groundTileType)
//                        {
//                            int x = tile.TileIndex % gridWidth;
//                            int y = tile.TileIndex / gridWidth;
//                            if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
//                            {
//                                tilesData[y][x] = new TileData("ground");
//                            }
//                        }
//                    }
//                }

//                // Đặt blocks
//                if (levelDataJSON.Blocks != null)
//                {
//                    foreach (var block in levelDataJSON.Blocks)
//                    {
//                        int x = block.PosIndex % gridWidth;
//                        int y = block.PosIndex / gridWidth;
//                        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight && blockShapeToId.ContainsKey(block.Shape))
//                        {
//                            tilesData[y][x] = new TileData("block", blockShapeToId[block.Shape]);
//                        }
//                    }
//                }
//            }
//            else
//            {
//                Debug.LogError("Không thể deserialize file JSON.");
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.LogError($"Lỗi khi đọc file JSON: {e.Message}");
//        }

//        return tilesData;
//    }
//}