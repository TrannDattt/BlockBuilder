using BuilderTool.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderTool.LevelEditor
{
    public class BlockSelectHandler : Singleton<BlockSelectHandler>
    {
        public List<EditorBlock> SelectedBlocks { get; private set; } = new();

        private Camera _mainCam;
        private bool _isPickingUpBlock;

        public event Action<EditorBlock> OnBlockSelected;
        public event Action OnNoBlockSelected;
        public event Action<EditorBlock> OnBlockDrop;

        private void GetUserInput()
        {
            if (!Input.GetKey(KeyCode.Mouse0) && _isPickingUpBlock)
            {
                DropBlock();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CheckIfClickedOnBlock(out EditorBlock block))
                {
                    //    if (!Input.GetKey(KeyCode.LeftControl))
                    //    {
                    RemoveAllSelectedBlocks();
                    //}

                    //AddSelectedBlock(block);
                    PickUpBlock(block);
                }
                else if (!CheckIfClickedOnUI())
                {
                    RemoveAllSelectedBlocks();
                }

                //if (Input.GetKey(KeyCode.LeftShift))
                //{
                //    _ROIEndPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                //    SelectTileInROI(_ROIStartPos, _ROIEndPos);
                //}
                //else
                //{
                //    _ROIStartPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
                //}
            }
        }

        private bool CheckIfClickedOnBlock(out EditorBlock block)
        {
            block = null;
            var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            if(TileSelectHandler.Instance.SelectedTiles.Count > 0){
                RemoveAllSelectedBlocks();
            }

            var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));

            if (hit.collider)
            {
                block = hit.collider.GetComponent<EditorBlock>();
                return true;
            }

            return false;
        }

        private bool CheckIfClickedOnUI()
        {
            //if(EventSystem.current.IsPointerOverGameObject())
            //{
            //    return EventSystem.current.CompareTag("Block");
            //}

            //return false;
            return EventSystem.current.IsPointerOverGameObject();
        }

        public void AddSelectedBlock(EditorBlock block)
        {
            if(!SelectedBlocks.Contains(block))
            {
                SelectedBlocks.Add(block);
                block.SelectBlock();
                OnBlockSelected?.Invoke(block);
            }
        }

        public void RemoveSelectedBlock(EditorBlock block)
        {
            //OnBlockDrop?.Invoke(block);
            SelectedBlocks.Remove(block);
            block.UnselectedBlock();
        }

        public void RemoveAllSelectedBlocks()
        {
            while(SelectedBlocks.Count > 0)
            {
                RemoveSelectedBlock(SelectedBlocks[0]);
            }

            OnNoBlockSelected?.Invoke();
        }

        public void PickUpBlock(EditorBlock block)
        {
            block.PickUpBlock();
            _isPickingUpBlock = true;
            AddSelectedBlock(block);
        }

        private void DropBlock()
        {
            var selectedBlocks = new List<EditorBlock>(SelectedBlocks);
            foreach (var block in selectedBlocks)
            {
                block.DropBlock();
                OnBlockDrop?.Invoke(block);
            }

            _isPickingUpBlock = false;
        }

        private void Start()
        {
            OnBlockSelected += BlockAttributeSelector.Instance.OpenSelectAttributeMenu;
            OnNoBlockSelected += BlockAttributeSelector.Instance.CloseMenu;
            OnBlockDrop += BlockPlacement.Instance.PlaceBlockOnGrid;

            _mainCam = Camera.main;
        }

        private void OnDestroy()
        {
            OnBlockSelected -= BlockAttributeSelector.Instance.OpenSelectAttributeMenu;
            OnNoBlockSelected -= BlockAttributeSelector.Instance.CloseMenu;
            OnBlockDrop -= BlockPlacement.Instance.PlaceBlockOnGrid;
        }

        private void Update()
        {
            GetUserInput();

            if (!_mainCam)
            {
                _mainCam = Camera.main;
            }
        }
    }
}
