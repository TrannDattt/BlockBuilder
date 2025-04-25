using BuilderTool.Enums;
using BuilderTool.Helpers;
using UnityEngine;
using static BuilderTool.FileConvert.FieldInfoConverter;

namespace BuilderTool.LevelEditor
{
    public class BlockTile : ATileAttribute
    {
        [SerializeField] private SpriteRenderer _blockNode;

        public EColor Color { get; private set; } = EColor.Black;

        public override void ResetAttribute()
        {
            base.ResetAttribute();

            ChangeBlockColor(EColor.Black);
        }

        public void UpdateAttribute(BlockTileData data)
        {
            ChangeBlockColor(data.BlockColor);
        }

        public void ChangeBlockColor(EColor color)
        {
            Color = color;
            _blockNode.color = ColorMapper.GetColor(color);
        }

        public void FrozenBlock(int frozen_number)
        {
            if (frozen_number >= 1)
            {

            }
        }
    }
}
