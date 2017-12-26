using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Injuries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class JumpAttack : AttackPattern
    {
        LivingObject unit;

        public JumpAttack(LivingObject unit)
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
            
            MainScript.GetInstance().GetSystem<UnitActionManager>().AddCommand(new AttackCommand(unit, MainScript.GetInstance().gridManager.GetTileFromVector2(TargetPositions[0]).character));
            MainScript.GetInstance().GetSystem<UnitActionManager>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
            //EventContainer.commandFinished();
            
        }

        public override void Execute()
        {
            Debug.Log("Execute: JumpAttack");
            EventContainer.attackPatternUsed(unit, this);
            EventContainer.continuePressed += DoAction;

        }


        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}

