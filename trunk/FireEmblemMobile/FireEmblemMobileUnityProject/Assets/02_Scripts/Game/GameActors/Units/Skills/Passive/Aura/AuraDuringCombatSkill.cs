using System;
using Game.GameActors.Units;

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
}