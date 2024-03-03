using Game.GameActors.Items.Weapons;
using Game.GUI.EncounterUI.Merchant;
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

        public Image icon;
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
            icon.sprite = weapon.Sprite;
        }
    }
}
