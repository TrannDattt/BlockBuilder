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

        public override void UpdateMechanicDisplay(AMechanic mechanic)
        {
            var chainMechanic = mechanic as Chain;
            switch(chainMechanic.Dir){
                case Enums.EDirection.None:
                    _blockChainTurn.text = chainMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Up:
                    _upDoorChainTurn.text = chainMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Down:
                    _downDoorChainTurn.text = chainMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Left:
                    _leftDoorChainTurn.text = chainMechanic.TurnCount.ToString();
                    break;

                case Enums.EDirection.Right:
                    _rightDoorChainTurn.text = chainMechanic.TurnCount.ToString();
                    break;
            }
        }

        public override void LoadMechanicData(AMechanic mechanic, ICanHaveMechanic obj)
        {
            var chain = mechanic as Chain;
            // if(chain == null){
            //     ResetMechanicData();
            // }

            if(_blockChainTurn != null)
            {
                _button.UpdateMechanicData(new Chain(EMechanic.Chained, chain.Dir, chain.TurnCount), obj, EDirection.None);
            }
            else{
                _button.UpdateMechanicData(new Chain(EMechanic.Chained, chain.Dir, chain.TurnCount), obj, chain.Dir);
            }
        }

        public override void ResetMechanicData(ICanHaveMechanic obj)
        {
            // ResetMechanicDisplay();

            var holder = obj.GetObject();

            if(holder.TryGetComponent(out EditorBlock block))
            {
                _button.UpdateMechanicData(null, block, EDirection.None);
            }
            else{
                // var tiles = TileSelectHandler.Instance.SelectedTiles;
                // tiles.ForEach(
                //     tile => {
                //         var door = tile.CurTileAttribute as DoorTile;
                //         _button.UpdateMechanicData(null, door, dir);
                //     }
                // );
            }
        }

        private void UpdateMechanicData(int chainTurn, EDirection dir){
            if(chainTurn > 0)
            {
                if(_blockChainTurn != null)
                {
                    var block = BlockSelectHandler.Instance.SelectedBlock;
                    _button.UpdateMechanicData(new Chain(EMechanic.Chained, dir, chainTurn), block, EDirection.None);
                }
                else{
                    var tiles = TileSelectHandler.Instance.SelectedTiles;
                    tiles.ForEach(
                        tile => {
                            var door = tile.CurTileAttribute as DoorTile;
                            _button.UpdateMechanicData(new Chain(EMechanic.Chained, dir, chainTurn), door, dir);
                        }
                    );
                }
            }
            else{
                if(_blockChainTurn != null)
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
                if (int.TryParse(value, out int chainTurn))
                {
                    UpdateMechanicData(chainTurn, dir);
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

            AssignListener(_blockChainTurn, EDirection.None);
            AssignListener(_upDoorChainTurn, EDirection.Up);
            AssignListener(_downDoorChainTurn, EDirection.Down);
            AssignListener(_leftDoorChainTurn, EDirection.Left);
            AssignListener(_rightDoorChainTurn, EDirection.Right);
        }
    }
}
