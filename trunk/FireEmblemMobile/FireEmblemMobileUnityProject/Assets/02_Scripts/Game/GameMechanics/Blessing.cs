using Game.GameActors.Units.Skills;

namespace LostGrace
{
    public class Blessing
    {
        private Skill skill;
        private string effectDescription;
        private string name;
        private string Description;
        private int tier;

        public Blessing(Skill skill, string effectDescription, string name, string description, int tier)
        {
            this.skill = skill;
            this.effectDescription = effectDescription;
            this.name = name;
            Description = description;
            this.tier = tier;
        }

        public Skill Skill => skill;

        public string EffectDescription => effectDescription;

        public string Name => name;

        public string Description1 => Description;

        public int Tier => tier;
    }
}