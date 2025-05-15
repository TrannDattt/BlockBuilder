using BuilderTool.Interfaces;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public class MechanicHandler : MonoBehaviour{
        public AMechanic CurMechanic {get; private set;}

        private ICanHaveMechanic _baseObject;

        public void SetMechanic(AMechanic mechanic){
            if(mechanic == null){
                return;
            }

            CurMechanic = mechanic;
            CurMechanic?.OnApplying(_baseObject);

            Debug.Log(gameObject + " " + mechanic.Type);
        }

        void Awake()
        {
            _baseObject = gameObject.GetComponent<ICanHaveMechanic>();
        }

        void FixedUpdate()
        {
            if(CurMechanic != null)
            {
                CurMechanic.Do(_baseObject);

                if(CurMechanic.CheckCanRemoveMechanic()){
                    CurMechanic.OnRemoving(_baseObject);
                }
            }
        }

        void OnDisable()
        {
            CurMechanic?.OnRemoving(_baseObject);
        }
    }
}
