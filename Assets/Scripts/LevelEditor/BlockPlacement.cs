using BuilderTool.Editor;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class BlockPlacement : Singleton<BlockPlacement>
    {
        private Dictionary<EShape, List<Vector3Int>> _blockDict = new Dictionary<EShape, List<Vector3Int>>
        {
            { EShape.Square, new List<Vector3Int> { new(0, 0), new(0, 1), new(1, 0), new(1, 1) } },
            { EShape.LShaped, new List<Vector3Int> { new(0, 0), new(0, 1), new(1, -1), new(0, -1) } },
            { EShape.Cross, new List<Vector3Int> { new(0, 0), new(1, 0), new(-1, 0), new(0, 1), new(0, -1) } }
        };

        private List<Vector3Int> GetBlockTile(EShape blockShape)
        {
            if (!_blockDict.ContainsKey(blockShape))
            {
                Debug.Log("No shape matched.");
                return null;
            }

            return _blockDict[blockShape];
        }

        public void PlaceBlockOnGrid(EditorBlock block)
        {
            if(CheckCanPlaceBlockOnGrid(block, out EditorTile tile))
            {
                SnapBlockToTile(block, tile);
                EditorField.Instance.UpdateBlockPos(block, tile);
            }
            else
            {
                RemoveBlock(block);
            }
        }

        public void RemoveBlock(params EditorBlock[] blocks)
        {
            foreach(var block in blocks)
            {
                BlockAttributeSelector.Instance.RemoveBlockMechanic(block);
                BlockPooling.Instance.ReturnBlock(block);
                EditorField.Instance.RemoveBlock(block);
            }
            BlockSelectHandler.Instance.RemoveSelectedBlock();
        }

        private bool CheckCanPlaceBlockOnGrid(EditorBlock block, out EditorTile mainTile)
        {
            mainTile = null;

            var coreHit = Physics2D.Raycast(block.BlockCoreUnit.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Map"));
            if (!coreHit.collider)
            {
                return false;
            }

            mainTile = coreHit.collider.GetComponent<EditorTile>();
            if (!mainTile.CanContainBlock)
            {
                return false;
            }

            foreach(var subUnit in block.BlockSubUnits)
            {
                var hit = Physics2D.Raycast(subUnit.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("Map"));
                if (!hit.collider)
                {
                    return false;
                }

                var tile = hit.collider.GetComponent<EditorTile>();
                if (!tile.CanContainBlock)
                {
                    return false;
                }
            }

            return true;
        }

        private void SnapBlockToTile(EditorBlock block, EditorTile tile)
        {
            var snapOffset = new Vector2(tile.transform.position.x - block.BlockCoreUnit.transform.position.x, tile.transform.position.y - block.BlockCoreUnit.transform.position.y);
            block.transform.position += (Vector3)snapOffset;

            block.CurrentTile = tile;

        }
    }
}