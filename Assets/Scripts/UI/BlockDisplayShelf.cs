using BuilderTool.Editor;
using BuilderTool.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuilderTool.LevelEditor
{
    public class BlockDisplayShelf : Singleton<BlockDisplayShelf>
    {
        [SerializeField] private GameObject _displayShelf;

        [SerializeField] private DisplayedItem _crossBlock;
        [SerializeField] private DisplayedItem _squareBlock;
        [SerializeField] private DisplayedItem _lShapeBlock;

        private Camera _mainCam;

        public void OpenShelf(){
            _displayShelf.SetActive(true);
        }

        public void CloseShelf(){
            _displayShelf.SetActive(false);
        }

        private void SpawnBlock(DisplayedItem item)
        {
            var newBlock = BlockPooling.Instance.GetBlock(item.Shape, GetItemWorldPos(item));
            BlockSelectHandler.Instance.PickUpBlock(newBlock);
        }

        private Vector3 GetItemWorldPos(DisplayedItem item)
        {
            var worldPos = _mainCam.ScreenToWorldPoint(new(item.RectTransform.position.x, item.RectTransform.position.y, 9));
            return worldPos;
        }

        private void OnEnable()
        {
            _crossBlock.OnItemClick += SpawnBlock;
            _squareBlock.OnItemClick += SpawnBlock;
            _lShapeBlock.OnItemClick += SpawnBlock;

            _mainCam = Camera.main;
        }

        void OnDisable()
        {
            _crossBlock.OnItemClick -= SpawnBlock;
            _squareBlock.OnItemClick -= SpawnBlock;
            _lShapeBlock.OnItemClick -= SpawnBlock;

        }
    }
}
