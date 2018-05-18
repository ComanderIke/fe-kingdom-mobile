﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Debuffs
{
    [System.Serializable]
    public class Poison : Debuff
    {
        private int slow;

        public Poison(int duration, int slow):base(duration)
        {
            this.slow = slow;
        }
        public override bool TakeEffect(Unit c)
        {
            c.Stats.MoveRange -= slow;
            return base.TakeEffect(c);

        }
        public override void Remove(Unit c)
        {
            base.TakeEffect(c);
        }
    }
}
