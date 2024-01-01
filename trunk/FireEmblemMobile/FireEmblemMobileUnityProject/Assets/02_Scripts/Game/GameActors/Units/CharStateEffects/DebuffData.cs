using System.Collections.Generic;
using System.Linq;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [CreateAssetMenu(menuName = "GameData/Buffs/DebuffData")]
    public class DebuffData : BuffDebuffBaseData
    {
        [SerializeField] public DebuffType debuffType;
        [SerializeField] public List<DebuffData> negateTypes;

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            Debug.Log("DEBUFF APPLY");
            base.Apply(caster,target,skilllevel);
            foreach (var negate in negateTypes)
            {
                var tmpList = target.StatusEffectManager.Buffs.Where(d => d == negate);
                foreach (var entry in tmpList)
                {
                    target.StatusEffectManager.Buffs.Remove(entry);
                }
            }
            switch (debuffType)
            {
                case DebuffType.Tempted:
                    target.Faction.RemoveUnit(target);
                    caster.Faction.AddUnit(target);
                    break;
                
            }
        }
        public override IEnumerable<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>(){new("Applies: ", debuffType.ToString(), debuffType.ToString())};
        }
        public override void TakeEffect(Unit unit)
        {
            base.TakeEffect(unit);
            Debug.Log("DEBUFF TAKE EFFECT");
            switch (debuffType)
            {
                case DebuffType.Stunned: unit.TurnStateManager.UnitTurnFinished(); 
                    break;
                case DebuffType.Poisened: unit.Hp -= unit.MaxHp / 10;
                    break;
                
            }
        }
    }
}