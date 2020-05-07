using UnityEngine;

namespace Assets.GameActors.Units.SpecialAttacks
{
    public abstract class SpecialAttack
    {
        protected SpecialAttack(string name, string description, int cooldown, Sprite iconSprite, GameObject animation)
        {
            Name = name;
            Description = description;
            Cooldown = cooldown;
            SpecialAttackVisual = new SpecialAttackVisual(iconSprite, animation);
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Cooldown { get; set; }
        public SpecialAttackVisual SpecialAttackVisual { get; set; }
        public abstract void UseSpecial(Unit user, int normalAttackDamage, Unit defender);
        public abstract int GetSpecialDmg(Unit user, Unit defender);
    }
}