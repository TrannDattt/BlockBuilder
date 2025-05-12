using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
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

        [Header("Mechanic Selector")]
        [SerializeField] private MechanicRadioButtonGroup _mechanicButtonGroup;

        public void OpenSelectAttributeMenu(EditorBlock block)
        {
            _attributeSelectorMenu.SetActive(true);
            _attributeSelectorMenu2.SetActive(true);

            OpenColorSelector(block);
            GetBlockMechanic(block);
        }

        public void OpenColorSelector(EditorBlock block)
        {
            var priColor = block.PrimaryColor;

            _primaryColorSelectorDropdown.value = (int)priColor;
        }

        public void GetBlockMechanic(EditorBlock block){
            var mechanic = EditorField.Instance.GetMechanicFromDict(block);
            _mechanicButtonGroup.UpdateMechanicButtonDisplay(mechanic);
        }

        public void CloseMenu()
        {
            _attributeSelectorMenu.SetActive(false);
            _mechanicButtonGroup.ResetButtonGroupDisplay();
            _attributeSelectorMenu2.SetActive(false);
        }

        private void OnPrimaryColorSelected(int index)
        {
            var color = (EColor)index;

            var block = BlockSelectHandler.Instance.SelectedBlock;
            block.ChangeBlockPrimaryColor(color);
        }

        private void OnDeleteButtonClicked()
        {
            var selectedBlock = BlockSelectHandler.Instance.SelectedBlock;
            BlockPlacement.Instance.RemoveBlock(selectedBlock);
        }

        public void LoadBlockMechanic(AMechanic mechanic, ICanHaveMechanic obj){
            _mechanicButtonGroup.LoadMechanicData(mechanic, obj);
        }

        public void RemoveBlockMechanic(EditorBlock block){
            // Debug.Log(block);
            var mechanic = EditorField.Instance.GetMechanicFromDict(block);
            _mechanicButtonGroup.RemoveMechanic(mechanic, block);
        }

        private void Start()
        {
            _primaryColorSelectorDropdown.onValueChanged.AddListener(OnPrimaryColorSelected);

            _deleteBlockBtn.onClick.AddListener(OnDeleteButtonClicked);

            _attributeSelectorMenu.SetActive(false);
            _attributeSelectorMenu2.SetActive(false);
        }
    }
}
