using System;
using System.Collections.Generic;
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

        public Dictionary<EDirection, AMechanic> SavedMechanic {get; private set;} = new Dictionary<EDirection, AMechanic>(){
            { EDirection.None, null},
            { EDirection.Up, null},
            { EDirection.Down, null},
            { EDirection.Left, null},
            { EDirection.Right, null},
        };
        private MechanicUIGroup _mechanicGroup;

        public event Action<EMechanic, bool> OnButtonClicked;

        /// <summary>
        /// Call when this button get activated, to show content of this button
        /// </summary>
        public void ActivateContent(){
            IsActivated = true;
            // OnButtonClicked?.Invoke(Key, IsActivated);

            _button.gameObject.SetActive(true);
            _label.color = _labelActiveColor;
            _content.interactable = true;
        }

        /// <summary>
        /// Call when this button get deactivated, to hide content of this button
        /// </summary>
        public void DeactivateContent(){
            IsActivated = false;
            // OnButtonClicked?.Invoke(Key, IsActivated);
            
            _button.gameObject.SetActive(false);
            _label.color = _labelDeactiveColor;
            _content.interactable = false;
        }
        
        /// <summary>
        /// Display data of the mechanic that the selected block/tile is holding
        /// </summary>
        /// <param name="mechanic"></param>
        public void UpdateContentDisplay(AMechanic mechanic){
            SavedMechanic[mechanic.Dir] = mechanic;
            _mechanicGroup?.UpdateMechanicDisplay(mechanic);
        }

        /// <summary>
        /// Return the displaying of this button to default
        /// </summary>
        public void ResetContentDisplay(){
            var keyList = SavedMechanic.Keys.ToList();
            foreach (var key in keyList)
            {
                SavedMechanic[key] = null;
            }
            _mechanicGroup?.ResetMechanicDisplay();
        }

        // public void ResetDirectionalContentDisplay(EDirection dir){
        //     SavedMechanic = new Dictionary<EDirection, AMechanic>(){
        //         { EDirection.None, null},
        //         { EDirection.Up, null},
        //         { EDirection.Down, null},
        //         { EDirection.Left, null},
        //         { EDirection.Right, null},
        //     };
        //     _mechanicGroup?.ResetMechanicDisplay();
        // }

        public void LoadMechanicData(AMechanic mechanic, ICanHaveMechanic obj){
            _mechanicGroup?.LoadMechanicData(mechanic, obj);
        }

        /// <summary>
        /// To load the mechanic saved previously when user reselect this button 
        /// after select another button
        /// </summary>
        private void LoadSavedMechanicData(){
            var keyList = SavedMechanic.Keys.ToList();
            foreach(var dir in keyList)
            {
                if(dir == EDirection.None)
                    {
                        if(SavedMechanic[dir] != null){
                        LoadMechanicData(SavedMechanic[dir], BlockSelectHandler.Instance.SelectedBlock);
                    }
                    else{
                        ResetMechanicData(BlockSelectHandler.Instance.SelectedBlock);
                    }
                }
                else{
                    // TODO: 
                }
            }
        }

        /// <summary>
        /// To apply mechanic to selected block/tile whenever content of this button is changed
        /// </summary>
        /// <param name="mechanic">Mechanic to apply</param>
        /// <param name="obj">Object to be applied</param>
        public void UpdateMechanicData(AMechanic mechanic, ICanHaveMechanic obj, EDirection dir){
            SavedMechanic[dir] = mechanic;
            EditorField.Instance.UpdateMechanic(mechanic, obj, dir);
            MechanicIconPooling.Instance.UpdateIcon(mechanic, obj, dir);
        }

        /// <summary>
        /// To remove mechanic from selected block/tile whenever user actively deselect this button
        /// and return the displaying to default
        /// </summary>
        public void ResetMechanicData(ICanHaveMechanic obj){
            _mechanicGroup.ResetMechanicData(obj);
            ResetContentDisplay();
        }

        // public void ResetDirectionalMechanicData(EDirection dir){

        // }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnButtonClicked?.Invoke(Key, !IsActivated);

            if(!IsActivated)
            {
                ActivateContent();
                LoadSavedMechanicData();
            }
            else{
                DeactivateContent();
                ResetMechanicData(BlockSelectHandler.Instance.SelectedBlock);

                var keyList = SavedMechanic.Keys.ToList();
                foreach (var key in keyList)
                {
                    SavedMechanic[key] = null;
                }
            }
        }

        void Awake()
        {
            _mechanicGroup = _content.GetComponentInChildren<MechanicUIGroup>();


        }

        void OnDestroy()
        {
            
        }
    }
}
