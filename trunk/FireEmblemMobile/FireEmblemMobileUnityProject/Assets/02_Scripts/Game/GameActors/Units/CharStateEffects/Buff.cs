using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public enum BuffType
    {
        Damage,
        Defense,
        Resistance,
        Dodge,
        Critical,
        Accuracy,
        Cleansing,
        Regeneration
    }
    [CreateAssetMenu(fileName = "Buff", menuName = "GameData/Buff")]
    public class Buff:BuffDebuffBase
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;
        
        [SerializeField] private BuffType buffType;
       
    }
}