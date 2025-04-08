using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid3D : MonoBehaviour
{
    public int XGrid { get; private set; } // Kích thước lưới X (được xác định từ dữ liệu)
    public int YGrid { get; private set; } // Kích thước lưới Y (được xác định từ dữ liệu)
    public float GridSpaceSize = 1f; // Khoảng cách giữa các ô grid
    public float BlockHeight = 0.5f; // Chiều cao của block so với ground

    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private Transform gridParent;

    [Serializable]
    public class BlockPrefabPair
    {
        public string blockId;
        public GameObject prefab;
    }

    [SerializeField] private List<BlockPrefabPair> blockPrefabsList = new List<BlockPrefabPair>();
    private Dictionary<string, GameObject> blockPrefabs = new Dictionary<string, GameObject>();

    private GameObject[,,] grid3D; // Mảng 3D lưu trữ các gameObject trong grid

    private void Awake()
    {
        // Chuyển đổi danh sách sang dictionary để dễ truy cập
        InitializeBlockPrefabs();
    }

    private void InitializeBlockPrefabs()
    {
        blockPrefabs.Clear();
        foreach (var pair in blockPrefabsList)
        {
            if (!string.IsNullOrEmpty(pair.blockId) && pair.prefab != null && !blockPrefabs.ContainsKey(pair.blockId))
            {
                blockPrefabs.Add(pair.blockId, pair.prefab);
            }
        }
    }

    public void ClearGrid()
    {
        // Xóa tất cả các gameObject con hiện có
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void LoadLevelFromData(List<List<LevelLoader3D.TileData>> tiles)
    {
        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogError("Dữ liệu Tiles không hợp lệ.");
            return;
        }

        // Đảm bảo dictionary đã được khởi tạo
        if (blockPrefabs.Count == 0)
        {
            InitializeBlockPrefabs();
        }

        // Xóa grid hiện tại nếu có
        ClearGrid();

        YGrid = tiles.Count;
        XGrid = tiles[0].Count;
        grid3D = new GameObject[XGrid, 2, YGrid]; // [x, y, z] với y=0 là ground, y=1 là block

        if (gridParent != null)
        {
            transform.SetParent(gridParent, false);
        }

        // Tạo ground trước
        for (int z = 0; z < YGrid; z++) // z tương ứng với y trong dữ liệu 2D
        {
            for (int x = 0; x < XGrid; x++) // x vẫn là x
            {
                LevelLoader3D.TileData tile = tiles[z][x];
                Vector3 groundPosition = new Vector3(x * GridSpaceSize, 0, -z * GridSpaceSize);

                if (tile.Type.ToLower() == "ground" || tile.Type.ToLower() == "block")
                {
                    if (groundPrefab != null)
                    {
                        GameObject newGround = Instantiate(groundPrefab, groundPosition, Quaternion.identity);
                        newGround.transform.SetParent(transform, false);
                        newGround.name = $"Ground_X{x}_Z{z}";
                        grid3D[x, 0, z] = newGround;
                    }
                    else
                    {
                        Debug.LogWarning("Ground prefab chưa được gán!");
                    }
                }
            }
        }

        // Sau đó tạo các block
        for (int z = 0; z < YGrid; z++)
        {
            for (int x = 0; x < XGrid; x++)
            {
                LevelLoader3D.TileData tile = tiles[z][x];
                Vector3 blockPosition = new Vector3(x * GridSpaceSize, BlockHeight, -z * GridSpaceSize);

                if (tile.Type.ToLower() == "block")
                {
                    if (!string.IsNullOrEmpty(tile.BlockId) && blockPrefabs.ContainsKey(tile.BlockId))
                    {
                        GameObject newBlock = Instantiate(blockPrefabs[tile.BlockId], blockPosition, Quaternion.identity);
                        newBlock.transform.SetParent(transform, false);
                        newBlock.name = $"Block_{tile.BlockId}_X{x}_Z{z}";
                        grid3D[x, 1, z] = newBlock;
                    }
                    else if (blockPrefabs.ContainsKey("default"))
                    {
                        // Sử dụng block mặc định nếu không tìm thấy block cụ thể
                        GameObject newBlock = Instantiate(blockPrefabs["default"], blockPosition, Quaternion.identity);
                        newBlock.transform.SetParent(transform, false);
                        newBlock.name = $"Block_default_X{x}_Z{z}";
                        grid3D[x, 1, z] = newBlock;
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy prefab cho BlockId: {tile.BlockId}");
                    }
                }
            }
        }
    }

    // Phương thức thêm block prefab vào từ điển
    public void AddBlockPrefab(string blockId, GameObject prefab)
    {
        if (!blockPrefabs.ContainsKey(blockId))
        {
            blockPrefabs.Add(blockId, prefab);

            // Cập nhật cả danh sách để hiển thị trong Inspector
            BlockPrefabPair newPair = new BlockPrefabPair { blockId = blockId, prefab = prefab };
            blockPrefabsList.Add(newPair);
        }
    }

    // Phương thức lấy GameObject tại một vị trí cụ thể
    public GameObject GetTileAt(int x, int y, int z)
    {
        if (x >= 0 && x < XGrid && y >= 0 && y < 2 && z >= 0 && z < YGrid)
        {
            return grid3D[x, y, z];
        }
        return null;
    }

    // Phương thức lấy block prefab theo ID
    public GameObject GetBlockPrefab(string blockId)
    {
        if (blockPrefabs.ContainsKey(blockId))
        {
            return blockPrefabs[blockId];
        }
        return null;
    }

    // Phương thức lấy tọa độ thế giới từ tọa độ grid
    public Vector3 GetWorldPosition(int x, int z, bool isBlock = false)
    {
        float y = isBlock ? BlockHeight : 0f;
        return new Vector3(x * GridSpaceSize, y, -z * GridSpaceSize);
    }

    // Thêm event để thông báo khi grid thay đổi
    public delegate void GridChangedHandler();
    public event GridChangedHandler OnGridChanged;

    // Thêm phương thức để đặt block mới
    public void PlaceBlock(int x, int z, string blockId)
    {
        if (x < 0 || x >= XGrid || z < 0 || z >= YGrid)
        {
            Debug.LogWarning($"Vị trí ({x}, {z}) nằm ngoài grid!");
            return;
        }

        // Kiểm tra xem có ground ở vị trí này không
        if (grid3D[x, 0, z] == null)
        {
            // Tạo ground nếu chưa có
            Vector3 groundPosition = GetWorldPosition(x, z);
            if (groundPrefab != null)
            {
                GameObject newGround = Instantiate(groundPrefab, groundPosition, Quaternion.identity);
                newGround.transform.SetParent(transform, false);
                newGround.name = $"Ground_X{x}_Z{z}";
                grid3D[x, 0, z] = newGround;
            }
            else
            {
                Debug.LogWarning("Ground prefab chưa được gán!");
                return;
            }
        }

        // Xóa block cũ nếu có
        if (grid3D[x, 1, z] != null)
        {
            Destroy(grid3D[x, 1, z]);
        }

        // Tạo block mới
        if (blockId != null)
        {
            if (blockPrefabs.ContainsKey(blockId))
            {
                Vector3 blockPosition = GetWorldPosition(x, z, true);
                GameObject newBlock = Instantiate(blockPrefabs[blockId], blockPosition, Quaternion.identity);
                newBlock.transform.SetParent(transform, false);
                newBlock.name = $"Block_{blockId}_X{x}_Z{z}";
                grid3D[x, 1, z] = newBlock;
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy prefab cho BlockId: {blockId}");
            }
        }

        // Thông báo grid đã thay đổi
        OnGridChanged?.Invoke();
    }

    // Phương thức để xóa block
    public void RemoveBlock(int x, int z)
    {
        if (x < 0 || x >= XGrid || z < 0 || z >= YGrid)
        {
            return;
        }

        if (grid3D[x, 1, z] != null)
        {
            Destroy(grid3D[x, 1, z]);
            grid3D[x, 1, z] = null;

            // Thông báo grid đã thay đổi
            OnGridChanged?.Invoke();
        }
    }

    // Phương thức để xóa ground và block
    public void RemoveTile(int x, int z)
    {
        if (x < 0 || x >= XGrid || z < 0 || z >= YGrid)
        {
            return;
        }

        // Xóa block trước nếu có
        if (grid3D[x, 1, z] != null)
        {
            Destroy(grid3D[x, 1, z]);
            grid3D[x, 1, z] = null;
        }

        // Xóa ground
        if (grid3D[x, 0, z] != null)
        {
            Destroy(grid3D[x, 0, z]);
            grid3D[x, 0, z] = null;
        }

        // Thông báo grid đã thay đổi
        OnGridChanged?.Invoke();
    }

    // Phương thức để lấy dữ liệu hiện tại của grid
    public List<List<LevelLoader3D.TileData>> GetCurrentGridData()
    {
        List<List<LevelLoader3D.TileData>> gridData = new List<List<LevelLoader3D.TileData>>();

        for (int z = 0; z < YGrid; z++)
        {
            List<LevelLoader3D.TileData> row = new List<LevelLoader3D.TileData>();

            for (int x = 0; x < XGrid; x++)
            {
                // Mặc định là empty
                string tileType = "empty";
                string blockId = "";

                // Kiểm tra ground
                if (grid3D[x, 0, z] != null)
                {
                    tileType = "ground";

                    // Kiểm tra block
                    if (grid3D[x, 1, z] != null)
                    {
                        string name = grid3D[x, 1, z].name;
                        // Trích xuất blockId từ tên
                        if (name.StartsWith("Block_"))
                        {
                            int start = "Block_".Length;
                            int end = name.IndexOf("_X");
                            if (end > start)
                            {
                                blockId = name.Substring(start, end - start);
                                tileType = "block";
                            }
                        }
                    }
                }

                row.Add(new LevelLoader3D.TileData(tileType, blockId));
            }

            gridData.Add(row);
        }

        return gridData;
    }
}