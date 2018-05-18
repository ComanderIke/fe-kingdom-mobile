using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Debuffs
{
    public abstract class Buff : CharacterState
    {
        public Buff(int duration) : base(duration)
        {

        }
        public override void Remove(Unit c)
        {
            c.Buffs.Remove(this);
        }
    }
}
