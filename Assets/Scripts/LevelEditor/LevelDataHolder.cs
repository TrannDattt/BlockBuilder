using UnityEngine;
using System.Collections.Generic;
using BuilderTool.Enums;

[System.Serializable]
public class TileData
{
    public int x;
    public int y;
    public string tileType; // "ground", "door", "block", "empty"

    public TileData(int x, int y, string type)
    {
        this.x = x;
        this.y = y;
        this.tileType = type;
    }
}

[System.Serializable]
public class Block3DData
{
    public EShape shape;
    public Vector3 position;

    public Block3DData(EShape shape, Vector3 position)
    {
        this.shape = shape;
        this.position = position;
    }
}

public class LevelDataHolder : MonoBehaviour
{
    public static LevelDataHolder Instance { get; private set; }

    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public List<TileData> levelData = new List<TileData>();

    public List<Block3DData> Block3DList { get; set; } = new List<Block3DData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ GameObject này khi load scene mới
        }
        else
        {
            Destroy(gameObject); // Hủy GameObject trùng lặp nếu có
        }
    }

    public void SetLevelData(int width, int height, List<TileData> data)
    {
        GridWidth = width;
        GridHeight = height;
        levelData = data;
    }

    public TileData GetTileData(int x, int y)
    {
        return levelData.Find(data => data.x == x && data.y == y);
    }

    public void SetBlock3DList(List<Block3DData> blockList)
    {
        Block3DList = blockList;
    }
}