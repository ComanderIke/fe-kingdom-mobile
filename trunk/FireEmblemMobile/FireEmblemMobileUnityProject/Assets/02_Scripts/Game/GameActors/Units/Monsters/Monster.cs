using Game.GameActors.Items.Weapons;
using Game.Grid;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.Monsters
{
    [CreateAssetMenu(menuName = "GameData/Monster", fileName = "Monster")]
    public class Monster : UnitBP
    {
        
        public MonsterType Type;
        [FormerlySerializedAs("Weapon")] public WeaponBP weaponBp;
    }
}