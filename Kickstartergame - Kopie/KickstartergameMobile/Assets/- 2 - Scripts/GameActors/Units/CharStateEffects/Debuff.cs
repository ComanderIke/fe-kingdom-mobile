using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Debuffs
{
    public abstract class Debuff : CharacterState
    {
        public Debuff(int duration):base(duration)
        {

        }
        
        public override void Remove(Unit c)
        {
            c.Debuffs.Remove(this);
        }
    }
}
