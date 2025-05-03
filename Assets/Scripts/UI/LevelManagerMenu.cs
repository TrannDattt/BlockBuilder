using BuilderTool.Enums;
using BuilderTool.FileConvert;
using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using System.Linq;
using System.Threading.Tasks;
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

        private Scene _testerScene;
        private Scene _editorScene;

        public bool CheckLevelInfo()
        {
            return LevelId != "" && LevelTime >= 0;
        }

        public void UpdateLevelInfo(FileManager.LevelInfoData levelInfoData)
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

        private async void TestLevel()
        {
            TileAttributeSelector.Instance.CloseMenu();
            BlockAttributeSelector.Instance.CloseMenu();
            BlockDisplayShelf.Instance.CloseShelf();

            await DisableScene(SceneManager.GetSceneByName("SceneEditor"));
            AsyncOperation loadSceneTask = SceneManager.LoadSceneAsync("SceneTester", LoadSceneMode.Additive);

            while(!loadSceneTask.isDone){
                await Task.Yield();
            }

            Grid3D.Instance.Geneate3DGrid();
        }

        private async void DesignLevel()
        {
            SceneManager.UnloadSceneAsync("SceneTester");
            await EnableScene(SceneManager.GetSceneByName("SceneEditor"));

            BlockDisplayShelf.Instance.OpenShelf();
        }

        private async Task EnableScene(Scene scene)
        {
            GameObject[] objects = scene.GetRootGameObjects();

            foreach (var obj in objects)
            {
                obj.SetActive(true);
            }
            
            await Task.Yield();
        }

        private async Task DisableScene(Scene scene){
            GameObject[] objects = scene.GetRootGameObjects();

            foreach (var obj in objects)
            {
                obj.SetActive(false);
            }

            await Task.Yield();
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