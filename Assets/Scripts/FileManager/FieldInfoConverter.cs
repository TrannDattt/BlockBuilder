using BuilderTool.Helpers;
using BuilderTool.LevelEditor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;

namespace BuilderTool.FileConvert
{
    public static class FieldInfoConverter
    {
        public static string ConvertFieldToJson()
        {
            string output = "";
            foreach(var tile in EditorField.Instance.Tiles)
            {
                
                //output += JsonUtility.ToJson(tile);
            }
            Debug.Log(output);
            return output;
        }
    }
}
