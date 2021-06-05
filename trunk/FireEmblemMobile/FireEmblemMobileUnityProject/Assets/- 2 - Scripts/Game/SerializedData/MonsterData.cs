using Game.GameActors.Units.Monsters;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class MonsterData: UnitData
    {
        public MonsterData(Monster unit) : base(unit)
        {
        }
    }
}