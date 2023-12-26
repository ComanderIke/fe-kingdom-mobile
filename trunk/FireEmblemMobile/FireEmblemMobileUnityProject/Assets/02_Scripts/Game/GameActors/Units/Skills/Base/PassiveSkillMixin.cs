using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
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