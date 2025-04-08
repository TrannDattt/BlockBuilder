using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class LevelLoader3D : MonoBehaviour
{
    [System.Serializable]
    public class TileData
    {
        public string Type; // "empty", "ground", "block"
        public string BlockId; // Định danh của block (nếu có)

        public TileData(string type, string blockId = "")
        {
            Type = type;
            BlockId = blockId;
        }
    }

    [SerializeField] private string levelFilePath = "Assets/Resources/Levels/level1.txt";
    [SerializeField] private Grid3D grid3DComponent;

    public void LoadLevel()
    {
        if (grid3DComponent == null)
        {
            grid3DComponent = GetComponent<Grid3D>();
            if (grid3DComponent == null)
            {
                grid3DComponent = gameObject.AddComponent<Grid3D>();
            }
        }

        List<List<TileData>> levelData = ReadLevelDataFromFile(levelFilePath);
        grid3DComponent.LoadLevelFromData(levelData);
    }

    public void LoadLevelFromPath(string path)
    {
        levelFilePath = path;
        LoadLevel();
    }

    public List<List<TileData>> ReadLevelDataFromFile(string filePath)
    {
        List<List<TileData>> tilesData = new List<List<TileData>>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                List<TileData> rowData = new List<TileData>();
                string[] tiles = line.Split(',');

                foreach (string tile in tiles)
                {
                    string trimmedTile = tile.Trim();
                    
                    if (string.IsNullOrEmpty(trimmedTile) || trimmedTile.ToLower() == "empty")
                    {
                        rowData.Add(new TileData("empty"));
                    }
                    else if (trimmedTile.ToLower() == "ground")
                    {
                        rowData.Add(new TileData("ground"));
                    }
                    else if (trimmedTile.StartsWith("block", StringComparison.OrdinalIgnoreCase))
                    {
                        // Định dạng dự kiến: "block:id" hoặc chỉ "block"
                        string[] parts = trimmedTile.Split(':');
                        string blockId = parts.Length > 1 ? parts[1] : "default";
                        rowData.Add(new TileData("block", blockId));
                    }
                    else
                    {
                        // Trường hợp khác, xem như empty
                        rowData.Add(new TileData("empty"));
                    }
                }

                tilesData.Add(rowData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Lỗi khi đọc file: {e.Message}");
        }

        return tilesData;
    }

    // Phương thức này để tải level từ một chuỗi văn bản (hữu ích khi tải từ Resources hoặc asset bundle)
    public List<List<TileData>> ReadLevelDataFromText(string levelText)
    {
        List<List<TileData>> tilesData = new List<List<TileData>>();

        try
        {
            string[] lines = levelText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                List<TileData> rowData = new List<TileData>();
                string[] tiles = line.Split(',');

                foreach (string tile in tiles)
                {
                    string trimmedTile = tile.Trim();
                    
                    if (string.IsNullOrEmpty(trimmedTile) || trimmedTile.ToLower() == "empty")
                    {
                        rowData.Add(new TileData("empty"));
                    }
                    else if (trimmedTile.ToLower() == "ground")
                    {
                        rowData.Add(new TileData("ground"));
                    }
                    else if (trimmedTile.StartsWith("block", StringComparison.OrdinalIgnoreCase))
                    {
                        // Định dạng dự kiến: "block:id" hoặc chỉ "block"
                        string[] parts = trimmedTile.Split(':');
                        string blockId = parts.Length > 1 ? parts[1] : "default";
                        rowData.Add(new TileData("block", blockId));
                    }
                    else
                    {
                        // Trường hợp khác, xem như empty
                        rowData.Add(new TileData("empty"));
                    }
                }

                tilesData.Add(rowData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Lỗi khi phân tích dữ liệu: {e.Message}");
        }

        return tilesData;
    }
}