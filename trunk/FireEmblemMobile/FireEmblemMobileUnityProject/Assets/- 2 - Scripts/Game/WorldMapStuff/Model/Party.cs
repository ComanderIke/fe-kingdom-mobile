using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

[CreateAssetMenu(fileName = "Party", menuName = "GameData/Party")]
public class Party:WM_Actor
{

    [SerializeField]
    private WM_Faction faction;

    public WM_Faction Faction
    {
        get
        {
            return faction;
        }
        set
        {
            faction = value;
        }
    }

    public GameTransformManager GameTransformManager { get; set; }
    public WorldMapPosition location { get; set; }
    [SerializeField]
    private List<Unit> members;
    public static Action<Party> PartyDied;
    public WM_ActorRenderer WmActorRenderer { get; set; }

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

    public override void SetAttackTarget(bool b)
    {
        Debug.Log("TODO AttackTarget Visuals");
    }

   

    public override void ResetPosition()
    {
        Debug.Log("ResetLocation");
    }
}