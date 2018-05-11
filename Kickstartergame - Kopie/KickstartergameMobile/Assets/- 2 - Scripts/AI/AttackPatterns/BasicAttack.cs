using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Injuries;
using UnityEngine;
using Assets.Scripts.Grid;
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;
using Assets.Scripts.Events;
using System.Collections;
using Assets.Scripts.Characters.CharStateEffects;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class BasicAttack : AttackPattern
    {
        LivingObject unit;
        LivingObject target;
        const int range = 4;

        public BasicAttack(LivingObject unit, LivingObject target)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            Name = "BasicAttack";
            Damage = 0;
            Hit = 100;
            this.target = target;
            this.unit = unit;
            Type = AttackPatternType.Aggressive;

            // possibleInjuries.Add(new Injury);
        }


        public Vector2 StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }




        void DoAction()
        {
            MainScript.GetInstance().GetSystem<UnitActionSystem>().AddCommand(new AttackCommand(unit, target));
            MainScript.GetInstance().GetSystem<UnitActionSystem>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
        }

        public override void Execute()
        {
            Debug.Log("Execute: BasicAttack");
            EventContainer.attackPatternUsed(unit, this);
            EventContainer.continuePressed += DoAction;

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
