using UnityEngine;

namespace Game.GameActors.Units.SpecialAttacks
{
    public class SpecialAttackVisual
    {
        public SpecialAttackVisual(Sprite icon, GameObject animation)
        {
            Icon = icon;
            Animation = animation;
        }

        public Sprite Icon { get; set; }
        public GameObject Animation { get; set; }
    }
}