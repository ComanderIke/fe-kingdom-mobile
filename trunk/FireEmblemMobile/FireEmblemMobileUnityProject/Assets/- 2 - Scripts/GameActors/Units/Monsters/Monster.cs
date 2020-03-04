using Assets.Grid;
using UnityEngine;

namespace Assets.GameActors.Units.Monsters
{
    [CreateAssetMenu(menuName = "GameData/Monster", fileName = "Monster")]
    public class Monster : Unit
    {
        public MonsterType Type;

        private new void OnEnable()
        {
            base.OnEnable();
            GridPosition = Type == MonsterType.BigMonster ? new BigTilePosition(this) : new GridPosition(this);
        }
    }
}