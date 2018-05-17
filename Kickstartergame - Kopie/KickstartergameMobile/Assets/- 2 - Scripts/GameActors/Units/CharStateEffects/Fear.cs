using Assets.Scripts.Characters.Debuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters.CharStateEffects
{
    public class Fear : Debuff
    {
        int originmove;
        public Fear(int duration):base(duration)
        {

        }
        public override bool TakeEffect(LivingObject c)
        {
            Debug.Log("Take Effect " + c.Name);
            originmove = c.Stats.MoveRange;
            c.Stats.MoveRange = 0;
            return base.TakeEffect(c);

        }
        public override void Remove(LivingObject c)
        {
            c.Stats.MoveRange = originmove;
            base.TakeEffect(c);
        }
    }
}
