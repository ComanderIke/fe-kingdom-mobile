using System;
using System.Collections.Generic;
using Assets.Scripts.Injuries;
using UnityEngine;
using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class LickWounds : AttackPattern
    {
        Unit unit;

        public LickWounds(Unit unit)
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
            UnitActionSystem.onCommandFinished();
        }

        public override void Execute()
        {
            Debug.Log("Execute: LickWounds");
            AttackPattern.onAttackPatternUsed(unit, this);
            UISystem.onContinuePressed += DoAction;

        }


        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
