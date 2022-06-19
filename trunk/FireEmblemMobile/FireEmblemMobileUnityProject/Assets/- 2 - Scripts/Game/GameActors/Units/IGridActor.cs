﻿using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units.OnGameObject;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    public interface IGridActor : IGridObject
    {
        GridActorComponent GetActorGridComponent();
        List<int> AttackRanges { get; }
        int MovementRange { get; }
        Faction Faction { get; }
        MoveType MoveType { get; set; }
        GameTransformManager GameTransformManager { get; set; }
        TurnStateManager TurnStateManager { get; set; }
        bool IsEnemy(IGridActor unit);
        

        bool IsAlive();
    }
}