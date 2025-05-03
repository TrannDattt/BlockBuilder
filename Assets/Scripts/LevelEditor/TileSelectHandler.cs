using BuilderTool.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderTool.LevelEditor
{
    public class TileSelectHandler : Singleton<TileSelectHandler>
    {
        public List<EditorTile> SelectedTiles { get; private set; } = new();

        private Vector2 _ROIStartPos;
        private Vector2 _ROIEndPos;

        private Camera _mainCam;

        public event Action<EditorTile> OnTileSelected;
        public event Action OnNoTileSelected;

        private void GetUserInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CheckIfClickedOnTile(out EditorTile tile))
                {
                    if (!Input.GetKey(KeyCode.LeftControl))
                    {
                        RemoveAllSelectedTile();
                    }

                    AddSelectedTile(tile);
                }
                else if(!CheckIfClickedOnUI())
                {
                    RemoveAllSelectedTile();
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _ROIEndPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                    SelectTileInROI(_ROIStartPos, _ROIEndPos);
                }
                else
                {
                    _ROIStartPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }

        private bool CheckIfClickedOnTile(out EditorTile tile)
        {
            tile = null;
            var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            if(Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"))){
                RemoveAllSelectedTile();
                return false;
            }

            var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Map"));

            if (hit.collider)
            {
                tile = hit.collider.GetComponent<EditorTile>();
                return true;
            }

            return false;
        }

        private bool CheckIfClickedOnUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void SelectTileInROI(Vector2 startPos, Vector2 endPos)
        {
            var overlapColliders = Physics2D.OverlapAreaAll(startPos, endPos, LayerMask.GetMask("Map"));
            foreach(var collider in overlapColliders)
            {
                var tile = collider.GetComponent<EditorTile>();
                AddSelectedTile(tile);
            }
        }

        public void AddSelectedTile(EditorTile tile)
        {
            if(!SelectedTiles.Contains(tile))
            {
                SelectedTiles.Add(tile);
                tile.SelectTile();
                OnTileSelected?.Invoke(tile);
            }
        }

        public void RemoveSelectedTile(EditorTile tile)
        {
            tile.UnselectTile();
            SelectedTiles.Remove(tile);
        }

        public void RemoveAllSelectedTile()
        {
            while (SelectedTiles.Count > 0)
            {
                RemoveSelectedTile(SelectedTiles[0]);
            }

            OnNoTileSelected?.Invoke();
        }

        private void Start()
        {
            OnTileSelected += TileAttributeSelector.Instance.OpenSelectAttributeMenu;
            OnNoTileSelected += TileAttributeSelector.Instance.CloseMenu;

            _mainCam = Camera.main;
        }

        private void Update()
        {
            GetUserInput();

            if (!_mainCam)
            {
                _mainCam = Camera.main;
            }
        }

        private void OnDestroy()
        {
            OnTileSelected -= TileAttributeSelector.Instance.OpenSelectAttributeMenu;

            OnNoTileSelected -= TileAttributeSelector.Instance.CloseMenu;
        }
    }
}
