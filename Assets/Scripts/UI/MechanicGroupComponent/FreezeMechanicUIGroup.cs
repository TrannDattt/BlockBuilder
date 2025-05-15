using System;
using System.Collections.Generic;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using BuilderTool.LevelEditor;
using TMPro;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public class FreezeMechanicUIGroup : MechanicUIGroup
    {
        [Header("Block Freeze")]
        [SerializeField] private TMP_InputField _blockFreezeTurn;

        [Header("Door Freeze")]
        [SerializeField] private TMP_InputField _upDoorFreezeTurn;
        [SerializeField] private TMP_InputField _downDoorFreezeTurn;
        [SerializeField] private TMP_InputField _leftDoorFreezeTurn;
        [SerializeField] private TMP_InputField _rightDoorFreezeTurn;
        
        public override void UpdateMechanicDisplay(AMechanic mechanic, EDirection dir)
        {
            switch(dir){
                case EDirection.None:
                    UpdateTextFieldDisplay(_blockFreezeTurn, mechanic as Freeze);
                    break;

                case EDirection.Up:
                    UpdateTextFieldDisplay(_upDoorFreezeTurn, mechanic as Freeze);
                    break;

                case EDirection.Down:
                    UpdateTextFieldDisplay(_downDoorFreezeTurn, mechanic as Freeze);
                    break;

                case EDirection.Left:
                    UpdateTextFieldDisplay(_leftDoorFreezeTurn, mechanic as Freeze);
                    break;

                case EDirection.Right:
                    UpdateTextFieldDisplay(_rightDoorFreezeTurn, mechanic as Freeze);
                    break;
            }
        }

        private void UpdateTextFieldDisplay(TMP_InputField inputField, Freeze mechanic){
            if(mechanic == null){
                inputField.text = "0";
                return;
            }

            inputField.text = mechanic.TurnCount.ToString();
        }

        public override void ResetMechanicDisplay()
        {
            if(_blockFreezeTurn != null)
            {
                _blockFreezeTurn.text = "0";
            }
            else{
                _upDoorFreezeTurn.text = "0";
                _downDoorFreezeTurn.text = "0";
                _leftDoorFreezeTurn.text = "0";
                _rightDoorFreezeTurn.text = "0";
            }
        }

        private Tuple<EDirection, Freeze> GetMechanicFromUI(TMP_InputField inputField, EDirection dir){
            if (int.TryParse(inputField.text, out int turnCount) && turnCount > 0)
            {
                return new(dir, new Freeze(EMechanic.Freeze, dir, turnCount));
            }

            return new(dir, null);
        }

        private void AssignListener(TMP_InputField inputField, EDirection dir){
            if(inputField == null){
                return;
            }
            
            inputField.onValueChanged.AddListener(_ =>
            {
                var mechanic = GetMechanicFromUI(inputField, dir);
                _button.AssignMechanicToObject(mechanic.Item2, mechanic.Item1);
            });
        }

        protected override void Awake()
        {
            base.Awake();

            AssignListener(_blockFreezeTurn, EDirection.None);
            AssignListener(_upDoorFreezeTurn, EDirection.Up);
            AssignListener(_downDoorFreezeTurn, EDirection.Down);
            AssignListener(_leftDoorFreezeTurn, EDirection.Left);
            AssignListener(_rightDoorFreezeTurn, EDirection.Right);
        }
    }
}
