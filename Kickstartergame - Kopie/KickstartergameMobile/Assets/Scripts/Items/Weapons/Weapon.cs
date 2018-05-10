using Assets.Scripts.Characters;
using Assets.Scripts.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [System.Serializable]
    [CreateAssetMenu(menuName="GameData/Weapon", fileName ="weapon")]
    public class Weapon : Item
    {
        public int Dmg;
        public int Hit;
        [Range(1,4)]
        public int[] AttackRanges;
        public int Crit;
        public WeaponType WeaponType;

        public Weapon(string name, string description, WeaponType type, int dmg, int hit, int crit, List<int> range, Sprite sprite):base(name, description, 1, sprite)
        {
            WeaponType = type;
            Dmg = dmg;
            Hit = hit;
            Crit = crit;
            AttackRanges = range.ToArray();
        }

        public override void use(Human character)
        {
            character.Stats.AttackRanges.Clear();
            character.EquipedWeapon = this;
            foreach(int r in AttackRanges){
                character.Stats.AttackRanges.Add(r);
            }
        }

    }
}
