//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//namespace BuilderTool.Test
//{
//    public class TestCurLevel : MonoBehaviour
//    {
//        private const string LEVEL_TESTER_SCENE = "LevelTester";
//        public Grid3D editorGrid; // Kéo instance Grid3D từ scene chỉnh sửa vào đây

//        public void TestLevel()
//        {
//            if (editorGrid == null || LevelDataHolder.Instance == null)
//            {
//                Debug.LogError("Chưa gán Grid3D hoặc LevelDataHolder không tồn tại.");
//                return;
//            }

//            int width = editorGrid.XGrid;
//            int height = editorGrid.YGrid;
//            List<TileData> data = new List<TileData>();

//            for (int x = 0; x < width; x++)
//            {
//                for (int y = 0; y < height; y++)
//                {
//                    GameObject tile = editorGrid.GetTileAt(x, 0, y); // Lấy tile ở layer ground
//                    if (tile != null)
//                    {
//                        string type = "empty";
//                        string blockId = "";

//                        if (tile.transform.Find("Ground")?.gameObject.activeSelf == true)
//                        {
//                            type = "ground";
//                        }
//                        else if (tile.transform.Find("Door")?.gameObject.activeSelf == true)
//                        {
//                            type = "door";
//                        }
//                        else if (tile.transform.Find("Block")?.gameObject.activeSelf == true)
//                        {
//                            type = "block";
//                            blockId = tile.transform.Find("Block").gameObject.name.Replace("Block_", "");
//                        }
//                        data.Add(new TileData(x, y, type, blockId));
//                    }
//                }
//            }

//            LevelDataHolder.Instance.SetLevelData(width, height, data);
//            Debug.Log("Dữ liệu level đã được lưu vào LevelDataHolder từ TestCurLevel.");

//            SceneManager.LoadScene(LEVEL_TESTER_SCENE);
//        }
//    }
//}