using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    class StampedeCommand : Command
    {
        List<BigTile> bigTiles;
        Unit unit;
        public StampedeCommand(Unit unit, List<BigTile> bigTiles)
        {
            this.bigTiles = bigTiles;
            this.unit = unit;
        }
        public override void Execute()
        {
            BigTile endPosition;
            int index = bigTiles.Count - 1;
            if (index < 0)
                return;
            do
            {
                endPosition = bigTiles[index];
                index -= 1;
            }
            while (!MainScript.instance.GetSystem<global::MapSystem>().GridLogic.IsBigTileAccessible(endPosition, unit) && index >= 0);
            Debug.Log("EndPosition: " + endPosition);
            if (MainScript.instance.GetSystem<global::MapSystem>().GridLogic.IsBigTileAccessible(endPosition, unit))
            {
                new MoveCharacterCommand(unit, (int)endPosition.BottomLeft().x, (int)endPosition.BottomLeft().y).Execute();
            }
            else
            {
                UnitActionSystem.onCommandFinished();
            }
                
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
