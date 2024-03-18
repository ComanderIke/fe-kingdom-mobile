using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameMechanics
{
    [CreateAssetMenu(menuName = "GameData/Skills/Curse", fileName = "Curse")]
    public class CurseBP : SkillBp
    {
        public override Skill Create()
        {
            return new Curse(Name, Description, Icon, Tier, maxLevel, base.InitPassiveSkillMixins(),base.InitCombatSkillMixin(), base.InitActiveSkillMixins(), SkillTransferData);
        }

    }
}