using UnityEngine;
using System.Collections.Generic;
using BuilderTool.Enums;

public class BlockSpawner3D : MonoBehaviour
{
    [Header("Prefab cho từng loại Shape")]
    [SerializeField] private List<Block3DPrefabEntry> blockPrefabs;

    [SerializeField] private int gridHeight = 15;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector3 gridOrigin = Vector3.zero;

    private Dictionary<EShape, GameObject> prefabDict;

    private void Awake()
    {
        // Tạo dictionary để dễ truy cập
        prefabDict = new Dictionary<EShape, GameObject>();
        foreach (var entry in blockPrefabs)
        {
            if (!prefabDict.ContainsKey(entry.shape))
            {
                prefabDict.Add(entry.shape, entry.prefab);
            }
        }
    }

    private void Start()
    {
        if (LevelDataHolder.Instance == null || LevelDataHolder.Instance.Block3DList == null)
        {
            Debug.LogWarning("Không có dữ liệu block để spawn.");
            return;
        }
        Debug.Log($"[BlockSpawner3D] Block3DList.Count = {LevelDataHolder.Instance.Block3DList.Count}");
        SpawnAllBlocks();
    }

    private void SpawnAllBlocks()
    {
        int width = LevelDataHolder.Instance.GridWidth;
        int height = LevelDataHolder.Instance.GridHeight;

        if (width <= 0 || height <= 0)
        {
            Debug.LogError($"Invalid grid dimensions: {width}x{height}");
            return;
        }

        Debug.Log($"Spawning blocks on grid {width}x{height}");

        foreach (Block3DData data in LevelDataHolder.Instance.Block3DList)
        {
            SpawnSingleBlock(data);
        }
    }

    private void SpawnSingleBlock(Block3DData data)
    {
        if (!prefabDict.TryGetValue(data.shape, out GameObject prefab))
        {
            Debug.LogWarning($"Không tìm thấy prefab cho shape: {data.shape}");
            return;
        }

        float spawnX = (data.position.x * cellSize);
        float spawnY = (data.position.y * cellSize);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject spawnedBlock = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
        spawnedBlock.name = $"Block3D_{data.shape}_{data.position.x}_{data.position.y}";

        Debug.Log($"Đã spawn block 3D {data.shape} tại {spawnPosition} từ tọa độ grid {data.position}");
    }
}

[System.Serializable]
public class Block3DPrefabEntry
{
    public EShape shape;
    public GameObject prefab;
}
