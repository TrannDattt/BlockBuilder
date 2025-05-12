using System;
using System.Collections.Generic;
using System.Linq;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using BuilderTool.Mechanic;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class MechanicRadioButtonGroup : MonoBehaviour{
        [SerializeField] private List<MechanicRadioButton> _buttonList;

        public MechanicRadioButton ActiveButton {get; private set;}
        // public event Action<AMechanic, ICanHaveMechanic> OnMechanicUpdated;

        /// <summary>
        /// Update activated button and deactivate all other button
        /// </summary>
        /// <param name="buttonKey">Key of the button</param>
        /// <param name="state">Toggle state of the button</param>
        public void ChangeActivatedButton(EMechanic buttonKey, bool state){
            foreach (var btn in _buttonList){
                if(btn.Key == buttonKey){
                    ActiveButton = state ? btn : null;
                    // btn.ActivateContent();
                }
                else{
                    btn.DeactivateContent();
                }
            }
        }

        public void LoadMechanicData(AMechanic mechanic, ICanHaveMechanic obj){
            var button = _buttonList.FirstOrDefault(btn => btn.Key == mechanic.Type);
            button.LoadMechanicData(mechanic, obj);
        }

        /// <summary>
        /// Update the displaying of group buttons whenever a block/tile is selected
        /// </summary>
        /// <param name="mechanic">Mechanic that the object is holding</param>
        public void UpdateMechanicButtonDisplay(AMechanic mechanic){
            if(mechanic == null){
                ResetButtonGroupDisplay();
                return;
            }

            ChangeActivatedButton(mechanic.Type, true);
            if(ActiveButton != null && ActiveButton.SavedMechanic[mechanic.Dir] != mechanic){
                // Debug.Log(2);
                ResetButtonGroupDisplay(false);
            }

            ActiveButton.ActivateContent();
            ActiveButton.UpdateContentDisplay(mechanic);
        }

        /// <summary>
        /// Return the displaying of group button to default 
        /// if the selected tile/block has no mechanic or no longer available
        /// </summary>
        /// <param name="resetAll">If the buttons to reset include the active button</param>
        public void ResetButtonGroupDisplay(bool resetAll = true) 
        {
            foreach(var button in _buttonList){
                if(ActiveButton != null && button == ActiveButton && !resetAll){
                    continue;
                }

                button.ResetContentDisplay();
                button.DeactivateContent();
            }
        }

        public void RemoveMechanic(AMechanic mechanic, ICanHaveMechanic obj){
            if(mechanic != null){
                var button = _buttonList.FirstOrDefault(btn => btn.Key == mechanic.Type);
                button.ResetMechanicData(obj);
                button.DeactivateContent();
            }
        }

        private void InitAllButtons(){
            foreach(var button in _buttonList){
                button.DeactivateContent();
                ActiveButton = null;
                button.OnButtonClicked += ChangeActivatedButton;
            }
        }

        void OnEnable()
        {
            InitAllButtons();
            // OnMechanicUpdated += 
        }

        void OnDisable()
        {
            if(ActiveButton != null){
                ActiveButton = null;
            }

        }

        void OnDestroy()
        {
            
        }
    }
}
