using System;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "GameData/Skills/Aura/DuringCombat", fileName = "AuraDuringCombat")]
public class AuraDuringCombatSkillBP : PassiveSkillBp
{
    public override Skill Create()
    {
        return new AuraDuringCombatSkill();
    }
}