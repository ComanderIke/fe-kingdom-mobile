using Game.GameActors.Items.Weapons;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Monsters
{
    [CreateAssetMenu(menuName = "GameData/Monster", fileName = "Monster")]
    public class Monster : Unit
    {
        public MonsterType Type;
        public Weapon Weapon;
    }
}