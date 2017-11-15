using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Skills
{
    abstract class SelfTargetSkill : Skill
    {
        public override bool CanTargetCharacters()
        {
            return false;
        }

        public override int getDamage(LivingObject user, bool justToShow)
        {
            return 0;
        }
        public virtual void Activate(LivingObject user)
        {
            inAnimation = true;
            skillTime = 0;
        }
    }
}
