using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GUI.EncounterUI.Merchant;
using Game.GUI.Screens;
using Game.GUI.ToolTips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
{
    public class WeaponToolTip : ItemToolTip
    {
        public TextMeshProUGUI dmg;
        public TextMeshProUGUI hit;
        public TextMeshProUGUI crit;
        public SmithingSlot slot;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetValues(Unit user,Weapon weapon,  Vector3 position)
        {
            base.SetValues(new StockedItem(weapon,1), position);
            dmg.text = ""+weapon.GetDamage();
            hit.text = ""+weapon.GetHit()+" %";
            crit.text = ""+weapon.GetCrit()+" %";
            slot.Show(user, weapon,false);
        }
    }
}
