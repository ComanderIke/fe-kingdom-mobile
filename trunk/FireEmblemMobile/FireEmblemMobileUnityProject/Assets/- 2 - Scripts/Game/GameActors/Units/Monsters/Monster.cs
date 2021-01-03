using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Monsters
{
    [CreateAssetMenu(menuName = "GameData/Monster", fileName = "Monster")]
    public class Monster : Unit
    {
        public MonsterType Type;

        private new void OnEnable()
        {
            base.OnEnable();
            GridPosition = new GridPosition(this);
        }
    }
}