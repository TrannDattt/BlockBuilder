using BuilderTool.Enums;
using BuilderTool.FileConvert;
using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static BuilderTool.FileConvert.FileManager;

namespace BuilderTool.UIElement
{
    [Serializable]
    public class ShapePrefabMapping
    {
        public EShape shape;
        public GameObject prefab;
    }

    public class LevelManagerMenu : Singleton<LevelManagerMenu>
    {
        [SerializeField] private Button _testBtn;
        [SerializeField] private Button _designBtn;

        [SerializeField] private TMP_InputField _levelIdInput;
        [SerializeField] private TMP_InputField _timeInput;
        [SerializeField] private DifficultyDropdown _difficultiDropdown;

        [SerializeField] private Button _testSavedLevelBtn;
        [SerializeField] private Button _clearFieldBtn;
        [SerializeField] private Button _saveLevelBtn;
        [SerializeField] private Button _loadLevelBtn;
        [SerializeField] private Button _deleteLevelBtn;

        [SerializeField] private Grid3D _editorGrid;

        [SerializeField] private Vector3 _grid3DOrigin = Vector3.zero;
        [SerializeField] private float _grid3DCellSize = 1f;

        public string LevelId => _levelIdInput.text;
        public int LevelTime => int.TryParse(_timeInput.text, out int result) ? result : -1;
        public EDifficulty LevelDifficulty => (EDifficulty)_difficultiDropdown.value;

        public bool CheckLevelInfo()
        {
            return LevelId != "" && LevelTime >= 0;
        }

        public void UpdateLevelInfo(LevelInfoData levelInfoData)
        {
            _levelIdInput.text = levelInfoData.Id;
            _timeInput.text = levelInfoData.Time.ToString();
            _difficultiDropdown.value = (int)levelInfoData.Difficulty;
        }

        private void ClearBoard()
        {
            var blockList = EditorField.Instance.BlockDict.Keys.ToArray();
            BlockPlacement.Instance.RemoveBlock(blockList);
            EditorField.Instance.CreateNewField();
        }

        private void SaveLevel()
        {
            FileManager.SaveJsonFile();
        }

        private void LoadLevel()
        {
            ClearBoard();
            FileManager.LoadSavedFile();
        }

        private void TestLevel()
        {
            if (LevelDataHolder.Instance == null)
            {
                Debug.LogError("LevelDataHolder không tồn tại.");
                return;
            }

            if (EditorField.Instance == null || EditorField.Instance.Tiles == null || EditorField.Instance.Tiles.Count == 0)
            {
                Debug.LogError("EditorField chưa được khởi tạo hoặc không có tile nào.");
                return;
            }

            int width = EditorField.FIELD_WIDTH;
            int height = EditorField.FIELD_HEIGHT;
            List<TileData> allTileData = new List<TileData>();

            //int minX = width, maxX = 0;
            //int minY = height, maxY = 0;

            for (int i = 0; i < EditorField.Instance.Tiles.Count; i++)
            {
                EditorTile editorTile = EditorField.Instance.Tiles[i];
                string type = "empty"; 

                if (editorTile != null)
                {
                    if (editorTile.TileType == ETileType.Ground)
                    {
                        type = "ground";
                        
                    }
                    else if (editorTile.TileType == ETileType.Door)
                    {
                        type = "door";
                        
                    }
                    else if (editorTile.TileType == ETileType.BlockNode)
                    {
                        type = "block";
                        break;
                        //foreach (Transform child in editorTile.transform)
                        //{
                            
                        ////    //if (child.name.StartsWith("Block_"))
                        ////    //{
                        ////    //    blockId = child.gameObject.name.Replace("Block_", "");
                        ////    //    break;
                        ////    //}
                        ////}
                        ////Debug.Log($"TestLevel: Tìm thấy BlockNode tại index= {i}");
                    }
                    //else if (editorTile.TileType == ETileType.Empty)
                    //{
                    //    type = "empty";
                    //    //Debug.Log($"TestLevel: Tìm thấy Empty tại index= {i}");
                    //}                    
                }
                
                int posX = i % width;
                int posY = -i / width;

                Debug.Log($"TestLevel: Tìm thấy {type} tại Index = {i}, Grid Position = {posX}, {posY}");

                //Vector3 worldPos = editorTile.transform.position;
                //int posX = Mathf.RoundToInt(worldPos.x);
                //int posY = Mathf.RoundToInt(worldPos.y);

                //Debug.Log($"TestLevel: Tìm thấy {type} tại worldPos=({posX}, {posY}), Index = {i}");

                allTileData.Add(new TileData(posX, posY, type));
            }
            LevelDataHolder.Instance.levelData = allTileData;
            LevelDataHolder.Instance.GridWidth = width;
            LevelDataHolder.Instance.GridHeight = height;

            LevelDataHolder.Instance.Block3DList.Clear();
            List<Block3DData> block3DList = new List<Block3DData>();

            foreach (var kvp in EditorField.Instance.BlockDict)
            {
                EditorBlock block = kvp.Key;
                EditorTile tile = kvp.Value;

                int tileIndex = EditorField.Instance.Tiles.IndexOf(tile);
                int x = tileIndex % width;
                int y = tileIndex / width; 
                
                //Vector3 worldPos = tile.transform.position;

                //int x = Mathf.RoundToInt(worldPos.x);
                //int y = Mathf.RoundToInt(worldPos.y);

                EShape shape = block.Shape;
                Vector3 position = new Vector3(x, y, 0);
                //Vector3 position = new Vector3(posX, posY, 0);
                LevelDataHolder.Instance.Block3DList.Add(new Block3DData(shape,position));
                Block3DData data = new Block3DData(block.Shape, position);
                block3DList.Add(data);
                Debug.Log($"[LevelManagerMenu] Block {block.Shape} at position ({position.x}, {position.y}, {position.z})");
            }

            LevelDataHolder.Instance.SetBlock3DList(block3DList);
            //foreach (var block in LevelDataHolder.Instance.Block3DList)
            //{
            //    Debug.Log($"[LevelManagerMenu] Block {block.shape} at position {block.position}");
            //}

            SceneManager.LoadSceneAsync("SceneTester");
        }

        private void DesignLevel()
        {
            SceneManager.LoadSceneAsync("SceneEditor");
        }

        private void Start()
        {
            _testBtn.onClick.AddListener(TestLevel);
            _designBtn.onClick.AddListener(DesignLevel);

            _clearFieldBtn.onClick.AddListener(ClearBoard);
            _saveLevelBtn.onClick.AddListener(SaveLevel);
            _loadLevelBtn.onClick.AddListener(LoadLevel);
        }
    }
}