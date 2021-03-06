﻿using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Unit/Debuffs/Fear", fileName = "Fear")]
[System.Serializable]
public class Fear : Debuff
{
    int originmove;

    public override bool TakeEffect(Unit c)
    {
        Debug.Log("Take Effect " + c.Name);
        originmove = c.Stats.MoveRange;
        c.Stats.MoveRange = 0;
        return base.TakeEffect(c);

    }
    public override void Remove(Unit c)
    {
        c.Stats.MoveRange = originmove;
        base.TakeEffect(c);
    }
}
