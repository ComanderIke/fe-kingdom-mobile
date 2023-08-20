using Game.Grid;
using Game.Manager;
using Game.Map;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/TargetConditions/OverrideTargetCondition", fileName = "OverrideTargetCondition")]
    public class OverrideTargetCondition : SingleTargetCondition
    {
        public override bool CanTarget(Unit caster, Unit target)
        {
            Vector2 direction = caster.GridComponent.GridPosition.AsVector() - target.GridComponent.GridPosition.AsVector();
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            Tile landOnTile = null;
            int newX = (int)(target.GridComponent.GridPosition.X + direction.x);
            int newY = (int)(target.GridComponent.GridPosition.Y + direction.y);
            while (landOnTile != null||gridSystem.IsOutOfBounds(newX, newY))
            {
                if(gridSystem.GridLogic.IsValidLocation(caster,caster.GridComponent.GridPosition.X, caster.GridComponent.GridPosition.Y,newX, newY, false))
                {
                    landOnTile = gridSystem.Tiles[newX, newY];
                }
                newX = (int)(newX + direction.x);
                newY = (int)(newY + direction.y);
            }
            return landOnTile!=null;
        }
    }

    public enum CompareNumbersType
    {
        Equal,
        NotEqual,
        Higher,
        Lower
    }
}