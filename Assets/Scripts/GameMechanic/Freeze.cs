using BuilderTool.Enums;
using BuilderTool.Interfaces;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    // [CreateAssetMenu(menuName = "Scriptable Object/Mechanic/Freeze")]
    public class Freeze : AMechanic, IImmoblized
    {
        // int IImmoblized.TurnCount { get; set; }
        // bool IImmoblized.IsImmoblized { get; set; }
        // RigidbodyConstraints2D IImmoblized.PreConstraint { get; set; }
        
        public Freeze(EMechanic type, int turnCount) : base(type)
        {
            // TurnCount = turnCount;
        }


        public override void Do()
        {
            throw new System.NotImplementedException();
        }

        public override void OnApplying()
        {
            throw new System.NotImplementedException();
        }

        public override void OnRemoving()
        {
            throw new System.NotImplementedException();
        }
    }
}
