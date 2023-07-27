using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public enum BuffType
    {
        Cleansing,
        Regeneration,
        Stealth,
        AbsorbingDmg,
        Invulnerability,
        //ClearMovement
        //Add as needed
    }
    [CreateAssetMenu(fileName = "Buff", menuName = "GameData/Buff")]
    public class Buff:BuffDebuffBase
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;
        
        [SerializeField] private BuffType buffType;

        public EffectDescription GetEffectDescription(int level)
        {
            return new EffectDescription("TODO", "TODO", "TODO");
        }

       
    }
}