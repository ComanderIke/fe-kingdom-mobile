using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName="GameData/Skills/PositionTarget", fileName = "PositionTargetSkill")]
    public class PositionTargetSkill : Skill
    {
        public int power;
        public int range;
        public int radius;
        public bool diagonal;
        public bool horizontal;
        public bool vertical;
        public void Activate(Unit user, Vector3 target)
        {

        }

        public void Effect(Unit user, Vector3 target)
        {
        }

        public override bool CanTargetCharacters()
        {
            throw new NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new NotImplementedException();
        }
    }
}