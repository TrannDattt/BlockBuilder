using BuilderTool.Enums;
using BuilderTool.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BuilderTool.UIElement
{
    public class ColorDropdown : GameDropdown<EColor>
    {
        [SerializeField] private Image _selectedItem;
        [SerializeField] private EDirection _direction;

        private void OnColorChanged(int index)
        {
            var color = (EColor)index;
            _selectedItem.color = color == EColor.Black ? Color.black : ColorMapper.GetColor(color);
        }

        protected override void FetchAllItem()
        {
            ClearOptions();

            foreach (var colorKey in ColorMapper.ColorDict.Keys) {
                if(colorKey == EColor.Selected)
                {
                    continue;
                }

                var newItem = GenerateItem(ColorMapper.ColorDict[colorKey]);
                options.Add(new OptionData("", newItem));
            }
        }

        private Sprite GenerateItem(Color color)
        {
            Texture2D texture = new(30, 30);
            for(int i = 0; i < texture.height; i++)
            {
                for(int j = 0; j < texture.width; j++)
                {
                    texture.SetPixel(j, i, color);
                }
            }
            texture.Apply();
            Rect rect = new(0, 0, texture.width, texture.height);
            Vector2 pivot = new(.5f, .5f);

            return Sprite.Create(texture, rect, pivot);
        }

        protected override void Start()
        {
            onValueChanged.AddListener(OnColorChanged);
            base.Start();
        }
    }
}
