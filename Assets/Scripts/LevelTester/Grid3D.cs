using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement; // Để kiểm tra tên scene

public class Grid3D : MonoBehaviour
{
    public int XGrid { get; set; } // Kích thước lưới X (Width) - Để script khác có thể set hoặc được load
    public int YGrid { get; set; } // Kích thước lưới Y (Height) - Để script khác có thể set hoặc được load
    public float GridSpaceSize = 1f; // Khoảng cách giữa các ô grid
    public float BlockHeight = 0.5f; // Chiều cao của block so với ground

    [SerializeField] private GameObject tilePrefab; // Prefab Tile chứa Ground, Door, Block
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

    // Tên của scene chỉnh sửa level (cần được thiết lập trong Inspector hoặc code)
    [SerializeField] private string editorSceneName = "SceneEditor"; // Thay "SceneEditor" bằng tên scene chỉnh sửa của bạn
    // Tên của scene test level (cần được thiết lập trong Inspector hoặc code)
    [SerializeField] private string testSceneName = "SceneTester"; // Thay "SceneTester" bằng tên scene test của bạn

    private void Awake()
    {
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
        // Reset mảng grid
        grid3D = null;
        XGrid = 0;
        YGrid = 0;
    }

    void Start()
    {
        // Kiểm tra tên của scene hiện tại
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == testSceneName)
        {
            // Nếu là scene test, load level từ LevelDataHolder
            LoadLevelFromDataHolder();
        }
        else if (currentSceneName == editorSceneName)
        {
            Debug.Log("Grid3D đang chạy trong scene chỉnh sửa.");
            XGrid = 15; // Thiết lập chiều rộng là 15
            YGrid = 15; // Thiết lập chiều cao là 15
            grid3D = new GameObject[XGrid, 2, YGrid];
            Debug.Log($"Grid3D trong Editor: grid3D đã được khởi tạo với kích thước {XGrid}x{YGrid}");
        }
        else
        {
            Debug.LogWarning($"Grid3D đang chạy trong scene không xác định: {currentSceneName}");
        }
    }

    public void LoadLevelFromDataHolder()
    {
        Debug.Log("--- Bắt đầu LoadLevelFromDataHolder ---");

        LevelDataHolder dataHolder = FindObjectOfType<LevelDataHolder>();
        if (dataHolder == null)
        {
            Debug.LogError("Không tìm thấy LevelDataHolder trong scene!");
            Debug.Log("--- Kết thúc LoadLevelFromDataHolder ---");
            return;
        }

        XGrid = dataHolder.GridWidth;
        YGrid = dataHolder.GridHeight;
        List<TileData> levelData = dataHolder.levelData;

        Debug.Log($"Kích thước grid lấy từ LevelDataHolder: Width={XGrid}, Height={YGrid}");

        if (XGrid <= 0 || YGrid <= 0)
        {
            Debug.LogError("Kích thước grid không hợp lệ.");
            Debug.Log("--- Kết thúc LoadLevelFromDataHolder ---");
            return;
        }

        grid3D = new GameObject[XGrid, 2, YGrid];

        if (gridParent != null)
        {
            transform.SetParent(gridParent, false);
            Debug.Log($"Đã đặt parent của Grid3D thành: {gridParent.name}");
        }
        else
        {
            Debug.Log("Không có Grid Parent được gán.");
        }

        if (tilePrefab == null)
        {
            Debug.LogError("Tile prefab chưa được gán trong Inspector!");
            Debug.Log("--- Kết thúc LoadLevelFromDataHolder ---");
            return;
        }
        Debug.Log($"Tile prefab được sử dụng: {tilePrefab.name}");

        for (int i = 0; i < levelData.Count; i++)
        {
            TileData data = levelData[i];

            if (data.tileType != "empty") // Chỉ tạo GameObject nếu không phải là empty
            {
                Vector3 worldPos = GetWorldPosition(data.x, data.y);
                GameObject newTile = Instantiate(tilePrefab, worldPos, Quaternion.identity);
                newTile.transform.SetParent(transform, true);
                newTile.name = $"Tile_X{data.x}_Y{data.y}";

                Transform groundChild = newTile.transform.Find("Ground");
                Transform doorChild = newTile.transform.Find("Door");
                Transform blockChild = newTile.transform.Find("Block");

                if (data.tileType == "ground" && groundChild != null)
                {
                    groundChild.gameObject.SetActive(true);
                    grid3D[data.x, 0, data.y] = newTile;
                    //Debug.Log($"Đã tạo Ground tile tại grid: x={data.x}, y={data.y}, world position={worldPos}");
                }
                else if (data.tileType == "door" && doorChild != null)
                {
                    doorChild.gameObject.SetActive(true);
                    grid3D[data.x, 0, data.y] = newTile;
                    //Debug.Log($"Đã tạo Door tile tại grid: x={data.x}, y={data.y}, world position={worldPos}");
                }
                else if (data.tileType == "block" && blockChild != null)
                {
                    blockChild.gameObject.SetActive(true);
                    grid3D[data.x, 0, data.y] = newTile; 
                    grid3D[data.x, 1, data.y] = newTile; 
                    //Debug.Log($"Đã tạo Block '{data.blockId}' tại grid: x={data.x}, y={data.y}, world position={worldPos}");
                }
            }
            //else
            //{
            //    //Debug.Log($"Ô grid x={data.x}, y={data.y} là empty.");
            //}
        }

        Debug.Log("--- Hoàn tất LoadLevelFromDataHolder ---");
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

    //// Phương thức lấy GameObject tại một vị trí cụ thể
    //public GameObject GetTileAt(int x, int y, int z)
    //{
    //    if (x >= 0 && x < XGrid && y >= 0 && y < 2 && z >= 0 && z < YGrid)
    //    {
    //        return grid3D[x, y, z];
    //    }
    //    return null;
    //}

    //// Phương thức lấy block prefab theo ID
    //public GameObject GetBlockPrefab(string blockId)
    //{
    //    if (blockPrefabs.ContainsKey(blockId))
    //    {
    //        return blockPrefabs[blockId];
    //    }
    //    return null;
    //}

    // Phương thức lấy tọa độ thế giới từ tọa độ grid (cho 2D XY)
    public Vector3 GetWorldPosition(int x, int y, bool isBlock = false)
    {
        float z = 0f; 
        float yPos = isBlock ? BlockHeight : 0f; // Vẫn giữ BlockHeight nếu bạn muốn block có độ cao
        return new Vector3((x + (grid3D != null ? grid3D.GetLength(0) > 0 ? 0 : 0 : 0)) * GridSpaceSize, (y + (grid3D != null ? grid3D.GetLength(2) > 0 ? 0 : 0 : 0)) * GridSpaceSize * -1f + yPos, z);
    }
        
    // Thêm event để thông báo khi grid thay đổi
    public delegate void GridChangedHandler();
    public event GridChangedHandler OnGridChanged;

    //// Thêm phương thức để đặt block mới
    //public void PlaceBlock(int x, int y, string blockId)
    //{
    //    if (x < 0 || x >= XGrid || y < 0 || y >= YGrid)
    //    {
    //        Debug.LogWarning($"Vị trí ({x}, {y}) nằm ngoài grid!");
    //        return;
    //    }

    //    // Kiểm tra xem có ground ở vị trí này không
    //    if (grid3D[x, 0, y] == null)
    //    {
    //        // Tạo ground nếu chưa có
    //        Vector3 groundPosition = GetWorldPosition(x, y);
    //        if (tilePrefab != null)
    //        {
    //            GameObject newTile = Instantiate(tilePrefab, groundPosition, Quaternion.identity);
    //            newTile.transform.SetParent(transform, false);
    //            newTile.name = $"Tile_X{x}_Y{y}";
    //            Transform groundChild = newTile.transform.Find("Ground");
    //            if (groundChild != null) groundChild.gameObject.SetActive(true);
    //            grid3D[x, 0, y] = newTile;
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Tile prefab chưa được gán!");
    //            return;
    //        }
    //    }

    //    // Xóa block cũ nếu có
    //    if (grid3D[x, 1, y] != null)
    //    {
    //        Destroy(grid3D[x, 1, y]);
    //    }

    //    // Tạo block mới
    //    if (blockId != null)
    //    {
    //        if (blockPrefabs.ContainsKey(blockId))
    //        {
    //            Vector3 blockPosition = GetWorldPosition(x, y, true);
    //            GameObject newBlock = Instantiate(blockPrefabs[blockId], blockPosition, Quaternion.identity);
    //            newBlock.transform.SetParent(transform, false);
    //            newBlock.name = $"Block_{blockId}_X{x}_Y{y}";
    //            grid3D[x, 1, y] = newBlock;
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Không tìm thấy prefab cho BlockId: {blockId}");
    //        }
    //    }

    //    // Thông báo grid đã thay đổi
    //    OnGridChanged?.Invoke();
    //}

    //// Phương thức để xóa block
    //public void RemoveBlock(int x, int y)
    //{
    //    if (x < 0 || x >= XGrid || y < 0 || y >= YGrid)
    //    {
    //        return;
    //    }

    //    if (grid3D[x, 1, y] != null)
    //    {
    //        Destroy(grid3D[x, 1, y]);
    //        grid3D[x, 1, y] = null;

    //        // Thông báo grid đã thay đổi
    //        OnGridChanged?.Invoke();
    //    }
    //}

    //// Phương thức để xóa ground và block
    //public void RemoveTile(int x, int y)
    //{
    //    if (x < 0 || x >= XGrid || y < 0 || y >= YGrid)
    //    {
    //        return;
    //    }

    //    // Xóa block trước nếu có
    //    if (grid3D[x, 1, y] != null)
    //    {
    //        Destroy(grid3D[x, 1, y]);
    //        grid3D[x, 1, y] = null;
    //    }

    //    // Xóa ground
    //    if (grid3D[x, 0, y] != null)
    //    {
    //        Destroy(grid3D[x, 0, y]);
    //        grid3D[x, 0, y] = null;
    //    }

    //    // Thông báo grid đã thay đổi
    //    OnGridChanged?.Invoke();
    //}
}