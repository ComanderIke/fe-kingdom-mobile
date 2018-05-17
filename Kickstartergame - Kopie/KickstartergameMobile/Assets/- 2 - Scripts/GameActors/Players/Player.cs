using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Players
{
    [System.Serializable]
    public class Player
    {

        public int ID;
        public string Name;
        public bool IsHumanPlayer;
        public List<LivingObject> Units;

        public Player(int number, string name, bool isPlayerControlled)
        {
            ID = number;
            Name = name;
            IsHumanPlayer = isPlayerControlled;
            Units = new List<LivingObject>();
           
        }
        public void Init()
        {
            EventContainer.unitDied += RemoveUnit;
        }
        void RemoveUnit(LivingObject unit)
        {
            if (Units.Contains(unit))
            {
                Units.Remove(unit);
            }
        }
        public bool IsAlive()
        {
            bool isAlive = false;
            foreach(LivingObject unit in Units)
            {
                if (unit.IsAlive())
                {
                    isAlive = true;
                }
            }
            return isAlive;
        }
        public void AddUnit(LivingObject c)
        {
            if(Units==null)
                Units = new List<LivingObject>();
            c.Player = this;
            c.Player.ID = ID;
            Units.Add(c);
        }

    }
}
