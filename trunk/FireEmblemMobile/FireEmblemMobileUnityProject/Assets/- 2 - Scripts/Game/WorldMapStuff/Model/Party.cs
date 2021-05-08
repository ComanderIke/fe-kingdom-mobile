using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class Party:ScriptableObject
{


    public WM_Faction Faction;
    private List<Unit> members;
    public static Action<Party> PartyDied;

    public Party()
    {
        members = new List<Unit>();
    }
    
    public bool IsAlive()
    {
        return members.Count(a => a.IsAlive()) != 0;
    }
}

