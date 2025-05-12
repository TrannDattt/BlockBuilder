using BuilderTool.Enums;
using BuilderTool.UIElement;
using SFB;
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

        private static string _lastDirectory = "LastDirectory";

        public static string ConvertLevelToJson()
        {
            var json = $"{{"
                       + $"\"LevelInfo\":{JsonUtility.ToJson(new LevelInfoData())},"
                       + $"\"Tiles\":[{FieldInfoConverter.ConvertTileToJson()}],"
                       + $"\"Blocks\":[{BlockInfoConverter.ConvertBlockToJson()}],"
                       + $"\"Mechanics\":[{MechanicInfoConverter.ConvertMechanicToJson()}]"
                       + $"}}";

            return json;
        }

        public static void ConvertJsonToLevel(string json)
        {
            SplitJson(json, out string levelJson, out string tilesJson, out string blocksJson, out string mechanicJson);

            var levelInfoData = JsonUtility.FromJson<LevelInfoData>(levelJson);
            LevelManagerMenu.Instance.UpdateLevelInfo(levelInfoData);

            FieldInfoConverter.ConvertJsonToTile(tilesJson);
            BlockInfoConverter.ConvertJsonToBlock(blocksJson);
            MechanicInfoConverter.ConvertJsonToMechanic(mechanicJson);
        }

        private static void SplitJson(string json, out string levelJson, out string tilesJson, out string blocksJson, out string mechanicJson)
        {
            levelJson = "";
            tilesJson = "";
            blocksJson = "";
            mechanicJson = "";

            int levelInfoStartIndex = json.IndexOf("\"LevelInfo\":") + 12;
            int tilesStartIndex = json.IndexOf("\"Tiles\":") + 8;
            int blocksStartIndex = json.IndexOf("\"Blocks\":") + 9;
            int mechanicsStartIndex = json.IndexOf("\"Mechanics\":") + 12;

            if(levelInfoStartIndex == -1 || tilesStartIndex == -1 || blocksStartIndex == -1 || mechanicsStartIndex == -1)
            {
                Debug.Log("Invalided JSON.");
                return;
            }

            levelJson = ExtractJsonSession(json, levelInfoStartIndex, json.IndexOf("\"Tiles\":") + 1);
            tilesJson = ExtractJsonSession(json, tilesStartIndex, json.IndexOf("\"Blocks\":") + 1);
            blocksJson = ExtractJsonSession(json, blocksStartIndex, json.IndexOf("\"Mechanics\":") + 1);
            mechanicJson = json[mechanicsStartIndex..].TrimEnd('}');

            // Debug.Log(levelJson);
            // Debug.Log(tilesJson);
            // Debug.Log(blocksJson);
            // Debug.Log(mechanicJson);
        }

        private static string ExtractJsonSession(string json, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex - 2;
            return json.Substring(startIndex, length).TrimEnd(',').Trim();
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
                var lastDir = PlayerPrefs.GetString(_lastDirectory, "");

                var extensions = new[] {
                    new ExtensionFilter("Data Files", "dat")
                };

                StandaloneFileBrowser.SaveFilePanelAsync(
                    "Save Files",
                    lastDir,
                    $"{LevelManagerMenu.Instance.LevelId}.dat",
                    extensions,
                    (string path) =>
                    {
                        if (path.Length > 0 && !string.IsNullOrEmpty(path))
                        {
                            File.WriteAllText(path, ConvertLevelToJson());
                            lastDir = Path.GetDirectoryName(path);
                            PlayerPrefs.SetString(_lastDirectory, lastDir);
                            Debug.Log($"File saved at: {path}");
                        }
                        else
                        {
                            Debug.Log("Save canceled.");
                        }
                    }
                );
            }
        }

        public static void LoadSavedFile()
        {
            var lastDir = PlayerPrefs.GetString(_lastDirectory, "");

            var extensions = new[]
            {
                new ExtensionFilter("Data Files", "dat")
            };

            StandaloneFileBrowser.OpenFilePanelAsync(
                "Open File",
                lastDir,
                extensions,
                false,
                (string[] path) =>
                {
                    if(path.Length > 0 && !string.IsNullOrEmpty(path[0]))
                    {
                        var json = File.ReadAllText(path[0]);
                        ConvertJsonToLevel(json);

                        lastDir = Path.GetDirectoryName(path[0]);
                        PlayerPrefs.SetString(_lastDirectory, lastDir);
                    }
                    else
                    {
                        Debug.Log("Cannot open file.");
                    }
                }
            );

        }
    }
}
