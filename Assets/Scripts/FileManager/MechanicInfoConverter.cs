using System;
using System.Collections.Generic;
using System.Linq;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using BuilderTool.LevelEditor;
using BuilderTool.Mechanic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace BuilderTool.FileConvert
{
    public static class MechanicInfoConverter{
        [Serializable]
        public class MechanicData{
            public EMechanic MechanicType;
            public EDirection Direction;
            public int PosIndex;

            public MechanicData(AMechanic mechanic, ICanHaveMechanic obj){
                MechanicType = mechanic.Type;
                Direction = mechanic.Dir;
                
                var field = EditorField.Instance;
                var holder = obj.GetObject();
                if(holder.TryGetComponent(out EditorBlock block))
                {
                    if (field.BlockDict.ContainsKey(block))
                    {
                        PosIndex = field.Tiles.IndexOf(field.BlockDict[block]); 
                    }
                    else
                    {
                        PosIndex = -1;
                        Debug.LogError($"Field does not contain {block}.");
                        return;
                    }
                }
                else{
                    var tile = holder.GetComponentInParent<EditorTile>();
                    PosIndex = EditorField.Instance.Tiles.IndexOf(tile);
                }
            }
        }

        [Serializable]
        public class FreezeMechanicData : MechanicData{
            public int TurnCount;

            public FreezeMechanicData(AMechanic mechanic, ICanHaveMechanic obj) : base(mechanic, obj)
            {
                var freeze = mechanic as Freeze;
                TurnCount = freeze.TurnCount;
            }
        }

        [Serializable]
        public class ChainMechanicData : MechanicData{
            public int TurnCount;

            public ChainMechanicData(AMechanic mechanic, ICanHaveMechanic obj) : base(mechanic, obj)
            {
                var chain = mechanic as Chain;
                TurnCount = chain.TurnCount;
            }
        }

        public static string ConvertMechanicToJson(){
            string json = "";
            var mechanicDict = EditorField.Instance.MechanicDict;
            var keys = mechanicDict.Keys.ToList();

            foreach(var key in keys){
                if(mechanicDict[key] != null){
                    json += mechanicDict[key].Type switch
                    {
                        EMechanic.Freeze => JsonUtility.ToJson(new FreezeMechanicData(mechanicDict[key], key.Item1)),
                        EMechanic.Chained => JsonUtility.ToJson(new ChainMechanicData(mechanicDict[key], key.Item1)),
                        _ => ""
                    };

                    json += keys.IndexOf(key) != keys.Count - 1 ? "," : "";
                }
            }

            Debug.Log(json);
            return json;
        }

        public static void ConvertJsonToMechanic(string json){
            Debug.Log(json);
            JArray mechanicArray = JArray.Parse(json);

            List<string> freezeMechanicJsons = new();
            List<string> chainMechanicJsons = new();

            foreach(var mechanic in mechanicArray)
            {
                int mechanicType = mechanic["MechanicType"]?.Value<int>() ?? -1;
                var mechanicJson = mechanic.ToString(Newtonsoft.Json.Formatting.None);

                switch(mechanicType){
                    case 1: // Freeze
                        freezeMechanicJsons.Add(mechanicJson);
                        break;
                    
                    case 2: // Chain
                        chainMechanicJsons.Add(mechanicJson);
                        break;
                }
            }

            List<FreezeMechanicData> freezeMechanicDatas = ConvertJsonToList<FreezeMechanicData>(freezeMechanicJsons);
            List<ChainMechanicData> chainMechanicDatas = ConvertJsonToList<ChainMechanicData>(chainMechanicJsons);

            List<MechanicData> freezeAsBase = freezeMechanicDatas.Cast<MechanicData>().ToList();
            List<MechanicData> chainAsBase = chainMechanicDatas.Cast<MechanicData>().ToList();

            EditorField.Instance.UpdateMechanicData(freezeAsBase, chainAsBase);
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
