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
    public class Stampede : AttackPattern
    {
        LivingObject unit;

        public Stampede(LivingObject unit, BigTile startPosition, Vector2 direction, int range)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            TargetBigTilePositions = new List<BigTile>();
            Name = "Stampede";
            Damage = 5;
            Hit = 80;
            StartPosition = startPosition;
            Direction = direction;
            this.unit = unit;
            for(int i=0; i <= range; i++)
            {
                AddBigTileToTargetPositions(new BigTile(new Vector2(startPosition.BottomLeft().x+i*direction.x,startPosition.BottomLeft().y+i*direction.y),
                    new Vector2(startPosition.BottomRight().x + i * direction.x, startPosition.BottomRight().y + i * direction.y),
                    new Vector2(startPosition.TopLeft().x + i * direction.x, startPosition.TopLeft().y + i * direction.y),
                    new Vector2(startPosition.TopRight().x + i * direction.x, startPosition.TopRight().y + i * direction.y)));
            }
            foreach(Vector2 pos in TargetPositions)
            {
                if (MainScript.GetInstance().gridManager.Tiles[(int)pos.x, (int)pos.y].character != null && MainScript.GetInstance().gridManager.Tiles[(int)pos.x, (int)pos.y].character != unit && MainScript.GetInstance().gridManager.Tiles[(int)pos.x, (int)pos.y].character.Player.ID != unit.Player.ID)
                    TargetCount++;
            }
           // possibleInjuries.Add(new Injury);
        }

        private void AddBigTileToTargetPositions(BigTile bigTile)
        {
            if (!MainScript.GetInstance().gridManager.GridLogic.IsBigTileAccessible(bigTile))
                return;
            if (!TargetBigTilePositions.Contains(bigTile))
                TargetBigTilePositions.Add(bigTile);
            if(!TargetPositions.Contains(bigTile.BottomLeft()))
                TargetPositions.Add(bigTile.BottomLeft());
            if (!TargetPositions.Contains(bigTile.BottomRight()))
                TargetPositions.Add(bigTile.BottomRight());
            if (!TargetPositions.Contains(bigTile.TopLeft()))
                TargetPositions.Add(bigTile.TopLeft());
            if (!TargetPositions.Contains(bigTile.TopRight()))
                TargetPositions.Add(bigTile.TopRight());
        }

        public List<Injury> PossibleInjuries { get; private set; }

        public BigTile StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }

        public String Name { get; private set; }

        public int Damage { get; private set; }

        public int Hit { get; private set; }

        public int MaxTargetCount { get; private set; }

        public int TargetCount { get; private set; }

        public List<BigTile> TargetBigTilePositions { get; private set; }

        public List<Vector2> TargetPositions { get; private set; }

        public void EffectTarget(LivingObject target)
        {

            MainScript.GetInstance().GetSystem<UnitActionManager>().PushUnit(target, Direction);

        }
        void DoAction()
        {
            Tile[,] tiles = MainScript.GetInstance().gridManager.Tiles;
            foreach (Vector2 position in TargetPositions)
            {

                if (tiles[(int)position.x, (int)position.y].character != null && !StartPosition.Contains(position))
                {
                    //Debug.Log("Effect"+tiles[(int)position.x, (int)position.y].character);
                    EffectTarget(tiles[(int)position.x, (int)position.y].character);
                }
                //Debug.Log(position);
            }
            MainScript.GetInstance().GetSystem<UnitActionManager>().AddCommand(new StampedeCommand(unit, TargetBigTilePositions));
            MainScript.GetInstance().GetSystem<UnitActionManager>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
        }
        public void Execute()
        {
            Debug.Log("Execute: BigCharge");
            EventContainer.stampedeUsed(unit, this);
            EventContainer.continuePressed += DoAction;
            
        }

    }
}
