using UnityEngine;

namespace BuilderTool.Interfaces
{
    public interface IImmoblized : IMechanicCore
    {

        public void ApplyImmoblized(ICanHaveMechanic obj)
        {
            if(obj.GetObject().TryGetComponent(out Rigidbody2D body)){
                body.constraints = RigidbodyConstraints2D.FreezePosition;
            }
        }

        public bool CheckCanRemoveImmoblized(bool isInMechanic, int turnCount)
        {
            return isInMechanic && LevelManager.Instance.Move >= turnCount;
        }

        public void RemoveImmoblized(ICanHaveMechanic obj, RigidbodyConstraints2D preConstraint)
        {
            if(obj.GetObject().TryGetComponent(out Rigidbody2D body)){
                body.constraints = preConstraint;
            }
        }
    }
}
