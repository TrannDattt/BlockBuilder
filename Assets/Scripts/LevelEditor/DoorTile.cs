using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BuilderTool.FileConvert.FieldInfoConverter;

namespace BuilderTool.LevelEditor
{
    public class DoorTile : ATileAttribute, ICanHaveMechanic
    {
        //public EColor ColorUp { get; private set; } = EColor.Black;
        //public EColor ColorDown { get; private set; } = EColor.Black;
        //public EColor ColorLeft { get; private set; } = EColor.Black;
        //public EColor ColorRight { get; private set; } = EColor.Black;
        //public List<EDirection> StarDirection { get; private set; } = new List<EDirection>(4) { EDirection.None };

        [SerializeField] private SpriteRenderer _upDoor;
        [SerializeField] private SpriteRenderer _downDoor;
        [SerializeField] private SpriteRenderer _leftDoor;
        [SerializeField] private SpriteRenderer _rightDoor;

        public Dictionary<EDirection, EColor> DoorDict { get; private set; } = new Dictionary<EDirection, EColor>
        {
            { EDirection.Up, EColor.Black },
            { EDirection.Down, EColor.Black },
            { EDirection.Left, EColor.Black },
            { EDirection.Right, EColor.Black },
        };

        public override void ResetAttribute()
        {
            base.ResetAttribute();

            ChangeDoorColor(EDirection.Up, EColor.Black);
            ChangeDoorColor(EDirection.Down, EColor.Black);
            ChangeDoorColor(EDirection.Left, EColor.Black);
            ChangeDoorColor(EDirection.Right, EColor.Black);
        }

        public void UpdateAttribute(DoorTileData data)
        {
            ChangeDoorColor(EDirection.Up, data.UpDoor.DoorColor);
            ChangeDoorColor(EDirection.Down, data.DownDoor.DoorColor);
            ChangeDoorColor(EDirection.Left, data.LeftDoor.DoorColor);
            ChangeDoorColor(EDirection.Right, data.RightDoor.DoorColor);
        }

        public void ChangeDoorColor(EDirection dir, EColor color)
        {
            DoorDict[dir] = color;
            GetDoor(dir).color = ColorMapper.GetColor(color);
        }

        public SpriteRenderer GetDoor(EDirection dir) => dir switch
        {
            EDirection.Up => _upDoor,
            EDirection.Down => _downDoor,
            EDirection.Left => _leftDoor,
            EDirection.Right => _rightDoor,
            _ => null,
        };

        public GameObject GetObject()
        {
            return gameObject;
        }
    }
}
