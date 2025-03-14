using BuilderTool.Editor;
using BuilderTool.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuilderTool.LevelEditor
{
    public class BlockDisplayShelf : MonoBehaviour
    {
        [SerializeField] private GameObject _displayShelf;

        [SerializeField] private DisplayedItem _crossBlock;
        [SerializeField] private DisplayedItem _squareBlock;
        [SerializeField] private DisplayedItem _lShapeBlock;

        private Camera _mainCam;

        private void SpawnBlock(DisplayedItem item)
        {
            BlockPooling.Instance.GetBlock(item.Shape, GetItemWorldPos(item));
        }

        private Vector3 GetItemWorldPos(DisplayedItem item)
        {
            var worldPos = _mainCam.ScreenToWorldPoint(new(item.RectTransform.position.x, item.RectTransform.position.y, 9));
            return worldPos;
        }

        private void Start()
        {
            _crossBlock.OnItemClick += SpawnBlock;
            _squareBlock.OnItemClick += SpawnBlock;
            _lShapeBlock.OnItemClick += SpawnBlock;

            _mainCam = Camera.main;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var isVisible = SceneManager.GetActiveScene().name != "SceneTester";
            _displayShelf.SetActive(isVisible);

            _mainCam = Camera.main;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
