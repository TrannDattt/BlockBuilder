using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
using BuilderTool.UIElement;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class TileAttributeSelector : Singleton<TileAttributeSelector>
    {
        [SerializeField] private GameObject _attributeSelectorMenu;
        [SerializeField] private GameObject _doorMechanicSelectorMenu;

        [Header("Tile Type")]
        [SerializeField] private TileTypeDropdown _tileTypeSelectorDropdown;

        [Header("Door Attribute")]
        [SerializeField] private ColorDropdown _colorUpSelectorDropdown;
        [SerializeField] private ColorDropdown _colorDownSelectorDropdown;
        [SerializeField] private ColorDropdown _colorLeftSelectorDropdown;
        [SerializeField] private ColorDropdown _colorRightSelectorDropdown;

        [Header("Pillar Attribute")]
        [SerializeField] private ColorDropdown _blockColorSelectorDropdown;

        [Header("Mechanic Selector")]
        [SerializeField] private MechanicRadioButtonGroup _doorMechanicButtonGroup;

        public void OpenSelectAttributeMenu(EditorTile tile)
        {
            _attributeSelectorMenu.SetActive(true);

            _tileTypeSelectorDropdown.SetValueWithoutNotify((int)tile.TileType);

            HideAttributeSelector();
            HideDoorMechanicSeletor();

            switch(tile.TileType)
            {
                case ETileType.BlockNode:
                    OpenBlockAttributeSelector(tile);
                    break;

                case ETileType.Door:
                    OpenDoorAttributeSelector(tile);
                    _doorMechanicSelectorMenu.SetActive(true);
                    GetDoorMechanic(tile.CurTileAttribute as DoorTile);
                    break;

                default:
                    break;
            }
        }

        public void GetDoorMechanic(DoorTile door){
            var upMechanic = EditorField.Instance.GetMechanicFromDict(door, EDirection.Up);
            var downMechanic = EditorField.Instance.GetMechanicFromDict(door, EDirection.Down);
            var leftMechanic = EditorField.Instance.GetMechanicFromDict(door, EDirection.Left);
            var rightMechanic = EditorField.Instance.GetMechanicFromDict(door, EDirection.Right);

            // Debug.Log((upMechanic as IImmoblized)?.TurnCount);
            // Debug.Log((downMechanic as IImmoblized)?.TurnCount);
            // Debug.Log((leftMechanic as IImmoblized)?.TurnCount);
            // Debug.Log((rightMechanic as IImmoblized)?.TurnCount);

            // Debug.Log(upMechanic);
            // Debug.Log(downMechanic);
            // Debug.Log(leftMechanic);
            // Debug.Log(rightMechanic);

            _doorMechanicButtonGroup.UpdateMechanicDisplay(upMechanic, door);
            _doorMechanicButtonGroup.UpdateMechanicDisplay(downMechanic, door);
            _doorMechanicButtonGroup.UpdateMechanicDisplay(leftMechanic, door);
            _doorMechanicButtonGroup.UpdateMechanicDisplay(rightMechanic, door);
        }

        private void HideAttributeSelector()
        {
            _blockColorSelectorDropdown.gameObject.SetActive(false);
            _colorUpSelectorDropdown.gameObject.SetActive(false);
            _colorDownSelectorDropdown.gameObject.SetActive(false);
            _colorLeftSelectorDropdown.gameObject.SetActive(false);
            _colorRightSelectorDropdown.gameObject.SetActive(false);
        }

        private void HideDoorMechanicSeletor(){
            // _doorMechanicButtonGroup.ResetButtonGroupDisplay();
            _doorMechanicSelectorMenu.SetActive(false);
        }

        public void CloseMenu()
        {
            HideAttributeSelector();
            HideDoorMechanicSeletor();
            _attributeSelectorMenu.SetActive(false);
        }

        private void OpenDoorAttributeSelector(EditorTile tile)
        {
            var doorAttribute = tile.CurTileAttribute as DoorTile;

            _colorUpSelectorDropdown.value = (int)doorAttribute.DoorDict[EDirection.Up];
            _colorDownSelectorDropdown.value = (int)doorAttribute.DoorDict[EDirection.Down];
            _colorLeftSelectorDropdown.value = (int)doorAttribute.DoorDict[EDirection.Left];
            _colorRightSelectorDropdown.value = (int)doorAttribute.DoorDict[EDirection.Right];

            _colorUpSelectorDropdown.gameObject.SetActive(true);
            _colorDownSelectorDropdown.gameObject.SetActive(true);
            _colorLeftSelectorDropdown.gameObject.SetActive(true);
            _colorRightSelectorDropdown.gameObject.SetActive(true);
        }

        private void OpenBlockAttributeSelector(EditorTile tile)
        {
            _blockColorSelectorDropdown.gameObject.SetActive(true);
            var blockAttribute = tile.CurTileAttribute as BlockTile;
            _blockColorSelectorDropdown.value = (int)blockAttribute.Color;
        }

        private void OnAttributeSelectorChanged(int index)
        {
            var tileType = (ETileType)index;
            foreach(var tile in TileSelectHandler.Instance.SelectedTiles)
            {
                tile.ChangeTileType(tileType);
            }
            OpenSelectAttributeMenu(TileSelectHandler.Instance.SelectedTiles[0]);
        }

        private void OnDoorColorChanged(EDirection dir, int index)
        {
            var color = (EColor)index;

            foreach (var tile in TileSelectHandler.Instance.SelectedTiles)
            {
                (tile.CurTileAttribute as DoorTile).ChangeDoorColor(dir, color);
            }
        }

        private void OnBlockColorChanged(int index)
        {
            var color = (EColor)index;

            foreach (var tile in TileSelectHandler.Instance.SelectedTiles)
            {
                (tile.CurTileAttribute as BlockTile).ChangeBlockColor(color);
            }
        }

        public void LoadDoorMechanic(AMechanic mechanic, ICanHaveMechanic obj){
            _doorMechanicButtonGroup.AssignMechanicToObject(mechanic, obj);
        }

        private void Start()
        {
            _tileTypeSelectorDropdown.onValueChanged.AddListener(OnAttributeSelectorChanged);

            _colorUpSelectorDropdown.onValueChanged.AddListener((value) => OnDoorColorChanged(EDirection.Up, value));
            _colorDownSelectorDropdown.onValueChanged.AddListener((value) => OnDoorColorChanged(EDirection.Down, value));
            _colorLeftSelectorDropdown.onValueChanged.AddListener((value) => OnDoorColorChanged(EDirection.Left, value));
            _colorRightSelectorDropdown.onValueChanged.AddListener((value) => OnDoorColorChanged(EDirection.Right, value));
            _blockColorSelectorDropdown.onValueChanged.AddListener(OnBlockColorChanged);

            CloseMenu();
        }
    }
}
