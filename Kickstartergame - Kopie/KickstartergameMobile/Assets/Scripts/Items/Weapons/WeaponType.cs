
namespace Assets.Scripts.Characters
{
    [System.Serializable]
    public enum WeaponType
    {
        Sword, Dagger, Magic, Bow, Staff, Spear, Axe
    }
    public class WeaponCategory
    {
        public static WeaponCategory sword = new WeaponCategory(WeaponType.Sword);
        public static WeaponCategory bow = new WeaponCategory(WeaponType.Bow);
        public static WeaponCategory axe = new WeaponCategory(WeaponType.Axe);
        public static WeaponCategory spear = new WeaponCategory(WeaponType.Spear);
        public WeaponType type;
        public WeaponCategory(WeaponType type){
            this.type = type;
        }
        
    }
}
