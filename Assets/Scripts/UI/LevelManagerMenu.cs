using BuilderTool.Enums;
using BuilderTool.FileConvert;
using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BuilderTool.FileConvert.FileManager;

namespace BuilderTool.UIElement
{
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
