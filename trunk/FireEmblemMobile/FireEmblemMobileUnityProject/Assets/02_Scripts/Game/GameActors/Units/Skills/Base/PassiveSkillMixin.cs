using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Base
{
   

    public abstract class PassiveSkillMixin:SkillMixin
    {
        [SerializeField] public bool toogleAble;
        [HideInInspector]public bool toggledOn=true;
        public event Action<bool> OnToggle;

        public void Toggle()
        {
            Debug.Log("TOGGLE SKILL");
            toggledOn = !toggledOn;
            OnToggle?.Invoke(toggledOn);
        }
    }
}