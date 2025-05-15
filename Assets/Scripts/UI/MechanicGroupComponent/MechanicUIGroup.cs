using System;
using System.Collections.Generic;
using BuilderTool.Enums;
using BuilderTool.Interfaces;
using BuilderTool.LevelEditor;
using BuilderTool.Mechanic;
using UnityEngine;

namespace BuilderTool.Mechanic
{
    public abstract class MechanicUIGroup : MonoBehaviour{
        protected MechanicRadioButton _button;

        public abstract void UpdateMechanicDisplay(AMechanic mechanic, EDirection dir);
        public abstract void ResetMechanicDisplay();

        // public abstract void LoadMechanicData(AMechanic mechanic, ICanHaveMechanic obj);
        // public abstract void ResetMechanicData(ICanHaveMechanic obj);

        protected virtual void Awake()
        {
            _button = GetComponentInParent<MechanicRadioButton>();
        }
    }
}
