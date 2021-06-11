using System;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.Graphics
{
    [Serializable]
    public abstract class IUnitEffectVisual : ScriptableObject
    {
        
        public abstract void ShowAttackable(IGridActor gridActor);

        public abstract void HideAttackable();
        public abstract void HideNoStamina();
        public abstract void ShowNoStamina(IGridActor actor);
    }
}