﻿using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;
using Assets.Scripts.Injuries;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class JumpAttack : AttackPattern
    {
        Unit unit;

        public JumpAttack(Unit unit)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            Name = "JumpAttack";
            Damage = (int)(unit.Stats.Attack*1.5f);
            TargetType = AttackTargetType.SingleEnemy;
            TargetPositions = new List<Vector2>();
            Hit = 100;
            Type = AttackPatternType.Aggressive;
            this.unit = unit;
        }

        public Vector2 StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }



        void DoAction()
        {
            
            MainScript.instance.GetSystem<UnitActionSystem>().AddCommand(new AttackCommand(unit, MainScript.instance.GetSystem<GridSystem>().GetTileFromVector2(TargetPositions[0]).character));
            MainScript.instance.GetSystem<UnitActionSystem>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
            //EventContainer.commandFinished();
            
        }

        public override void Execute()
        {
            AttackPattern.onAttackPatternUsed(unit, this);
            UISystem.onContinuePressed += DoAction;

        }


        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

