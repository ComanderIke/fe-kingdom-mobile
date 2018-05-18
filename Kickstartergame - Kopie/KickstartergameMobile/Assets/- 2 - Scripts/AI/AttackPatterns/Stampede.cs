using System;
using System.Collections.Generic;
using Assets.Scripts.Injuries;
using UnityEngine;
using Assets.Scripts.Grid;
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.AI.AttackPatterns
{
    public class Stampede : AttackPattern
    {
        Unit unit;

        public Stampede(Unit unit, BigTile startPosition, Vector2 direction, int range)
        {
            PossibleInjuries = new List<Injury>();
            TargetPositions = new List<Vector2>();
            TargetBigTilePositions = new List<BigTile>();
            Name = "Stampede";
            Damage = 5;
            Hit = 80;
            StartPosition = startPosition;
            Direction = direction;
            Type = AttackPatternType.Aggressive;
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
                if (MainScript.instance.GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character != null && MainScript.instance.GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character != unit && MainScript.instance.GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character.Player.ID != unit.Player.ID)
                    TargetCount++;
            }
           // possibleInjuries.Add(new Injury);
        }

        private void AddBigTileToTargetPositions(BigTile bigTile)
        {
            if (!MainScript.instance.GetSystem<GridSystem>().GridLogic.IsBigTileAccessible(bigTile))
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


        public BigTile StartPosition { get; private set; }

        public Vector2 Direction { get; private set; }



        public List<BigTile> TargetBigTilePositions { get; private set; }



        public void EffectTarget(Unit target)
        {

            MainScript.instance.GetSystem<UnitActionSystem>().PushUnit(target, Direction);

        }
        void DoAction()
        {
            Tile[,] tiles = MainScript.instance.GetSystem<GridSystem>().Tiles;
            foreach (Vector2 position in TargetPositions)
            {

                if (tiles[(int)position.x, (int)position.y].character != null && !StartPosition.Contains(position))
                {
                    //Debug.Log("Effect"+tiles[(int)position.x, (int)position.y].character);
                    EffectTarget(tiles[(int)position.x, (int)position.y].character);
                }
                //Debug.Log(position);
            }
            MainScript.instance.GetSystem<UnitActionSystem>().AddCommand(new StampedeCommand(unit, TargetBigTilePositions));
            MainScript.instance.GetSystem<UnitActionSystem>().ExecuteActions();
            unit.UnitTurnState.IsWaiting = true;
        }
        public override void Execute()
        {
            Debug.Log("Execute: BigCharge");
            AttackPattern.onAttackPatternUsed(unit, this);
            UISystem.onContinuePressed += DoAction;
            
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
