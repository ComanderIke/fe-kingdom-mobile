using System.Linq;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using Assets.GameResources;
using Assets.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class TopUi : MonoBehaviour
    {
        [SerializeField] private Canvas characterScreen = default;
        [SerializeField] private Image topUiEnemy = default;
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
        [SerializeField] private FilledBarController hpBar = default;
        [SerializeField] private TextMeshProUGUI sp = default;
        [SerializeField] private FilledBarController spBar = default;
        [SerializeField] private Image characterSprite = default;
        [SerializeField] private Image classSprite = default;
        [SerializeField] private Image[] motivationSprites = default;
        [SerializeField] private TextMeshProUGUI atk = default;
        [SerializeField] private Image equippedWeaponSprite = default;
        [SerializeField] private Image[] weaponProficiencyGradients = default;
        [SerializeField] private TextMeshProUGUI[] weaponProficiencyLevels = default;
        [SerializeField] private Image[] skillSprites = default;
        [SerializeField] private Image[] inventorySprites = default;
        private ColorManager colorManager;


        void Start()
        {
            
            colorManager = FindObjectOfType<ColorManager>();

        }
        public void Show(Unit c)
        {
            if (colorManager == null)
            {
                colorManager = FindObjectOfType<ColorManager>();
            }
            characterScreen.enabled = true;
            
            
            //topUiEnemy.gameObject.SetActive(!c.Faction.IsPlayerControlled);
            expBar.gameObject.SetActive(c.Faction.IsPlayerControlled);
            expLabel.gameObject.SetActive(c.Faction.IsPlayerControlled);

            characterName.text = c.Name;
            level.text = "" + c.ExperienceManager.Level;
            expBar.SetText(c.ExperienceManager.Exp);
            expBar.SetFillAmount(c.ExperienceManager.Exp);

            str.text = "" + c.Stats.Str;
            spd.text = "" + c.Stats.Spd;
            mag.text = "" + c.Stats.Mag;
            skl.text = "" + c.Stats.Skl;
            def.text = "" + c.Stats.Def;
            res.text = "" + c.Stats.Res;
            hpBar.SetFillAmount((c.Hp * 1.0f) / c.Stats.MaxHp);
            hpBar.SetColor(c.Faction.Id == 0 ? colorManager.MainGreenColor : colorManager.MainRedColor);
            if(c.Hp==c.Stats.MaxHp)
                hp.text = ""+c.Hp;
            else
                hp.text = c.Hp + "/" + c.Stats.MaxHp;
            spBar.SetFillAmount((c.Sp * 1.0f) / c.Stats.MaxSp);
            if (c.Sp == c.Stats.MaxSp)
                sp.text = ""+c.Sp;
            else
                sp.text = c.Sp + "/" + c.Stats.MaxSp;
            //atk.text = "" + c.BattleStats.GetAttackDamage();
            characterSprite.sprite = c.CharacterSpriteSet.FaceSprite;
           
            foreach (var motivationSprite in motivationSprites)
            {
                motivationSprite.gameObject.SetActive(false);
            }
            if(c.Faction.IsPlayerControlled)
                switch (c.Motivation)
                {
                    case Motivation.Tired:
                        motivationSprites[0].gameObject.SetActive(true);
                        break;
                    case Motivation.Negative: motivationSprites[1].gameObject.SetActive(true); break;
                    case Motivation.Neutral: motivationSprites[2].gameObject.SetActive(true); break;
                    case Motivation.Positive: motivationSprites[3].gameObject.SetActive(true); break;
                    case Motivation.Happy: motivationSprites[4].gameObject.SetActive(true); break;
                }

            if (c is Human human)
            {
                classSprite.sprite = human.Class.Sprite;
                //var sixColors =colorManager.SixGradeColors;
                //if(human.EquippedWeapon!=null)
                //    equippedWeaponSprite.sprite = human.EquippedWeapon.Sprite;
                //for (int i = 0; i < human.WeaponProficiencies().Count; i++)
                //{
                //    string weaponProficiencyLvl = human.WeaponProficiencies().Values.ElementAt(i);
                //    weaponProficiencyLevels[i].text = weaponProficiencyLvl;

                //    switch (weaponProficiencyLvl)
                //    {
                //        case "E":
                //            weaponProficiencyGradients[i].color = sixColors[0];
                //            weaponProficiencyLevels[i].color = new Color(sixColors[0].r, sixColors[0].g, sixColors[0].b, 1); break;
                //        case "D":
                //            weaponProficiencyGradients[i].color = sixColors[1];
                //            weaponProficiencyLevels[i].color = new Color(sixColors[1].r, sixColors[1].g, sixColors[1].b, 1); break;
                //        case "C":
                //            weaponProficiencyGradients[i].color = sixColors[2];
                //            weaponProficiencyLevels[i].color = new Color(sixColors[2].r, sixColors[2].g, sixColors[2].b, 1); break;
                //        case "B":
                //            weaponProficiencyGradients[i].color = sixColors[3];
                //            weaponProficiencyLevels[i].color = new Color(sixColors[3].r, sixColors[3].g, sixColors[3].b, 1); break;
                //        case "A":
                //            weaponProficiencyGradients[i].color = sixColors[4];
                //            weaponProficiencyLevels[i].color = new Color(sixColors[4].r, sixColors[4].g, sixColors[4].b, 1); break;
                //        case "S":
                //            weaponProficiencyGradients[i].color = sixColors[5];
                //            weaponProficiencyLevels[i].color = new Color(sixColors[5].r, sixColors[5].g, sixColors[5].b, 1); break;
                //    }

                //}
                for (int i = 0; i < human.SkillManager.Skills.Count; i++)
                {
                    skillSprites[i].sprite = human.SkillManager.Skills[i].SpriteSet.Sprite;
                }
                for (int i = 0; i < human.Inventory.Items.Count; i++)
                {
                    inventorySprites[i].sprite = human.Inventory.Items[i].Sprite;
                }
            }



              
               
         
        }

        public void Hide()
        {
            characterScreen.enabled = false;
        }
    }
}