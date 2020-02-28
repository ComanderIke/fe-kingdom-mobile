using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    class PushCharacterCommand : Command
    {
        private Unit unit;
        private Vector2 direction;

        public PushCharacterCommand(Unit unit,Vector2 direction)
        {
            this.unit = unit;
            this.direction = direction;
        }
        public override void Execute()
        {
            Vector2 pos = new Vector2(unit.GridPosition.x + direction.x, (unit.GridPosition.y + direction.y));
            if (MainScript.instance.GetSystem<global::MapSystem>().GridLogic.IsTileAccessible(pos))
            {
                if (MainScript.instance.GetSystem<global::MapSystem>().Tiles[(int)pos.x, (int)pos.y].character != null)
                {
                    MainScript.instance.GetSystem<global::MapSystem>().Tiles[(int)pos.x, (int)pos.y].character.MoveActions.Push(direction);
                }
                if (MainScript.instance.GetSystem<global::MapSystem>().GridLogic.IsTileAccessible(pos, unit))
                    unit.SetPosition((int)pos.x, (int)pos.y);
            }
            MainScript.instance.StartCoroutine(Delay());

        }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.0f);
            UnitActionSystem.onCommandFinished();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
