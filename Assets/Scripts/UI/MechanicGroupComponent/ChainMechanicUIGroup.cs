using System;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using BuilderTool.LevelEditor;
using TMPro;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public class ChainMechanicUIGroup : MechanicUIGroup{
        [Header("Block Chain")]
        [SerializeField] private TMP_InputField _blockChainTurn;

        [Header("Door Chain")]
        [SerializeField] private TMP_InputField _upDoorChainTurn;
        [SerializeField] private TMP_InputField _downDoorChainTurn;
        [SerializeField] private TMP_InputField _leftDoorChainTurn;
        [SerializeField] private TMP_InputField _rightDoorChainTurn;
        
        public override void UpdateMechanicDisplay(AMechanic mechanic, EDirection dir)
        {
            switch(dir){
                case EDirection.None:
                    UpdateTextFieldDisplay(_blockChainTurn, mechanic as Chain);
                    break;

                case EDirection.Up:
                    UpdateTextFieldDisplay(_upDoorChainTurn, mechanic as Chain);
                    break;

                case EDirection.Down:
                    UpdateTextFieldDisplay(_downDoorChainTurn, mechanic as Chain);
                    break;

                case EDirection.Left:
                    UpdateTextFieldDisplay(_leftDoorChainTurn, mechanic as Chain);
                    break;

                case EDirection.Right:
                    UpdateTextFieldDisplay(_rightDoorChainTurn, mechanic as Chain);
                    break;
            }
        }

        private void UpdateTextFieldDisplay(TMP_InputField inputField, Chain mechanic){
            if(mechanic == null){
                inputField.text = "0";
                return;
            }

            inputField.text = mechanic.TurnCount.ToString();
        }

        public override void ResetMechanicDisplay()
        {
            if(_blockChainTurn != null)
            {
                _blockChainTurn.text = "0";
            }
            else{
                _upDoorChainTurn.text = "0";
                _downDoorChainTurn.text = "0";
                _leftDoorChainTurn.text = "0";
                _rightDoorChainTurn.text = "0";
            }
        }

        private Tuple<EDirection, Chain> GetMechanicFromUI(TMP_InputField inputField, EDirection dir){
            if (int.TryParse(inputField.text, out int turnCount) && turnCount > 0)
            {
                return new(dir, new Chain(EMechanic.Chained, dir, turnCount));
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

            AssignListener(_blockChainTurn, EDirection.None);
            AssignListener(_upDoorChainTurn, EDirection.Up);
            AssignListener(_downDoorChainTurn, EDirection.Down);
            AssignListener(_leftDoorChainTurn, EDirection.Left);
            AssignListener(_rightDoorChainTurn, EDirection.Right);
        }
    }
}
