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
    public class Flee : AttackPattern
    {
        LivingObject unit;

        public Flee(LivingObject unit)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            Name = "Flee";
            Damage = 0;
            Hit = 100;
            Type = AttackPatternType.Defensive;
            this.unit = unit;
        }

        public Vector2 StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }





        void DoAction()
        {
            //MainScript.GetInstance().GetSystem<UnitActionManager>().AddCommand(new LickWoundsCommand(unit));
            // MainScript.GetInstance().GetSystem<UnitActionManager>().ExecuteActions();
            //unit.Heal(2);
           // unit.UnitTurnState.IsWaiting = true;
        }

        public override void Execute()
        {
            Debug.Log("Execute: Flee");
            EventContainer.attackPatternUsed(unit, this);
            EventContainer.continuePressed += DoAction;

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
