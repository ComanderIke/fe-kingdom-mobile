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
    public class LickWounds : AttackPattern
    {
        LivingObject unit;

        public LickWounds(LivingObject unit)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            Name = "LickWounds";
            Damage = 0;
            Hit = 100;
            TargetType = AttackTargetType.NoTarget;
            Type = AttackPatternType.Defensive;
            this.unit = unit;
        }

        public Vector2 StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }


        void DoAction()
        {
            //MainScript.GetInstance().GetSystem<UnitActionManager>().AddCommand(new LickWoundsCommand(unit));
            // MainScript.GetInstance().GetSystem<UnitActionManager>().ExecuteActions();
            Debug.Log("DoActionLick");
            unit.Heal(2);
            unit.UnitTurnState.IsWaiting = true;
            EventContainer.commandFinished();
        }

        public override void Execute()
        {
            Debug.Log("Execute: LickWounds");
            EventContainer.attackPatternUsed(unit, this);
            EventContainer.continuePressed += DoAction;

        }


        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
