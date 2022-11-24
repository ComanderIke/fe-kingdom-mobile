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
        public  ExpBarController expRenderer;
        [SerializeField] private Canvas characterScreen = default;
        // [SerializeField] private Image topUiEnemy = default;
        [SerializeField] private TextMeshProUGUI characterName = default;
        [SerializeField] private TextMeshProUGUI level = default;
        [SerializeField] private ExpBarController expBar = default;
        [SerializeField] private TextMeshProUGUI expLabel = default;
        [SerializeField] private TextMeshProUGUI str = default;
        [SerializeField] private TextMeshProUGUI spd = default;
        [SerializeField] private TextMeshProUGUI mag = default;
        [SerializeField] private TextMeshProUGUI def = default;
        [SerializeField] private TextMeshProUGUI skl = default;
        [SerializeField] private TextMeshProUGUI res = default;
        [SerializeField] private TextMeshProUGUI hp = default;
        [SerializeField] private UIFilledBarController hpBar = default;
        [SerializeField] private TextMeshProUGUI sp = default;
        [SerializeField] private UIFilledBarController spBar = default;
        [SerializeField] private Image characterSprite = default;
        [SerializeField] private Image classSprite = default;
        [SerializeField] private Image[] motivationSprites = default;
        // [SerializeField] private TextMeshProUGUI atk = default;
        // [SerializeField] private Image equippedWeaponSprite = default;
        // [SerializeField] private Image[] weaponProficiencyGradients = default;
        // [SerializeField] private TextMeshProUGUI[] weaponProficiencyLevels = default;
        [SerializeField] private Image[] skillSprites = default;
        [SerializeField] private Image[] inventorySprites = default;



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