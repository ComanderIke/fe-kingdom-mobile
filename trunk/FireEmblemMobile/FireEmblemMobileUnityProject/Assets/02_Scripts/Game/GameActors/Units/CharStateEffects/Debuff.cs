using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public enum DebuffType
    {
        CrippleDamage,
        CrippleDefense,
        CrippleResistance,
        Blinded,
        Stunned,
        Silenced,
        Frozen,
        Burned,
        Poisened,
        Snarred,
        Slept
    }
    [CreateAssetMenu(fileName = "Debuff", menuName = "GameData/Debuff")]
    public class Debuff:BuffDebuffBase
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;
        [SerializeField] private DebuffType debuffType;
        public bool TakeEffect(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }

    
}