using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class MoveActions
    {
        Unit unit;

        public MoveActions(Unit unit)
        {
            this.unit = unit;
        }

        public void Push(Vector2 direction)
        {
            Vector2 pos = new Vector2(unit.GridPosition.x + direction.x, (unit.GridPosition.y + direction.y));
            if (MainScript.instance.GetSystem<GridSystem>().GridLogic.IsTileAccessible(pos)){
                if(MainScript.instance.GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character != null)
                {
                    MainScript.instance.GetSystem<GridSystem>().Tiles[(int)pos.x, (int)pos.y].character.MoveActions.Push(direction);
                }
                if (MainScript.instance.GetSystem<GridSystem>().GridLogic.IsTileAccessible(pos, unit))
                    unit.SetPosition((int)pos.x, (int)pos.y);
            }
        }
    }
}
