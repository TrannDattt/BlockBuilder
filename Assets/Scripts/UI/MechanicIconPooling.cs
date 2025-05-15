using System;
using System.Collections.Generic;
using BuilderTool.Enums;
using BuilderTool.Helpers;
using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

namespace BuilderTool.LevelEditor
{
    public class MechanicIconPooling : Singleton<MechanicIconPooling>{
        [SerializeField] private Canvas _spawnCanvas;
        [SerializeField] private Transform _spawnTransform;

        [Header("Icons")]
        [SerializeField] private MechanicIcon _iconPreb;
        [SerializeField] private Sprite _freezeIcon;
        [SerializeField] private Sprite _chainIcon;

        private Queue<MechanicIcon> _iconQueue = new();
        private Dictionary<Tuple<ICanHaveMechanic, EDirection>, MechanicIcon> _iconDict = new();
        private Camera _mainCam;

        public void ShowIcons(){
            _spawnTransform.gameObject.SetActive(true);
        }

        public void HideIcons(){
            _spawnTransform.gameObject.SetActive(false);
        }

        public void UpdateIcon(AMechanic mechanic, ICanHaveMechanic obj, EDirection dir){
            var key = new Tuple<ICanHaveMechanic, EDirection>(obj, dir);
            if(mechanic == null){
                ReturnIcon(key);
                return;
            }

            Sprite icon = GetSpawnedIcon(mechanic.Type);

            if(icon != null)
            {
                var canvasPos = GetCanvasPosFromWorld(obj.GetObject().transform.position);

                if(_iconQueue.Count == 0 && !_iconDict.ContainsKey(key)){
                    var newIcon = Instantiate(_iconPreb, _spawnTransform);
                    newIcon.InitIcon(icon, mechanic, obj);
                    UpdateDict(key, newIcon);
                    // return newIcon;
                    return;
                }

                var spawnedIcon = !_iconDict.ContainsKey(key) ? _iconQueue.Dequeue() : _iconDict[key];
                spawnedIcon.InitIcon(icon, mechanic, obj);
                UpdateDict(key, spawnedIcon);
                // return spawnedIcon;
            }

            // return null;
        }

        private void UpdateDict(Tuple<ICanHaveMechanic, EDirection> key, MechanicIcon value){
            if(!_iconDict.ContainsKey(key)){
                _iconDict.Add(key, value);
                return;
            }
            
            if(_iconDict[key] == value){
                return;
            }

            _iconDict[key] = value;
        }

        public void ReturnIcon(Tuple<ICanHaveMechanic, EDirection> key){
            if(_iconDict.ContainsKey(key))
            {
                var icon = _iconDict[key];
                _iconQueue.Enqueue(icon);
                icon.ReturnToPool();
                _iconDict.Remove(key);
            }
        }

        private Sprite GetSpawnedIcon(EMechanic mechanic){
            return mechanic switch
            {
                EMechanic.Freeze => _freezeIcon,
                EMechanic.Chained => _chainIcon,
                _ => null,
            };
        }

        public Vector2 GetCanvasPosFromWorld(Vector3 worldPos){
            var screenPos = _mainCam.WorldToScreenPoint(worldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _spawnTransform.GetComponent<RectTransform>(),
                screenPos,
                null,
                out var canvasPos
            );

            return canvasPos;
        }

        void OnEnable()
        {
            _mainCam = Camera.main;
        }
    }
}
