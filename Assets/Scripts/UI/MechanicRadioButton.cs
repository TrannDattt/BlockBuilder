using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuilderTool.LevelEditor
{
    public class MechanicRadioButton : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public EMechanic Key {get; private set;}

        [SerializeField] private Image _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private CanvasGroup _content;

        public bool IsActivated {get; private set;}

        private Color _labelDeactiveColor = new(1, 1, 1, .5f);
        private Color _labelActiveColor = new(1, 1, 1, 1);

        private ICanHaveMechanic _selectedObject;

        public Dictionary<EDirection, AMechanic> SavedMechanic {get; private set;} = new Dictionary<EDirection, AMechanic>(){
            { EDirection.None, null},
            { EDirection.Up, null},
            { EDirection.Down, null},
            { EDirection.Left, null},
            { EDirection.Right, null},
        };
        private MechanicUIGroup _mechanicGroup;

        public event Action<EMechanic, bool> OnButtonClicked;

        public void SetSelectedObject(ICanHaveMechanic obj, bool resetMechanic){
            var lastObj = _selectedObject;
            _selectedObject = obj;
            if(resetMechanic || lastObj != obj)
            {
                ResetSavedMechanic();
                Debug.Log(1);
            }
        }

        public void ActivateContent(){
            IsActivated = true;

            _button.gameObject.SetActive(true);
            _label.color = _labelActiveColor;
            _content.interactable = true;
        }

        public void DeactivateContent(){
            IsActivated = false;
            
            _button.gameObject.SetActive(false);
            _label.color = _labelDeactiveColor;
            _content.interactable = false;
        }

        public void UpdateMechanicDisplay(AMechanic mechanic){
            if(mechanic == null){
                Debug.LogError("No mechanic to display");
            }

            SavedMechanic[mechanic.Dir] = mechanic;
            _mechanicGroup?.UpdateMechanicDisplay(mechanic, mechanic.Dir);
        }

        public void AssignMechanicToObject(AMechanic mechanic, EDirection dir){
            // Debug.Log(mechanic);
            SavedMechanic[dir] = mechanic;
            EditorField.Instance.UpdateMechanic(mechanic, _selectedObject, dir);
            MechanicIconPooling.Instance.UpdateIcon(mechanic, _selectedObject, dir);
        }

        public void AssignSavedMechanicToObject(){
            var keys = SavedMechanic.Keys.ToList();
            foreach(var dir in keys){
                AssignMechanicToObject(SavedMechanic[dir], dir);
            }
        }

        public void ResetSavedMechanic(){
            var keyList = SavedMechanic.Keys.ToList();
            foreach (var key in keyList)
            {
                SavedMechanic[key] = null;
            }
            _mechanicGroup?.ResetMechanicDisplay();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsActivated)
            {
                ActivateContent();
                AssignSavedMechanicToObject();
            }
            else{
                DeactivateContent();
                ResetSavedMechanic();
                AssignSavedMechanicToObject();
            }

            OnButtonClicked?.Invoke(Key, !IsActivated);
        }

        void Awake()
        {
            _mechanicGroup = _content.GetComponentInChildren<MechanicUIGroup>();
        }
    }
}
