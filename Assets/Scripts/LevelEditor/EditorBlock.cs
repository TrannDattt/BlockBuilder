using BuilderTool.Editor;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static BuilderTool.FileConvert.BlockInfoConverter;

namespace BuilderTool.LevelEditor
{
    public class EditorBlock : MonoBehaviour, ICanHaveMechanic
    {
        [SerializeField] private List<SpriteRenderer> _priRenderers;
        //[SerializeField] private List<SpriteRenderer> _secRenderers;

        [field: SerializeField] public GameObject BlockCoreUnit { get; private set; }
        [field: SerializeField] public List<GameObject> BlockSubUnits { get; private set; }
        [field: SerializeField] public EShape Shape { get; private set; }

        public EColor PrimaryColor { get; private set; }
        public EColor SecondaryColor { get; private set; }
        public bool ContainKey { get; private set; }
        public bool ContainStar { get; private set; }
        public EditorTile CurrentTile { get; set; }

        private Camera _mainCamera;
        private Vector3 _offsetToPointer;
        private bool _isPickedUp = false;

        public void InitBlock()
        {
            _mainCamera = Camera.main;

            PrimaryColor = EColor.Black;
            SecondaryColor = EColor.Black;

            ChangeBlockPrimaryColor(PrimaryColor);
        }

        public void UpdateBlockData(BlockData data)
        {
            ChangeBlockPrimaryColor(data.PrimaryColor);
            transform.rotation = Quaternion.Euler(data.Rotation);
            //Change
        }

        public void ChangeBlockPrimaryColor(EColor color)
        {
            if(color != EColor.Selected)
            { 
                PrimaryColor = color; 
            }

            foreach(var renderer in _priRenderers)
            {
                renderer.color = color == EColor.Black ? Color.black : ColorMapper.GetColor(color);
            }
        }

        public void SelectBlock()
        {
            //_isSelected = true;
        }

        public void UnselectedBlock()
        {
            //_isSelected = false;
            //ChangeBlockPrimaryColor(PrimaryColor);
        }

        public void PickUpBlock()
        {
            _isPickedUp = true;
            _offsetToPointer = transform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        public void DropBlock()
        {
            _isPickedUp = false;
        }

        public void MoveBlockAfterPointer()
        {
            transform.position = _mainCamera.ScreenToWorldPoint(Input.mousePosition) + _offsetToPointer;
        }

        private void Update()
        {
            if (_isPickedUp)
            {
                MoveBlockAfterPointer();
            }
        }

        public GameObject GetObject()
        {
            return gameObject;
        }
    }
}
