using BuilderTool.Enums;
using BuilderTool.Interfaces;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public class Chain : AMechanic, IImmoblized
    {
        public int TurnCount {get; private set;}
        int IImmoblized.TurnCount { get => TurnCount; set => TurnCount = value; }

        private RigidbodyConstraints2D _preConstraint;
        public RigidbodyConstraints2D PreConstraint { get => _preConstraint; set => _preConstraint = value; }

        public Chain(EMechanic type, EDirection dir, int turnCount) : base(type, dir)
        {
            TurnCount = turnCount;
        }

        public override bool CheckCanRemoveMechanic(){
            return _isInMechanic && (this as IImmoblized).CheckCanRemoveImmoblized();
        } 

        public override void Do(ICanHaveMechanic obj)
        {
            
        }

        public override void OnApplying(ICanHaveMechanic obj)
        {
            base.OnApplying(obj);

            (this as IImmoblized).ApplyImmoblized(obj);
        }

        public override void OnRemoving(ICanHaveMechanic obj)
        {
            base.OnRemoving(obj);

            (this as IImmoblized).RemoveImmoblized(obj);
        }
    }
}
