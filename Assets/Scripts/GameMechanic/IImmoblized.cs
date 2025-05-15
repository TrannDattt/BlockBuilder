using UnityEngine;

namespace BuilderTool.Interfaces
{
    public interface IImmoblized : IMechanicCore
    {
        public int TurnCount {get; set;}
        public RigidbodyConstraints PreConstraint {get; set;}

        public void ApplyImmoblized(ICanHaveMechanic obj)
        {
            if(obj.GetObject().TryGetComponent(out Rigidbody body)){
                PreConstraint = body.constraints;
                body.constraints = RigidbodyConstraints.FreezePosition;
            }
        }

        public bool CheckCanRemoveImmoblized()
        {
            return LevelManager.Instance.Move >= TurnCount;
        }

        public void RemoveImmoblized(ICanHaveMechanic obj)
        {
            if(obj.GetObject().TryGetComponent(out Rigidbody body)){
                body.constraints = PreConstraint;
            }
        }
    }
}
