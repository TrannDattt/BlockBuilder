using BuilderTool.Enums;
using BuilderTool.Helpers;
using UnityEngine;

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

        public void ChangeBlockColor(EColor color)
        {
            Color = color;
            _blockNode.color = ColorMapper.GetColor(color);
        }
    }
}
