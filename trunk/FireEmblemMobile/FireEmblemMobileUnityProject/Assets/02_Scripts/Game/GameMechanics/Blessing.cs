using Game.GameActors.Units.Skills;

namespace LostGrace
{
    public class Blessing
    {
        private Skill skill;
        private string name;
        private string description;
        private int tier;

        public Blessing(Skill skill, string name, string description, int tier)
        {
            this.skill = skill;
            this.name = name;
            this.description = description;
            this.tier = tier;
        }

        public Skill Skill => skill;

        public string Name => name;

        public string Description => description;

        public int Tier => tier;
    }
}