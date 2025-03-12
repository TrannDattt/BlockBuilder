//using BuilderTool.Enum;
//using BuilderTool.Helpers;
//using BuilderTool.LevelEditor;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//namespace BuilderTool.Editor
//{
//    public class TileManager : Singleton<TileManager>
//    {
//        [SerializeField] private Sprite _emptyTileSprite;
//        [SerializeField] private Sprite _blockTileSprite;
//        [SerializeField] private Sprite _groundTileSprite;
//        [SerializeField] private Sprite _selectedTileSprite;

//        public void ChangeTileSprite(Tile tile, ETileType tileType)
//        {
//            switch (tileType)
//            {
//                case ETileType.Empty:
//                    tile.sprite = _emptyTileSprite;
//                    break;

//                case ETileType.Ground:
//                    tile.sprite = _groundTileSprite;
//                    break;

//                //case ETileType.ColorNode:
//                //    tile.sprite = _blockTileSprite;
//                //    break;

//                case ETileType.BlockNode:
//                    tile.sprite = _blockTileSprite;
//                    break;

//                case ETileType.Door:
//                    tile.sprite = _groundTileSprite;
//                    break;

//                default:
//                    break;
//            }
//        }

//        public void ChangeTileType(Tilemap field, Vector3Int pos)
//        {
            
//        }
//    }
//}
