using System.Linq;
using Game.GameActors.Units;
using Game.GameActors.Units.Monsters;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class MonsterData: UnitData
    {
        public MonsterData(Monster monster) : base(monster)
        {
            
        }
        public override void Load(Unit unit)
        {
            base.Load(unit);
            Monster monster = (Monster) unit;
            monster.Weapon = Object.Instantiate(GameData.Instance.GetMonster(unit.name).Weapon);
        }
    }
}