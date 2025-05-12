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

        public override void UpdateMechanicDisplay(AMechanic mechanic)
        {
            var freezeMechanic = mechanic as Freeze;
            switch(freezeMechanic.Dir){
                case Enums.EDirection.None:
                    _blockFreezeTurn.text = freezeMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Up:
                    _upDoorFreezeTurn.text = freezeMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Down:
                    _downDoorFreezeTurn.text = freezeMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Left:
                    _leftDoorFreezeTurn.text = freezeMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Right:
                    _rightDoorFreezeTurn.text = freezeMechanic.TurnCount.ToString();
                    break;
            }
        }

        public override void LoadMechanicData(AMechanic mechanic, ICanHaveMechanic obj)
        {
            var freeze = mechanic as Freeze;

            // if(freeze == null){
            //     ResetMechanicData();
            // }

            if(_blockFreezeTurn != null)
            {
                _button.UpdateMechanicData(new Freeze(EMechanic.Freeze, freeze.Dir, freeze.TurnCount), obj, EDirection.None);
            }
            else{
                _button.UpdateMechanicData(new Freeze(EMechanic.Freeze, freeze.Dir, freeze.TurnCount), obj, freeze.Dir);
            }
        }

        public override void ResetMechanicData(ICanHaveMechanic obj)
        {
            var holder = obj.GetObject();

            if(holder.TryGetComponent(out EditorBlock block))
            {
                // Debug.Log(block);
                _button.UpdateMechanicData(null, block, EDirection.None);
            }
            else{
                // var tiles = TileSelectHandler.Instance.SelectedTiles;
                // tiles.ForEach(
                //     tile => {
                //         var door = tile.CurTileAttribute as DoorTile;
                //         button.UpdateMechanicData(null, door, dir);
                //     }
                // );
            }
        }

        private void UpdateMechanicData(int freezeTurn, EDirection dir){
            if(freezeTurn > 0)
            {
                if(_blockFreezeTurn != null)
                {
                    var block = BlockSelectHandler.Instance.SelectedBlock;
                    _button.UpdateMechanicData(new Freeze(EMechanic.Freeze, dir, freezeTurn), block, dir);
                }
                else{
                    var tiles = TileSelectHandler.Instance.SelectedTiles;
                    tiles.ForEach(
                        tile => {
                            var door = tile.CurTileAttribute as DoorTile;
                            _button.UpdateMechanicData(new Freeze(EMechanic.Freeze, dir, freezeTurn), door, dir);
                        }
                    );
                }
            }
            else{
                if(_blockFreezeTurn != null)
                {
                    var block = BlockSelectHandler.Instance.SelectedBlock;
                    _button.UpdateMechanicData(null, block, dir);
                }
                else{
                    var tiles = TileSelectHandler.Instance.SelectedTiles;
                    tiles.ForEach(
                        tile => {
                            var door = tile.CurTileAttribute as DoorTile;
                            _button.UpdateMechanicData(null, door, dir);
                        }
                    );
                }
            }
        }

        private void AssignListener(TMP_InputField inputField, EDirection dir){
            if(inputField == null){
                return;
            }
            
            inputField.onValueChanged.AddListener(value =>
            {
                if (int.TryParse(value, out int freezeTurn))
                {
                    UpdateMechanicData(freezeTurn, dir);
                }
                else
                {
                    UpdateMechanicData(0, dir);
                }
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
