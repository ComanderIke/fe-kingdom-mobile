using System;
using Game.GameActors.Units;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "GameData/Skills/Aura/DuringCombat", fileName = "AuraDuringCombat")]
public class AuraDuringCombat : PassiveSkill
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