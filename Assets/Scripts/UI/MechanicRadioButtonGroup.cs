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

        private MechanicRadioButton GetButtonByKey(EMechanic key){
            return _buttonList.FirstOrDefault(btn => btn.Key == key);
        }

        /// <summary>
        /// Update activated button and deactivate all other button
        /// </summary>
        /// <param name="buttonKey">Key of the button</param>
        /// <param name="state">Toggle state of the button</param>
        public void ChangeActivatedButton(EMechanic buttonKey, bool state){
            foreach (var btn in _buttonList){
                if(btn.Key != buttonKey){
                    btn.DeactivateContent();
                }
            }
        }

        // TODO: When set more then one mechanic to tile, then click to an empty tile and click back, it lost the mechanic
        public void UpdateMechanicDisplay(AMechanic mechanic, ICanHaveMechanic obj){
            Debug.Log(mechanic);

            _buttonList.ForEach(btn => {
                btn.SetSelectedObject(obj, false);
                btn.DeactivateContent();
            });

            if(mechanic == null){
                _buttonList.ForEach(btn => btn.ResetSavedMechanic());
                return;
            }

            _buttonList.ForEach(btn => {
                if(btn.Key != mechanic.Type){
                    btn.ResetSavedMechanic();
                    Debug.Log(2);
                }
            });

            var button = GetButtonByKey(mechanic.Type);
            ChangeActivatedButton(mechanic.Type, true);
            button.ActivateContent();
            button.UpdateMechanicDisplay(mechanic);
        }

        public void AssignMechanicToObject(AMechanic mechanic, ICanHaveMechanic obj){
            if(mechanic != null)
            {
                var button = GetButtonByKey(mechanic.Type);
                button.SetSelectedObject(obj, false);
                button.AssignMechanicToObject(mechanic, mechanic.Dir);
            }
            else{
                _buttonList.ForEach(btn => {
                    btn.SetSelectedObject(obj, true);
                    btn.AssignSavedMechanicToObject();
                    // btn.ResetSavedMechanic();
                });
            }
        }

        private void InitAllButtons(){
            foreach(var button in _buttonList){
                button.DeactivateContent();
                button.OnButtonClicked += ChangeActivatedButton;
            }
        }

        void OnEnable()
        {
            InitAllButtons();
        }

        void OnDestroy()
        {
            foreach(var button in _buttonList){
                // button.DeactivateContent();
                button.OnButtonClicked -= ChangeActivatedButton;
            }
        }
    }
}
