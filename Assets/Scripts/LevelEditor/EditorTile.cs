using BuilderTool.Enums;
using BuilderTool.Helpers;
using SerializeReferenceEditor;
using System;
using UnityEngine;
//using NewtonSoft.Json;

namespace BuilderTool.LevelEditor
{
    public class EditorTile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private EmptyTile _emptyTile;
        [SerializeField] private GroundTile _groundTile;
        [SerializeField] private DoorTile _doorTile;
        [SerializeField] private BlockTile _blockTile;

        //[SR]
        //[SerializeReference]
        public ATileAttribute CurTileAttribute { get; private set; }
        public ETileType TileType { get; private set; }
        public bool CanContainBlock => CurTileAttribute.CanContainBlock;
        public bool IsEmpty { get; private set; }

        public void InitTile()
        {
            ResetAttribute();

            ActivateAttribute(ETileType.Empty);
            CurTileAttribute = _emptyTile;
            TileType = ETileType.Empty;

            _spriteRenderer.color = _emptyTile.TileColor;

            IsEmpty = true;
        }

        private void ResetAttribute()
        {
            _emptyTile.ResetAttribute();
            _groundTile.ResetAttribute();
            _doorTile.ResetAttribute();
            _blockTile.ResetAttribute();
        }

        private void ActivateAttribute(ETileType tileType)
        {
            _emptyTile.gameObject.SetActive(tileType == _emptyTile.TileType);
            _groundTile.gameObject.SetActive(tileType == _groundTile.TileType);
            _doorTile.gameObject.SetActive(tileType == _doorTile.TileType);
            _blockTile.gameObject.SetActive(tileType == _blockTile.TileType);
        }

        public void ChangeTileType(ETileType tileType)
        {
            ActivateAttribute(tileType);

            switch (tileType)
            {
                case ETileType.Empty:
                    TileType = ETileType.Empty;
                    CurTileAttribute = _emptyTile;
                    break;

                case ETileType.Ground:
                    TileType = ETileType.Ground;
                    CurTileAttribute = _groundTile;
                    break;

                case ETileType.Door:
                    TileType = ETileType.Door; 
                    CurTileAttribute = _doorTile;
                    break;

                case ETileType.BlockNode:
                    TileType = ETileType.BlockNode;
                    CurTileAttribute = _blockTile;
                    break;

                default:
                    break;
            }

            _spriteRenderer.color = CurTileAttribute.TileColor;
            TileType = tileType;
        }

        public void SelectTile()
        {
            _spriteRenderer.color = ColorMapper.GetColor(EColor.Selected);
        }

        public void UnselectTile()
        {
            _spriteRenderer.color = CurTileAttribute.TileColor;
        }
    }
}
