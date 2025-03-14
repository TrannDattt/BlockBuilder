using BuilderTool.Enums;
using BuilderTool.UIElement;
using System;
using System.IO;
using UnityEngine;

namespace BuilderTool.FileConvert
{
    public static class FileManager
    {
        [Serializable]
        public class LevelInfoData
        {
            public string Id;
            public int Time;
            public EDifficulty Difficulty;

            public LevelInfoData()
            {
                Id = LevelManagerMenu.Instance.LevelId;
                Time = LevelManagerMenu.Instance.LevelTime;
                Difficulty = LevelManagerMenu.Instance.LevelDifficulty;
            }
        }

        public static string ConvertLevelToJson()
        {
            var json = $"{{"
                       + $"\"LevelInfo\": {JsonUtility.ToJson(new LevelInfoData())},"
                       + $"\"Tiles\": {FieldInfoConverter.ConvertTileToJson()},"
                       + $"\"Blocks\": {BlockInfoConverter.ConvertBlockToJson()}"
                       + $"}}";

            Debug.Log(json);
            return json;
        }

        public static void SaveJsonFile()
        {
            if (!LevelManagerMenu.Instance.CheckLevelInfo())
            {
                Debug.LogError("Missing level info.");
                return;
            }
            else
            {
                File.WriteAllText($"{Application.dataPath}/level/{LevelManagerMenu.Instance.LevelId}.dat", ConvertLevelToJson());
            }
        }
    }
}
