using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using Game.GUI;
using UnityEngine;

namespace Game.GameMechanics
{
    [CreateAssetMenu(menuName = "GameData/Skills/Blessing", fileName = "Blessing")]
    public class BlessingBP : SkillBp
    {
        [SerializeField]public God god;
        public override Skill Create()
        {
            return new Blessing(Name, Description, Icon, Tier, maxLevel, base.InitPassiveSkillMixins(), base.InitCombatSkillMixin(),base.InitActiveSkillMixins(), SkillTransferData, god);
        }

    }
}