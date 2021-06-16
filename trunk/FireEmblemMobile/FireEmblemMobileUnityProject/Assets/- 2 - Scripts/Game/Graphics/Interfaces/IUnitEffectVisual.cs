using System;
using Game.GameActors.Units;

namespace Game.Graphics
{
    [Serializable]
    public abstract class IUnitEffectVisual : SerializeableSO
    {
        
        public abstract void ShowAttackable(IGridActor gridActor);

        public abstract void HideAttackable();
        public abstract void HideNoStamina();
        public abstract void ShowNoStamina(IGridActor actor);
    }
}