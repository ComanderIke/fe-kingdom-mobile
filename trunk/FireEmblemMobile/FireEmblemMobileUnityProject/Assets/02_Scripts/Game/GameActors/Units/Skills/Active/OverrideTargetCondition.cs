using Game.Grid;
using Game.Grid.Tiles;
using Game.Manager;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Active
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/TargetConditions/OverrideTargetCondition", fileName = "OverrideTargetCondition")]
    public class OverrideTargetCondition : SingleTargetCondition
    {
        public override bool CanTarget(Unit caster, Unit target)
        {
            Debug.Log("Check Target Condition for Override");
            Vector2 direction = target.GridComponent.GridPosition.AsVector()-caster.GridComponent.GridPosition.AsVector();
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            Tile landOnTile = null;
            int newX = (int)(target.GridComponent.GridPosition.X + direction.x);
            int newY = (int)(target.GridComponent.GridPosition.Y + direction.y);
            while (landOnTile == null||gridSystem.IsOutOfBounds(newX, newY))
            {
                if(gridSystem.GridLogic.IsValidLocation(caster,caster.GridComponent.GridPosition.X, caster.GridComponent.GridPosition.Y,newX, newY, false))
                {
                    landOnTile = gridSystem.Tiles[newX, newY];
                    Debug.Log("Valid Land on Tile: " + landOnTile.X+landOnTile.Y);
                }
                else
                {
                    newX = (int)(newX + direction.x);
                    newY = (int)(newY + direction.y);
                }
            }

            Debug.Log("Override Can Target: " + landOnTile!=null);
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