using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Grid;
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class Roar : AttackPattern
    {
        Unit unit;
        const int range = 4;

        public Roar(Unit unit, Vector2 startPosition)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            Name = "Roar";
            Damage = 0;
            Hit = 100;

            StartPosition = startPosition;
            this.unit = unit;
            GridLogic gridLogic = MainScript.instance.GetSystem<global::MapSystem>().GridLogic;
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    Vector2 pos = new Vector2(startPosition.x + x, startPosition.y + y);
                    if (!TargetPositions.Contains(pos)&&!gridLogic.IsOutOfBounds(pos))
                        TargetPositions.Add(pos);
                }
                
            }
            foreach (Vector2 pos in TargetPositions)
            {
                if (MainScript.instance.GetSystem<global::MapSystem>().Tiles[(int)pos.x, (int)pos.y].character != null && MainScript.instance.GetSystem<global::MapSystem>().Tiles[(int)pos.x, (int)pos.y].character != unit && MainScript.instance.GetSystem<global::MapSystem>().Tiles[(int)pos.x, (int)pos.y].character.Player.ID != unit.Player.ID)
                    TargetCount++;
            }
            // possibleInjuries.Add(new Injury);
        }


     

        public Vector2 StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }

        public void EffectTarget(Unit target)
        {
        }

        void DoAction()
        {
            Tile[,] tiles = MainScript.instance.GetSystem<global::MapSystem>().Tiles;
            foreach (Vector2 position in TargetPositions)
            {

                if (tiles[(int)position.x, (int)position.y].character != null && StartPosition!=position)
                {
                    //Debug.Log("Effect"+tiles[(int)position.x, (int)position.y].character);
                    EffectTarget(tiles[(int)position.x, (int)position.y].character);
                }
                //Debug.Log(position);
            }
            MainScript.instance.GetSystem<UnitActionSystem>().AddCommand(new HowlCommand(unit, TargetPositions));
            MainScript.instance.GetSystem<UnitActionSystem>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
        }

        public override void Execute()
        {
            Debug.Log("Execute: Roar");
            DoAction();

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
