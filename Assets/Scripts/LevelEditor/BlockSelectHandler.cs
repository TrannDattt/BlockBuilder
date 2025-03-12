using BuilderTool.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class BlockSelectHandler : Singleton<BlockSelectHandler>
    {
        public List<EditorBlock> SelectedBlocks { get; private set; } = new();

        private bool _isMultiSelecting;

        public event Action<EditorBlock> OnBlockSelected;
        public event Action<EditorBlock> OnBlockDrop;

        public void AddSelectedBlock(EditorBlock block)
        {
            //if(SelectedBlocks.Count != 1 || SelectedBlocks[0] != block)
            if (!_isMultiSelecting)
            {
                RemoveAllSelectedBlocks();
            }

            if(!SelectedBlocks.Contains(block))
            {
                SelectedBlocks.Add(block);

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
        }

        private void DropBlock()
        {
            var selectedBlocks = new List<EditorBlock>(SelectedBlocks);
            foreach (var block in selectedBlocks)
            {
                block.DropBlock();
                OnBlockDrop?.Invoke(block);
            }
        }

        private void GetUserInput()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DropBlock();
            }
        }

        private void Start()
        {
            OnBlockSelected += BlockAttributeSelector.Instance.OpenSelectAttributeMenu;
            OnBlockDrop += BlockPlacement.Instance.PlaceBlockOnGrid;
        }

        private void OnDestroy()
        {
            OnBlockSelected -= BlockAttributeSelector.Instance.OpenSelectAttributeMenu;
            OnBlockDrop -= BlockPlacement.Instance.PlaceBlockOnGrid;
        }

        private void Update()
        {
            GetUserInput();
        }
    }
}
