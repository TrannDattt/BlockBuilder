using System.Collections.Generic;
using BuilderTool.Enums;
using SerializeReferenceEditor;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public abstract class AMechanic{
        [field: SerializeField] public EMechanic Type {get;private set;}
        
        protected bool _isInMechanic;

        public AMechanic(EMechanic type){
            _isInMechanic = false;
            Type = type;
        }

        public abstract void OnApplying();
        public abstract void Do();
        public abstract void OnRemoving();
    }
}
