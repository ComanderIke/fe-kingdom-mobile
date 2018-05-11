using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Skills
{
    [System.Serializable]
    public abstract class SingleTargetSkill : Skill
    {
        public SingleTargetSkill()
        {
        }
        public virtual void Activate(LivingObject user, LivingObject target)
        {
            inAnimation = true;
            skillTime = 0;
        }
        public virtual void Effect(LivingObject user, LivingObject target)
        {

        }
        public override bool CanTargetCharacters()
        {
            return true;
        }
    }

}
