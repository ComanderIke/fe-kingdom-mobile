using System.Collections.Generic;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units.Numbers;
using Game.Mechanics;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillEffectMixin
    {
        private EffectType effectType;
    }
    public class DamageSkillEffectMixin:SkillEffectMixin
    {
        private int dmg;
        private DamageType type;
        private Dictionary<AttributeType, float> scalingAttributes;//float multiplier example INT *0.5 + STR *0.5 +dmg
    }
    public class HealSkillEffectMixin:SkillEffectMixin
    {
        private int heal;
        private Dictionary<AttributeType, float> scalingAttributes;//float multiplier example INT *0.5 + STR *0.5 +heal
    }
    public class DebuffSkillEffectMixin:SkillEffectMixin
    {
        private CharStateEffects.Debuff debuff;
    }
}