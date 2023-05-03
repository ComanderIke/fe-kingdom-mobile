using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class TopUi : MonoBehaviour
    { 
      
        [SerializeField] private Canvas characterScreen = default;
        // [SerializeField] private Image topUiEnemy = default;
  
        // [SerializeField] private TextMeshProUGUI atk = default;
        // [SerializeField] private Image equippedWeaponSprite = default;
        // [SerializeField] private Image[] weaponProficiencyGradients = default;
        // [SerializeField] private TextMeshProUGUI[] weaponProficiencyLevels = default;
   



        void Start()
        {
       

        }
        public void Show(UnitBP c)//Unused
        {
           
            characterScreen.enabled = true;
            
            //
            // //topUiEnemy.gameObject.SetActive(!c.Faction.IsPlayerControlled);
            // expBar.gameObject.SetActive(c.Faction.IsPlayerControlled);
            // expLabel.gameObject.SetActive(c.Faction.IsPlayerControlled);
            //
            // characterName.text = c.name;
            // level.text = "" + c.ExperienceManager.Level;
            // expBar.SetText(c.ExperienceManager.Exp);
            // expBar.SetFillAmount(c.ExperienceManager.Exp);
            //
            // str.text = "" + c.Stats.STR;
            // spd.text = "" + c.Stats.AGI;
            // mag.text = "" + c.Stats.INT;
            // skl.text = "" + c.Stats.DEX;
            // def.text = "" + c.Stats.Armor;
            // res.text = "" + c.Stats.FAITH;
            // hpBar.SetFillAmount((c.Hp * 1.0f) / c.Stats.MaxHp);
            // hpBar.SetColor(c.Faction.Id == 0 ? ColorManager.Instance.MainGreenColor : ColorManager.Instance.MainRedColor);
            // if(c.Hp==c.Stats.MaxHp)
            //     hp.text = ""+c.Hp;
            // else
            //     hp.text = c.Hp + "/" + c.Stats.MaxHp;
            // spBar.SetFillAmount((c.Sp * 1.0f) / c.Stats.MaxSp);
            // if (c.Sp == c.Stats.MaxSp)
            //     sp.text = ""+c.Sp;
            // else
            //     sp.text = c.Sp + "/" + c.Stats.MaxSp;
            // //atk.text = "" + c.BattleStats.GetAttackDamage();
            // characterSprite.sprite = c.visuals.CharacterSpriteSet.FaceSprite;



        }

        public void Hide()
        {
            characterScreen.enabled = false;
        }
    }
}