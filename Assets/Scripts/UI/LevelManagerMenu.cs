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

        private void OnClearButtonClicked()
        {
            var blockList = EditorField.Instance.BlockDict.Keys.ToArray();
            BlockPlacement.Instance.RemoveBlock(blockList);
            EditorField.Instance.CreateNewField();
        }

        private void OnSaveButtonClicked()
        {
            FileManager.ConvertLevelToJson();
        }

        private void OnTestButtonClicked()
        {
            SceneManager.LoadSceneAsync("SceneTester");
        }

        private void OnDesignButtonClicked()
        {
            SceneManager.LoadSceneAsync("SceneEditor");
        }

        private void Start()
        {
            _testBtn.onClick.AddListener(OnTestButtonClicked);
            _designBtn.onClick.AddListener(OnDesignButtonClicked);

            _clearFieldBtn.onClick.AddListener(OnClearButtonClicked);
            _saveLevelBtn.onClick.AddListener(OnSaveButtonClicked);
        }
    }
}
