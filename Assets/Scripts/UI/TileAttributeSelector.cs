using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.UIElement;
using System;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class TileAttributeSelector : Singleton<TileAttributeSelector>
    {
        [SerializeField] private GameObject _attributeSelectorMenu;
        [SerializeField] private GameObject _doorAttributSelectorMenu;
        [SerializeField] private TileTypeDropdown _tileTypeSelectorDropdown;

        [SerializeField] private ColorDropdown _colorUpSelectorDropdown;
        [SerializeField] private ColorDropdown _colorDownSelectorDropdown;
        [SerializeField] private ColorDropdown _colorLeftSelectorDropdown;
        [SerializeField] private ColorDropdown _colorRightSelectorDropdown;

        [SerializeField] private ColorDropdown _blockColorSelectorDropdown;

        public void OpenSelectAttributeMenu(EditorTile tile)
        {
            _attributeSelectorMenu.SetActive(true);
            _tileTypeSelectorDropdown.SetValueWithoutNotify((int)tile.TileType);

            HideAllSelector();

            switch(tile.TileType)
            {
                //case ETileType.Empty:
                //    break;

                //case ETileType.Ground:
                //    break;

                case ETileType.BlockNode:
                    OpenBlockAttributeSelector(tile);
                    break;

                case ETileType.Door:
                    OpenDoorAttributeSelector(tile);
                    break;

                default:
                    break;
            }
        }

        private void HideAllSelector()
        {
            _blockColorSelectorDropdown.gameObject.SetActive(false);
            _colorUpSelectorDropdown.gameObject.SetActive(false);
            _colorDownSelectorDropdown.gameObject.SetActive(false);
            _colorLeftSelectorDropdown.gameObject.SetActive(false);
            _colorRightSelectorDropdown.gameObject.SetActive(false);
            _doorAttributSelectorMenu.SetActive(false);
        }

        public void CloseMenu()
        {
            HideAllSelector();
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

            _doorAttributSelectorMenu.SetActive(true);
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
