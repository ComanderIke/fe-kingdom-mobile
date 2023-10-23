﻿using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/SpecialState", fileName = "SpecialStateEffect")]
    public class ActivateSpecialStateMixin : SelfTargetSkillEffectMixin
    {
        public override void Activate(Unit target, int level)
        {
            target.SpecialState = true;
        }

        public override void Deactivate(Unit user, int skillLevel)
        {
            user.SpecialState = false;
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return null;
        }
    }
}