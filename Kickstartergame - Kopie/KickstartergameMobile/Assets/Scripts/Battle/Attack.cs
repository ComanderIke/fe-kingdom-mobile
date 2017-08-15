using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Battle
{
    public class Attack
    {
        public Character attacker;
        public int dmg;
        public int hitchance;
        public bool isRunAttack;
        public bool isCounterAttack;
        public bool hit;
        public bool isLethal;
        public bool isFollowUp;
        public Attack(Character attacker, int dmg, int hitchance, bool isRunAttack, bool isCounterAttack, bool hit, bool isLethal, bool isFollowUp)
        {
            this.attacker = attacker;
            this.dmg = dmg;
            this.hitchance = hitchance;
            this.isRunAttack = isRunAttack;
            this.isCounterAttack = isCounterAttack;
            this.hit = hit;
            this.isLethal = isLethal;
            this.isFollowUp = isFollowUp;
        }
    }
}
