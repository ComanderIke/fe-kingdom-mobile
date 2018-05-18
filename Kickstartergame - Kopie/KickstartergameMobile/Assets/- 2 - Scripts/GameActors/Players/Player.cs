using Assets.Scripts.Characters;
using System.Collections.Generic;

namespace Assets.Scripts.Players
{
    [System.Serializable]
    public class Player
    {

        public int ID;
        public string Name;
        public bool IsHumanPlayer;
        public List<Unit> Units;

        public Player(int number, string name, bool isPlayerControlled)
        {
            ID = number;
            Name = name;
            IsHumanPlayer = isPlayerControlled;
            Units = new List<Unit>();
           
        }
        public void Init()
        {
            Unit.onUnitDied += RemoveUnit;
        }
        void RemoveUnit(Unit unit)
        {
            if (Units.Contains(unit))
            {
                Units.Remove(unit);
            }
        }
        public bool IsAlive()
        {
            bool isAlive = false;
            foreach(Unit unit in Units)
            {
                if (unit.IsAlive())
                {
                    isAlive = true;
                }
            }
            return isAlive;
        }
        public void AddUnit(Unit c)
        {
            if(Units==null)
                Units = new List<Unit>();
            c.Player = this;
            c.Player.ID = ID;
            Units.Add(c);
        }

    }
}
