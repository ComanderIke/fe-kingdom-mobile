using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

public class Party:IWM_Actor
{


    public WM_Faction Faction{ get; set; }
    public GameTransformManager GameTransformManager { get; set; }
    public WorldMapPosition location { get; set; }
    private List<Unit> members;
    public static Action<Party> PartyDied;

    public Party()
    {
        members = new List<Unit>();
        TurnState = new TurnStateManager(this);
    }

    public TurnStateManager TurnState { get; set; }

    public bool IsAlive()
    {
        return members.Count(a => a.IsAlive()) != 0;
    }

    public void SetAttackTarget(bool b)
    {
        Debug.Log("TODO AttackTarget Visuals");
    }

    public TurnStateManager TurnStateManager { get; set; }

    public void ResetPosition()
    {
        Debug.Log("ResetLocation");
    }
}

