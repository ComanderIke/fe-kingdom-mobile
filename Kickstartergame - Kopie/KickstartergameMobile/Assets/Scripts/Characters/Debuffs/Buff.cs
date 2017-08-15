using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Debuffs
{
    [System.Serializable]
    public abstract class Buff
    {
        protected int duration;
        protected int currduration;
        public abstract bool TakeEffect(global::Character c);
        public abstract void End(global::Character c);
    }
}
