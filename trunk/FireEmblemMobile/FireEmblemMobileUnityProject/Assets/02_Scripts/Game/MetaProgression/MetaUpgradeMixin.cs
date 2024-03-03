using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.MetaProgression
{
    public abstract class MetaUpgradeMixin : ScriptableObject
    {
        public abstract void Activate(int level);

        public abstract IEnumerable<EffectDescription> GetEffectDescriptions(int level);
    }
}