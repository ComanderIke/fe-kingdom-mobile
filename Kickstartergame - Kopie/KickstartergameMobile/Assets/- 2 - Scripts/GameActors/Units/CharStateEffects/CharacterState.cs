using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Debuffs
{
    [System.Serializable]
    public abstract class CharacterState
    {
        protected int duration;
        protected int currduration;

        public CharacterState(int duration)
        {
            this.duration = duration;
            currduration = duration;
        }
        public virtual bool TakeEffect(Unit c)
        {
            if (currduration > 0)
            {
                currduration -= 1;
            }
            else
                return true;
            return false;

        }
        public abstract void Remove(Unit c);
    }
}
