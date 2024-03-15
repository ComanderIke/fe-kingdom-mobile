using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GUI.EncounterUI.Merchant;
using Game.GUI.Screens;
using Game.GUI.ToolTips;
using TMPro;
using UnityEditor;
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

        public GameObject skillArea;
        public TextMeshProUGUI skillName;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private Weapon weapon;
        public void SetValues(Unit user,Weapon weapon,  Vector3 position)
        {
            base.SetValues(new StockedItem(weapon,1), position);
            this.weapon = weapon;
            if (weapon.Skill != null)
            {
                skillName.text = weapon.Skill.Name;
            }
            skillArea.gameObject.SetActive(weapon.Skill!=null);
            dmg.text = ""+weapon.GetDamage();
            hit.text = ""+weapon.GetHit()+" %";
            crit.text = ""+weapon.GetCrit()+" %";
            slot.Show(user, weapon,false);
        }

        public void SkillToolTipCLicked()
        {
            if (weapon.Skill != null)
            {
                ToolTipSystem.Show(weapon.Skill,false,GetComponent<RectTransform>().anchoredPosition+new Vector2(450,150), true);
            }
        }
    }
}
