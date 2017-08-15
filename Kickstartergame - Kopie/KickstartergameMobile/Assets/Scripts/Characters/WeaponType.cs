using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public enum WeaponType
    {
        Sword, Dagger, Magic, Bow, Staff, Spear, Axe
    }
    public class WeaponCategory
    {
        public WeaponType type;
        public WeaponCategory(WeaponType type){
            this.type = type;
        }
        
    }
}
