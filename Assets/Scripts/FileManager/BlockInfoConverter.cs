using BuilderTool.Enums;
using BuilderTool.LevelEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderTool.FileConvert
{
    public static class BlockInfoConverter
    {
        [Serializable]
        public class BlockData
        {
            public EShape Shape;
            public EColor PrimaryColor;
            public EColor SecondaryColor;
            public bool ContainKey;
            public bool ContainStar;
            public int PosIndex;

            public BlockData(EditorBlock block)
            {
                Shape = block.Shape;
                PrimaryColor = block.PrimaryColor;
                SecondaryColor = block.SecondaryColor;
                ContainKey = block.ContainKey;
                ContainStar = block.ContainStar;

                var field = EditorField.Instance;
                if (field.BlockDict.ContainsKey(block))
                {
                    PosIndex = field.Tiles.IndexOf(field.BlockDict[block]); 
                }
                else
                {
                    PosIndex = -1;
                    Debug.LogError($"Field does not contain {block}.");
                }
            }
        }

        public static string ConvertBlockToJson()
        {
            string json = "";
            foreach(var block in EditorField.Instance.BlockDict.Keys)
            {
                json += JsonUtility.ToJson(new BlockData(block));
            }

            return json;
        }

        public static void ConvertJsonToBlock(string json)
        {

        }
    }
}
