using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class Debuff : CharacterState
{
    public override void Remove(Unit c)
    {
        c.Debuffs.Remove(this);
    }
}

