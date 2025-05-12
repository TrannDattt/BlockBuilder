using System.Collections.Generic;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using SerializeReferenceEditor;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public abstract class AMechanic{
        public EMechanic Type {get; private set;}
        public EDirection Dir {get; private set;}
        
        protected bool _isInMechanic;

        public AMechanic(EMechanic type, EDirection dir){
            _isInMechanic = false;
            Type = type;
            Dir = dir;
        }

        public virtual void OnApplying(ICanHaveMechanic obj) => _isInMechanic = true;
        public abstract void Do(ICanHaveMechanic obj);
        public virtual void OnRemoving(ICanHaveMechanic obj) => _isInMechanic = false;
        public abstract bool CheckCanRemoveMechanic();
    }
}
