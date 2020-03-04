using Assets.Core;
using Assets.Map;
using UnityEngine;

namespace Assets.GameActors.Units
{
    public class MoveActions
    {
        private readonly Unit unit;

        public MoveActions(Unit unit)
        {
            this.unit = unit;
        }

        public void Push(Vector2 direction)
        {
            var pos = new Vector2(unit.GridPosition.X + direction.x, unit.GridPosition.Y + direction.y);
            if (MainScript.Instance.GetSystem<MapSystem>().GridLogic.IsTileAccessible(pos))
            {
                if (MainScript.Instance.GetSystem<MapSystem>().Tiles[(int)pos.x, (int)pos.y].Unit != null)
                    MainScript.Instance.GetSystem<MapSystem>().Tiles[(int)pos.x, (int)pos.y].Unit.MoveActions
                        .Push(direction);
                if (MainScript.Instance.GetSystem<MapSystem>().GridLogic.IsTileAccessible(pos, unit))
                    unit.SetPosition((int)pos.x, (int)pos.y);
            }
        }
    }
}