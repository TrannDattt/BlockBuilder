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
        [SerializeField] private Button _deleteBlockBtn;

        [Header("Color")]
        [SerializeField] private ColorDropdown _primaryColorSelectorDropdown;
        [SerializeField] private ColorDropdown _secondaryColorSelectorDropdown;

        [Header("Attribute Toggle")]
        [SerializeField] private Toggle _containStarToggle;
        [SerializeField] private Toggle _containKeyToggle;

        [Header("Contraint")]
        [SerializeField] private InputField _freezeCount;
        [SerializeField] private InputField _boomCount;
        [SerializeField] private InputField _chainCount;

        [SerializeField] private DirectionContrainedDropdown _directionContrainedDropdown;

        public void OpenSelectAttributeMenu(EditorBlock block)
        {
            _attributeSelectorMenu.SetActive(true);

            OpenColorSelector(block);
            OpenToggle(block);
        }

        public void OpenColorSelector(EditorBlock block)
        {
            var priColor = block.PrimaryColor;
            var secColor = block.SecondaryColor;

            _primaryColorSelectorDropdown.value = (int)priColor;
            _secondaryColorSelectorDropdown.value = (int)secColor;
        }

        public void OpenToggle(EditorBlock block)
        {
            _containStarToggle.isOn = block.ContainStar;
            _containKeyToggle.isOn = block.ContainKey;
        }

        public void OpenContraintSelector(EditorBlock block){
            // TODO: Get block attribute
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
        }

        private void Start()
        {
            _primaryColorSelectorDropdown.onValueChanged.AddListener(OnPrimaryColorSelected);
            _secondaryColorSelectorDropdown.onValueChanged.AddListener(OnSecondaryColorSelected);

            _containStarToggle.onValueChanged.AddListener(OnContainStarToggled);
            _containKeyToggle.onValueChanged.AddListener(OnContainKeyToggled);

            _deleteBlockBtn.onClick.AddListener(OnDeleteButtonClicked);

            _attributeSelectorMenu.SetActive(false);
        }
    }
}
