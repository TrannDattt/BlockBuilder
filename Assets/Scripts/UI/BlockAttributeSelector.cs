using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.UIElement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderTool.LevelEditor
{
    public class BlockAttributeSelector : Singleton<BlockAttributeSelector>
    {
        [SerializeField] private GameObject _attributeSelectorMenu;
        [SerializeField] private GameObject _attributeSelectorMenu2;
        [SerializeField] private Button _deleteBlockBtn;

        [Header("Color")]
        [SerializeField] private ColorDropdown _primaryColorSelectorDropdown;
        [SerializeField] private ColorDropdown _secondaryColorSelectorDropdown;

        [Header("Attribute Toggle")]
        [SerializeField] private ToolRadioButton _containStarToggle;
        [SerializeField] private ToolRadioButton _containKeyToggle;

        [Header("Contraint")]
        [SerializeField] private TMP_InputField _freezeCount;
        [SerializeField] private TMP_InputField _boomCount;
        [SerializeField] private TMP_InputField _chainCount;

        [SerializeField] private DirectionContrainedDropdown _directionContrainedDropdown;

        public void OpenSelectAttributeMenu(EditorBlock block)
        {
            _attributeSelectorMenu.SetActive(true);
            _attributeSelectorMenu2.SetActive(true);

            OpenColorSelector(block);
            OpenToggle(block);
            OpenContraintSelector(block);
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
            _containStarToggle.ChangeState(block.ContainStar);
            _containKeyToggle.ChangeState(block.ContainKey);
        }

        public void OpenContraintSelector(EditorBlock block){
            // TODO: Get block attribute
        }

        public void CloseMenu()
        {
            _attributeSelectorMenu.SetActive(false);
            _attributeSelectorMenu2.SetActive(false);
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

            // _containStarToggle.OnButtonClicked += OnContainStarToggled;
            // _containKeyToggle.onValueChanged.AddListener(OnContainKeyToggled);

            _deleteBlockBtn.onClick.AddListener(OnDeleteButtonClicked);

            _attributeSelectorMenu.SetActive(false);
            _attributeSelectorMenu2.SetActive(false);
        }
    }
}
