using UnityEngine;

namespace BuilderTool.Interfaces
{
    public interface IImmoblized : IMechanicCore
    {
        public int TurnCount {get; set;}
        public RigidbodyConstraints2D PreConstraint {get; set;}

        public void ApplyImmoblized(ICanHaveMechanic obj)
        {
            if(obj.GetObject().TryGetComponent(out Rigidbody2D body)){
                PreConstraint = body.constraints;
                body.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }

        public bool CheckCanRemoveImmoblized()
        {
            return LevelManager.Instance.Move >= TurnCount;
        }

        public void RemoveImmoblized(ICanHaveMechanic obj)
        {
            if(obj.GetObject().TryGetComponent(out Rigidbody2D body)){
                body.constraints = PreConstraint;
            }
        }
    }
}
