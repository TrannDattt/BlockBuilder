using BuilderTool.Enums;
using BuilderTool.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class EditorField : Singleton<EditorField>
    {
        private const int FIELD_HEIGHT = 15;
        private const int FIELD_WIDTH = 15;

        [field: SerializeField] public List<EditorTile> Tiles { get; private set; }

        public Dictionary<EditorBlock, EditorTile> BlockDict { get; private set; } = new();

        public void CreateNewField()
        {
            foreach (var tile in Tiles)
            {
                tile.InitTile();
            }

            BlockDict.Clear();
        }

        public void UpdateBlockPos(EditorBlock block, EditorTile tile)
        {
            if(BlockDict.ContainsKey(block))
            { 
                BlockDict[block] = tile; 
            }
            else
            {
                BlockDict.Add(block, tile);
            }
        }

        public void RemoveBlock(EditorBlock block)
        {
            if (!BlockDict.ContainsKey(block))
            {
                return;
            }
            BlockDict.Remove(block);
        }

        private void Start()
        {
            CreateNewField();
        }

        private void Update()
        {
            //LogTileAttribute();
        }
    }
}
