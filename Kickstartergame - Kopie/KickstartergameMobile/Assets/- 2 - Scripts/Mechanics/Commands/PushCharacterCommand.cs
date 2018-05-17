using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Commands
{
    class PushCharacterCommand : Command
    {
        private LivingObject unit;
        private Vector2 direction;

        public PushCharacterCommand(LivingObject unit,Vector2 direction)
        {
            this.unit = unit;
            this.direction = direction;
        }
        public override void Execute()
        {
            Vector2 pos = new Vector2(unit.GridPosition.x + direction.x, (unit.GridPosition.y + direction.y));
            if (MainScript.GetInstance().GetSystem<GridSystem>().GridLogic.IsTileAccessible(pos))
            {
                if (MainScript.GetInstance().GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character != null)
                {
                    MainScript.GetInstance().GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character.MoveActions.Push(direction);
                }
                if (MainScript.GetInstance().GetSystem<GridSystem>().GridLogic.IsTileAccessible(pos, unit))
                    unit.SetPosition((int)pos.x, (int)pos.y);
            }
            MainScript.GetInstance().StartCoroutine(Delay());

        }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.0f);
            EventContainer.commandFinished();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
