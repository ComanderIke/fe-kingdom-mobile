using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters.Skills
{
    [System.Serializable]
    public abstract class PositionTargetSkill : Skill
    {
        public PositionTargetSkill()
        {
        }
        public virtual void Activate(Unit user, Vector3 target)
        {
            inAnimation = true;
            skillTime = 0;
        }
        public virtual void Effect(Unit user, Vector3 target)
        {

        }
    }
}
