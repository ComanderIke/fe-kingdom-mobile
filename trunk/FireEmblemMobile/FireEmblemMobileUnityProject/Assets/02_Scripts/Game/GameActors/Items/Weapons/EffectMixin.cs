using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public abstract class EffectMixin : ScriptableObject
    {
        public abstract string GetDescription();
    }
    
}