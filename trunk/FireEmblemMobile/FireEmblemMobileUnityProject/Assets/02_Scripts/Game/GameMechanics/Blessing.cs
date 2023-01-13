using System;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;

namespace LostGrace
{
    public interface ITemporaryEffect
    {
        void DecreaseDuration();
        int GetDuration(int faith);
    }

    public class Blessing : CurseBlessBase
    {

 

        public Blessing(Skill skill, string name, string description, int tier):base(skill,name, description, tier)
        {
           
        }

 
    }
}