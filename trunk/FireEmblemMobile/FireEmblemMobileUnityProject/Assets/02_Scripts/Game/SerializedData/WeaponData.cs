using Game.DataAndReferences.Data;
using Game.GameActors.Items.Weapons;
using UnityEngine;

namespace Game.SerializedData
{
    [System.Serializable]
    public class WeaponData
    {
        [SerializeField] public string weaponId;
        [SerializeField] public int weaponLvl;
        [SerializeField] public int powerUpgLvl = 0;
        [SerializeField] public int accUpgLvl = 0;
        [SerializeField] public int critUpgLvl = 0;
        [SerializeField] public int specialUpgLvl = 0;

        public WeaponData(Weapon weapon)
        {
            weaponId = weapon.Name;
            weaponLvl = weapon.weaponLevel;
            powerUpgLvl = weapon.powerUpgLvl;
            accUpgLvl = weapon.accUpgLvl;
            critUpgLvl = weapon.critUpgLvl;
            specialUpgLvl = weapon.specialUpgLvl;
        }

        public Weapon Load()
        {
            Weapon weapon =GameBPData.Instance.GetWeapon(weaponId);
            weapon.weaponLevel = weaponLvl;
            weapon.powerUpgLvl = powerUpgLvl;
            weapon.accUpgLvl = accUpgLvl;
            weapon.critUpgLvl = critUpgLvl;
            weapon.specialUpgLvl = specialUpgLvl;
            return weapon;
        }
    }
}