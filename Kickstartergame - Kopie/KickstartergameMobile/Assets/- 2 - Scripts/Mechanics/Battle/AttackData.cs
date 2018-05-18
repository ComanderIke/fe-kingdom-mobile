using Assets.Scripts.Characters;
using System.Collections.Generic;

namespace Assets.Scripts.Battle
{
    public class AttackData
    {
        public Unit Attacker { get; set; }
        public float AttackMultiplier { get; set; }
        public int Dmg { get; set; }
        public int HitChance { get; set; }
        public bool DidConnect { get; set; }
        public List<AttackAttributes> AttackAttributes { get; set; }

        public AttackData(Unit attacker, int dmg, int hitChance, bool didConnect,float attackMultiplier=1.0f,List<AttackAttributes> attackAttributes=null )
        {
            Attacker = attacker;
            Dmg = dmg;
            HitChance = hitChance;
            DidConnect = didConnect;
            AttackMultiplier = attackMultiplier;
        }
    }
    public enum AttackAttributes
    {
        FollowUp,
        Counter,
        Critical,
        Lethal,
        SurpriseAttack,
        FrontalAttack,
        SpecialAttack
    }
}
