using Assets.Scripts.Characters;

namespace Assets.Scripts.Battle
{
    public class AttackData
    {
        public LivingObject Attacker { get; set; }
        public int Dmg { get; set; }
        public int HitChance { get; set; }
        public bool IsCounterAttack { get; set; }
        public bool Hit { get; set; }
        public bool IsLethal { get; set; }
        public bool IsFollowUp { get; set; }

        public AttackData(LivingObject attacker, int dmg, int hitchance, bool isCounterAttack, bool hit, bool isLethal, bool isFollowUp)
        {
            Attacker = attacker;
            Dmg = dmg;
            HitChance = hitchance;
            IsCounterAttack = isCounterAttack;
            Hit = hit;
            IsLethal = isLethal;
            IsFollowUp = isFollowUp;
        }
    }
}
