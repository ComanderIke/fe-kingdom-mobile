﻿using System.Collections.Generic;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillEffectMixin:ScriptableObject
    {
        public abstract List<EffectDescription> GetEffectDescription(Unit caster, int level);

    }
    public abstract class SelfTargetSkillEffectMixin : SkillEffectMixin
    {
        
        public abstract void Activate(Unit target,  int level);

        public abstract void Deactivate(Unit user, int skillLevel);

    }
    public abstract class UnitTargetSkillEffectMixin : SkillEffectMixin
    {
        public abstract void Activate(Unit target, Unit caster, int level);

        public abstract void Deactivate(Unit target,  Unit caster, int skillLevel);
        [field:SerializeField]public bool TargetIsCaster { get; set; }
    }

    public abstract class TileTargetSkillEffectMixin:SkillEffectMixin
    {
        public abstract void Activate(Tile target, int level);
    }
}