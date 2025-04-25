using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.UIElement;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderTool.LevelEditor
{
    public class BlockAttributeSelector : Singleton<BlockAttributeSelector>
    {
        [SerializeField] private GameObject _attributeSelectorMenu;
        [SerializeField] private GameObject _blockSpecialsSelectorMenu;

        [SerializeField] private ColorDropdown _primaryColorSelectorDropdown;
        [SerializeField] private ColorDropdown _secondaryColorSelectorDropdown;

        [SerializeField] private Toggle _containStarToggle;
        [SerializeField] private Toggle _containKeyToggle;

        [SerializeField] private Button _deleteBlockBtn;

        public void OpenSelectAttributeMenu(EditorBlock block)
        {
            _attributeSelectorMenu.SetActive(true);
            _blockSpecialsSelectorMenu.SetActive(true);

            OpenPrimaryColorSelector(block);
            OpenSecondaryColorSelector(block);
            OpenStarToggle(block);
            OpenKeyToggle(block);
        }

        public void OpenPrimaryColorSelector(EditorBlock block)
        {
            var priColor = block.PrimaryColor;

            _primaryColorSelectorDropdown.value = (int)priColor;
        }

        public void OpenSecondaryColorSelector(EditorBlock block)
        {
            var secColor = block.SecondaryColor;

            _secondaryColorSelectorDropdown.value = (int)secColor;
        }

        public void OpenStarToggle(EditorBlock block)
        {
            _containStarToggle.isOn = block.ContainStar;
        }

        public void OpenKeyToggle(EditorBlock block)
        {
            _containKeyToggle.isOn = block.ContainKey;
        }

        public void CloseMenu()
        {
            _attributeSelectorMenu.SetActive(false);
        }

        private void OnPrimaryColorSelected(int index)
        {
            var color = (EColor)index;
            Debug.Log(index);

            foreach(var block in BlockSelectHandler.Instance.SelectedBlocks)
            {
                block.ChangeBlockPrimaryColor(color);
            }
        }

        private void OnSecondaryColorSelected(int index)
        {
            var color = (EColor)index;
            Debug.Log(index);

            foreach (var block in BlockSelectHandler.Instance.SelectedBlocks)
            {
                block.ChangeBlockSecondaryColor(color);
            }
        }

        private void OnContainStarToggled(bool isOn)
        {
            
        }

        private void OnContainKeyToggled(bool isOn)
        {

        }

        private void OnDeleteButtonClicked()
        {
            BlockPlacement.Instance.RemoveBlock(BlockSelectHandler.Instance.SelectedBlocks.ToArray());
            CloseMenu();
        }

        public void CloseMenu()
        {
            _attributeSelectorMenu.SetActive(false);
            _blockSpecialsSelectorMenu.SetActive(false);
        }

        private void Start()
        {
            _primaryColorSelectorDropdown.onValueChanged.AddListener(OnPrimaryColorSelected);
            _secondaryColorSelectorDropdown.onValueChanged.AddListener(OnSecondaryColorSelected);

            _containStarToggle.onValueChanged.AddListener(OnContainStarToggled);
            _containKeyToggle.onValueChanged.AddListener(OnContainKeyToggled);

            _deleteBlockBtn.onClick.AddListener(OnDeleteButtonClicked);

            CloseMenu();
        }
    }
}
