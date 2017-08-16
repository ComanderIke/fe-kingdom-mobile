using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Debuffs
{
    [System.Serializable]
    public class Poison : Debuff
    {
        int slow;
        int dmg;
        global::Character caster;
        public Poison(int duration, int slow, int dmg, global::Character caster)
        {
            base.duration = duration;
            base.currduration = duration;
            this.slow = slow;
            this.dmg = dmg;
            this.caster = caster;
        }
        public override bool TakeEffect(global::Character c)
        {
            if (currduration > 0)
            {
                base.currduration -= 1;
                c.charclass.movRange -= slow;
               // c.inflictMagicDamage((c.charclass.stats.maxHP / dmg), caster);
            }
            else
                return true;
            return false;

        }
        public override void End(Character c)
        {
            c.Debuffs.Remove(this);
        }
    }
}
