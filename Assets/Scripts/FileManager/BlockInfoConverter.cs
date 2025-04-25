using BuilderTool.Enums;
using BuilderTool.LevelEditor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var blocks = EditorField.Instance.BlockDict.Keys.ToList<EditorBlock>() ;

            foreach (var block in blocks)
            {
                json += JsonUtility.ToJson(new BlockData(block));
                json += blocks.IndexOf(block) != blocks.Count - 1 ? ',' : "";
            }

            return json;
        }

        public static void ConvertJsonToBlock(string json)
        {
            //Debug.Log(json);
            JArray blockArray = JArray.Parse(json);
            List<string> blockJsons = new();

            foreach(var block in blockArray)
            {
                var blockJson = block.ToString(Newtonsoft.Json.Formatting.None);
                blockJsons.Add(blockJson);
            }

            List<BlockData> blockDatas = ConvertJsonToList<BlockData>(blockJsons);

            EditorField.Instance.UpdateBlockData(blockDatas);
        }

        private static List<T> ConvertJsonToList<T>(List<string> jsonList)
        {
            List<T> data = new();
            foreach (var json in jsonList)
            {
                //Debug.Log(json);
                var newData = JsonUtility.FromJson<T>(json);
                data.Add(newData);
            }

            return data;
        }
    }
}
