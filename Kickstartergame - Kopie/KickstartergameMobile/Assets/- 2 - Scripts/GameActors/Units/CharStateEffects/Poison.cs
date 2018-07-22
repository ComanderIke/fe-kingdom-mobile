using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName="GameData/Unit/Debuffs/Poison", fileName="Poison")]
[System.Serializable]
public class Poison : Debuff
{
    public int slow;

    public override bool TakeEffect(Unit c)
    {
        c.Stats.MoveRange -= slow;
        return base.TakeEffect(c);

    }
    public override void Remove(Unit c)
    {
        base.TakeEffect(c);
    }
}

