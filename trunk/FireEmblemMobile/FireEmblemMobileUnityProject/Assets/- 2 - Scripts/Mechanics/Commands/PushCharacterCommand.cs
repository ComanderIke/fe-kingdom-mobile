using Assets.Core;
using Assets.GameActors.Units;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Mechanics.Commands
{
    internal class PushCharacterCommand : Command
    {
        private readonly Unit unit;
        private Vector2 direction;

        public PushCharacterCommand(Unit unit, Vector2 direction)
        {
            this.unit = unit;
            this.direction = direction;
        }

        public override void Execute()
        {
            var pos = new Vector2(unit.GridPosition.X + direction.x, (unit.GridPosition.Y + direction.y));
            if (GridGameManager.Instance.GetSystem<Map.MapSystem>().GridLogic.IsTileAccessible(pos))
            {
                if (GridGameManager.Instance.GetSystem<Map.MapSystem>().Tiles[(int) pos.x, (int) pos.y].Unit != null)
                {
                    GridGameManager.Instance.GetSystem<Map.MapSystem>().Tiles[(int) pos.x, (int) pos.y].Unit.MoveActions
                        .Push(direction);
                }

                if (GridGameManager.Instance.GetSystem<Map.MapSystem>().GridLogic.IsTileAccessible(pos, unit))
                    unit.SetPosition((int) pos.x, (int) pos.y);
            }

            GridGameManager.Instance.StartCoroutine(Delay());
        }

        private static IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.0f);
            UnitActionSystem.OnCommandFinished();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}