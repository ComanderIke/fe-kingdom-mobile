using System;
using UnityEngine;

namespace Assets.GameActors.Units.Skills
{
    [Serializable]
    public abstract class PositionTargetSkill : Skill
    {
        public virtual void Activate(Unit user, Vector3 target)
        {

        }

        public virtual void Effect(Unit user, Vector3 target)
        {
        }
    }
}