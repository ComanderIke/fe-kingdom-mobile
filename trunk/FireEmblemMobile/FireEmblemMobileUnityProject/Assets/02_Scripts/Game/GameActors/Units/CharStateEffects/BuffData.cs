using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [CreateAssetMenu(menuName = "GameData/Buffs/BuffData")]
    public class BuffData : BuffDebuffBaseData
    {
        [SerializeField] private BuffType buffType;

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster,target,skilllevel);
            switch (buffType)
            {
                case BuffType.Cleansing:
                    //TODO
                    break;
                
            }
        }
        public override void TakeEffect(Unit unit)
        {
            base.TakeEffect(unit);
            switch (buffType)
            {
                case BuffType.Cleansing: //TODO
                    break;
                
            }
        }
        public override IEnumerable<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>(){new("Grants", buffType!=BuffType.Custom?buffType.ToString():"", buffType!=BuffType.Custom?buffType.ToString():"")};
        }

       
    }
}