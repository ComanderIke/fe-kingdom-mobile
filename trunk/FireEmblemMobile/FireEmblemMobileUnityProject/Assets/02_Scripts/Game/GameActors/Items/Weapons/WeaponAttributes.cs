using System;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class WeaponAttributes 
    {
        public int Dmg=3;
        public int Hit=70;
        public int Crit=0;
        public int Weight;
        public EffectMixin effect;

    }
}