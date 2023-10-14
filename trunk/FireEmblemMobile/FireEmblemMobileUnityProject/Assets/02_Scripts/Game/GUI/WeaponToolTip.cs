using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class WeaponToolTip : ItemToolTip
    {
        public TextMeshProUGUI dmg;
        public TextMeshProUGUI hit;
        public TextMeshProUGUI crit;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetValues(Weapon weapon,  Vector3 position)
        {
            base.SetValues(new StockedItem(weapon,1), position);
            dmg.text = ""+weapon.GetDamage();
            hit.text = ""+weapon.GetHit()+" %";
            crit.text = ""+weapon.GetCrit()+" %";
        }
    }
}
