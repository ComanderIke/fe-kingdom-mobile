using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Players
{
    [System.Serializable]
    public class Army
    {
        public int ID;
        public string Name;
        public bool IsPlayerControlled;
        public List<Unit> Units;

        public Army(int number, string name, bool isPlayerControlled)
        {
            ID = number;
            Name = name;
            IsPlayerControlled = isPlayerControlled;
            Units = new List<Unit>();
           
        }

        public List<Unit> GetActiveUnits()
        {
            List<Unit> activeUnits = new List<Unit>();
            foreach (Unit c in Units)
            {
                if (c.IsActive()&&c.IsAlive())
                {
                    activeUnits.Add(c);
                }
            }
            return activeUnits;
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

        public List<Army> GetOpponentArmys()
        {
            List<Army> opp = new List<Army>();
            foreach(Army army in MainScript.instance.PlayerManager.Players)
            {
                if(army.ID != ID)
                {
                    opp.Add(army);
                }
            }
            return opp;
        }
    }
}
