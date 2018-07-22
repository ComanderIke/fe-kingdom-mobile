using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;


namespace Assets.Scripts.AI.AttackPatterns
{
    public class BasicAttack : AttackPattern
    {
        Unit unit;
        Unit target;
        const int range = 4;

        public BasicAttack(Unit unit, Unit target)
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
            MainScript.instance.GetSystem<UnitActionSystem>().AddCommand(new AttackCommand(unit, target));
            MainScript.instance.GetSystem<UnitActionSystem>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
        }

        public override void Execute()
        {
            Debug.Log("Execute: BasicAttack");
            DoAction();

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
