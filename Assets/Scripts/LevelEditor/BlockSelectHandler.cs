using BuilderTool.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderTool.LevelEditor
{
    public class BlockSelectHandler : Singleton<BlockSelectHandler>
    {
        public EditorBlock SelectedBlock { get; private set; }

        private Camera _mainCam;
        private bool _isPickingUpBlock;
        private Vector3 _rotateAngle = new(0, 0, 90);

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
                    PickUpBlock(block);
                }
                else if (!CheckIfClickedOnUI())
                {
                    RemoveSelectedBlock();
                }
            }

            if(_isPickingUpBlock && Input.GetKeyDown(KeyCode.R)){
                RotateBlock();
            }
        }

        private bool CheckIfClickedOnBlock(out EditorBlock block)
        {
            block = null;
            var mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Block"));

            if (hit.collider)
            {
                block = hit.collider.GetComponent<EditorBlock>();
                return true;
            }

            if(Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Map")))
            {
                RemoveSelectedBlock();
            }

            return false;
        }

        private bool CheckIfClickedOnUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        public void ChangeSelectedBlock(EditorBlock block)
        {
            if(SelectedBlock != block)
            {
                SelectedBlock = block;
                block.SelectBlock();
                OnBlockSelected?.Invoke(block);
            }
        }

        public void RemoveSelectedBlock()
        {
            //OnBlockDrop?.Invoke(block);
            SelectedBlock?.UnselectedBlock();
            SelectedBlock = null;
            OnNoBlockSelected?.Invoke();
        }

        public void PickUpBlock(EditorBlock block)
        {
            ChangeSelectedBlock(block);
            block.PickUpBlock();
            _isPickingUpBlock = true;
        }

        private void DropBlock()
        {
            SelectedBlock.DropBlock();
            OnBlockDrop?.Invoke(SelectedBlock);
            _isPickingUpBlock = false;
        }

        private void RotateBlock(){
            var curRotation = SelectedBlock.transform.rotation.eulerAngles;
            var newRotation = curRotation + _rotateAngle;
            newRotation.Set(newRotation.x, newRotation.y, newRotation.z % 360);
            SelectedBlock.transform.rotation = Quaternion.Euler(newRotation);
        }

        void OnEnable()
        {
            _mainCam = Camera.main;
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
        }
    }
}
