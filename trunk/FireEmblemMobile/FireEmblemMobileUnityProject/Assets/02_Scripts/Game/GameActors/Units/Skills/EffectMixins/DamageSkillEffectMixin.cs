using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Damage", fileName = "DamageSkillEffect")]
    public class DamageSkillEffectMixin : SkillEffectMixin
    {
        public DamageType damageType;
        public int []dmg;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;

        public override void Activate(Unit target, Unit caster, int level)
        {
            int baseDamageg = dmg[level];

            int scalingdmg = (int)(caster.Stats.CombinedAttributes().GetAttributeStat(scalingType) * scalingcoeefficient[level]);
            
            target.InflictFixedDamage(caster, baseDamageg+scalingdmg, damageType);
        }

        public override void Activate(Unit target, int level)
        {
           
        }
        public override void Activate(List<Unit> targets, int level)
        {
            foreach (var target in targets)
            {
                Activate(target, level);
            }
        }

       

        public override void Activate(Tile target, int level)
        {
            if (target.GridObject == null)
                return;
            if(target.GridObject is Unit u )
                Activate(u, level);
        }
    }
}