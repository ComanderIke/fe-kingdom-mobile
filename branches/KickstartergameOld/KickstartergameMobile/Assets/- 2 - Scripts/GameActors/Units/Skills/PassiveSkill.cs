using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Skills
{
    [Serializable]
    class PassiveSkill : Skill
    {
        public override bool CanTargetCharacters()
        {
            return false;
        }

        public override int getDamage(Unit user, bool justToShow)
        {
            return 0;
        }
    }
}
