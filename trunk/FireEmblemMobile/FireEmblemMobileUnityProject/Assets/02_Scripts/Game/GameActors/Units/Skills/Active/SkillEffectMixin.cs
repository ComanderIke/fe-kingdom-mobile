using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillEffectMixin:ScriptableObject
    {
        public abstract void Activate(Unit target, Unit caster, int level);
        public abstract void Activate(Unit target, int level);
        public abstract void Activate(Tile target, int level);
        public abstract void Activate(List<Unit> targets, int level);



    }
}