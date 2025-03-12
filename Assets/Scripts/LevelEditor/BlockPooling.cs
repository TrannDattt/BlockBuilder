using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderTool.Editor
{
    public class BlockPooling : Singleton<BlockPooling>
    {
        [SerializeField] private EditorBlock _crossBlock;
        [SerializeField] private EditorBlock _squareBlock;
        [SerializeField] private EditorBlock _lShapeBlock;

        private Queue<EditorBlock> _crossBlockQueue = new();
        private Queue<EditorBlock> _squareBlockQueue = new();
        private Queue<EditorBlock> _lShapeBlockQueue = new();

        public void GetBlock(EShape shape, Vector3 spawnedPos)
        {
            switch (shape)
            {
                case EShape.Square:
                    GetBlockFromPool(_squareBlock, ref _squareBlockQueue, spawnedPos);
                    break;

                case EShape.Cross:
                    GetBlockFromPool(_crossBlock, ref _crossBlockQueue, spawnedPos);
                    break;

                case EShape.LShaped:
                    GetBlockFromPool(_lShapeBlock, ref _lShapeBlockQueue, spawnedPos);
                    break;

                default:
                    break;
            }
        }

        private void GetBlockFromPool(EditorBlock block, ref Queue<EditorBlock> pool, Vector3 spawnedPos)
        {
            if(pool.Count == 0)
            {
                var newBlock = Instantiate(block, spawnedPos, Quaternion.identity);
                var blockParent = GameObject.Find("----Blocks----");
                newBlock.transform.SetParent(blockParent.transform);

                newBlock.gameObject.SetActive(false);
                pool.Enqueue(newBlock);
            }
            var spawnedBlock = pool.Dequeue();

            spawnedBlock.gameObject.SetActive(true);
            spawnedBlock.transform.localPosition = spawnedPos;

            spawnedBlock.InitBlock();
            spawnedBlock.PickUpBlock();
        }

        public void ReturnBlock(EditorBlock block)
        {
            switch (block.Shape)
            {
                case EShape.Square:
                    ReturnBlockToPool(block, ref _squareBlockQueue);
                    break;

                case EShape.Cross:
                    ReturnBlockToPool(block, ref _crossBlockQueue);
                    break;

                case EShape.LShaped:
                    ReturnBlockToPool(block, ref _lShapeBlockQueue);
                    break;

                default:
                    break;
            }
        }

        private void ReturnBlockToPool(EditorBlock block, ref Queue<EditorBlock> pool)
        {
            pool.Enqueue(block);
            block.gameObject.SetActive(false);
        }
    }
}
