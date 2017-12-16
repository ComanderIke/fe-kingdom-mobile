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

namespace Assets.Scripts.AI.AttackPatterns
{
    public class Howl : AttackPattern
    {
        LivingObject unit;
        const int range = 4;

        public Howl(LivingObject unit, Vector2 startPosition)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            Name = "Howl";
            Damage = 0;
            Hit = 100;

            StartPosition = startPosition;
            this.unit = unit;
            GridLogic gridLogic = MainScript.GetInstance().gridManager.GridLogic;
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
                if (MainScript.GetInstance().gridManager.Tiles[(int)pos.x, (int)pos.y].character != null && MainScript.GetInstance().gridManager.Tiles[(int)pos.x, (int)pos.y].character != unit && MainScript.GetInstance().gridManager.Tiles[(int)pos.x, (int)pos.y].character.Player.ID != unit.Player.ID)
                    TargetCount++;
            }
            // possibleInjuries.Add(new Injury);
        }


        public List<Injury> PossibleInjuries { get; private set; }

        public Vector2 StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }

        public String Name { get; private set; }

        public int Damage { get; private set; }

        public int Hit { get; private set; }

        public int MaxTargetCount { get; private set; }

        public int TargetCount { get; private set; }

        public List<Vector2> TargetPositions { get; private set; }

        public void EffectTarget(LivingObject target, Vector2 direction)
        {
            MainScript.GetInstance().GetSystem<UnitActionManager>().PushUnit(target, direction);
        }

        void DoAction()
        {
            Tile[,] tiles = MainScript.GetInstance().gridManager.Tiles;
            foreach (Vector2 position in TargetPositions)
            {

                if (tiles[(int)position.x, (int)position.y].character != null && StartPosition!=position)
                {
                    //Debug.Log("Effect"+tiles[(int)position.x, (int)position.y].character);
                    Vector2 direction = StartPosition - position;
                    EffectTarget(tiles[(int)position.x, (int)position.y].character, direction);
                }
                //Debug.Log(position);
            }
            MainScript.GetInstance().GetSystem<UnitActionManager>().AddCommand(new HowlCommand(unit, TargetPositions));
            MainScript.GetInstance().GetSystem<UnitActionManager>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
        }

        public void Execute()
        {
            Debug.Log("Execute: Howl");
            EventContainer.howlUsed(unit, this);
            EventContainer.continuePressed += DoAction;

        }

    }
}
