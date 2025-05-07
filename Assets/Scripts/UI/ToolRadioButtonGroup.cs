using System;
using System.Collections.Generic;
using System.Linq;
using BuilderTool.Enums;
using UnityEngine;

namespace BuilderTool.LevelEditor
{
    public class ToolRadioButtonGroup : MonoBehaviour{
        [SerializeField] private List<ToolRadioButton> _buttonList;

        public ToolRadioButton ActiveButton {get; private set;}
        public event Action<EMechanic> OnValueChanged;

        public void ChangeValue(ToolRadioButton button, bool state){
            _buttonList.Where(btn => btn != button).ToList().ForEach(btn => btn.DeactivateContent());
            
            if(state)
            {
                ActiveButton = button;
                OnValueChanged?.Invoke(button.Key);
            }
            else{
                ActiveButton = null;
                OnValueChanged?.Invoke(EMechanic.None);
            }
        }

        private void InitAllButtons(){
            foreach(var button in _buttonList){
                button.DeactivateContent();
                ActiveButton = null;
                button.OnButtonClicked += ChangeValue;
            }
        }

        void OnEnable()
        {
            InitAllButtons();
        }

        void OnDisable()
        {
            _buttonList.ForEach(btn => btn.OnButtonClicked -= ChangeValue);
        }
    }
}
