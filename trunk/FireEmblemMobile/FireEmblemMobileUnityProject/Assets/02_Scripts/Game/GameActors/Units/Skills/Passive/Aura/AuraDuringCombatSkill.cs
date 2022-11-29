using System;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

[Serializable]
public class AuraDuringCombatSkill : PassiveSkill
{
    public override bool CanTargetCharacters()
    {
        return false;
    }

    public override int GetDamage(Unit user, bool justToShow)
    {
        return 0;
    }

    public AuraDuringCombatSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
    {
    }
}