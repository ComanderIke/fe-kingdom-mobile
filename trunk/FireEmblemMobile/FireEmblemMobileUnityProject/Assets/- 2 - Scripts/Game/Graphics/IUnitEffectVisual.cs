using Game.GameActors.Units;
using UnityEngine;

namespace Game.Graphics
{
    public abstract class IUnitEffectVisual : ScriptableObject
    {
        public abstract void ShowAttackable(IGridActor gridActor);

        public abstract void HideAttackable();
    }
}