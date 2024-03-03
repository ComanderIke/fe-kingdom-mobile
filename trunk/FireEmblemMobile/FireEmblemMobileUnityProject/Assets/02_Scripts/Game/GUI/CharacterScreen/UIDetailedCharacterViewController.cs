using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GUI.Skills;
using Game.GUI.ToolTips;
using Game.GUI.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.CharacterScreen
{
    public enum ShowAttributeState
    {
        Attributes,
        Growths,
        CombatStats,
    }
    [ExecuteInEditMode]
    public class UIDetailedCharacterViewController : UICharacterViewController
    {
        public TextMeshProUGUI Lv;
        //  public IStatBar ExpBar;
        public UIEquipmentController equipmentController;
        [SerializeField] private UIAnimationSpriteSwapper uiAnimationSpriteSwapper;
        public SkillTreeUI skillTreeUI;
        public Animator IdleAnimation;
        public SkillsUI skillsUI;
   
        [SerializeField] private Image blessingEffect;
        [SerializeField] private Image blessingEffect2;
        [SerializeField] private UIBlessingButton blessingButton;
        [SerializeField] private TextMeshProUGUI attributeHeaderText;
        [SerializeField] private UIBoonBaneController boonBaneController;
        [SerializeField] private Image frame;
        [SerializeField] private Material SelectedBorderMaterial;
        [SerializeField] private Material defaultMateril;
        [SerializeField] private GameObject invisibleButton;
        [SerializeField] private TextMeshProUGUI skillPoints;
        public void SkillTreeClicked()
        {
            skillTreeUI.Show(unit);
        }

        // public void BlessingClicked()
        // {
        //     if (unit.Blessing == null)
        //         return;
        //     ToolTipSystem.Show(unit.Blessing, blessingImage.transform.position);
        // }
        public override void Show(Unit unit, bool button = false)
        {
            if (button)
            {
                if(invisibleButton!=null)
                    invisibleButton.gameObject.SetActive(true);
            }
            else
            {
                if(invisibleButton!=null)
                    invisibleButton.gameObject.SetActive(false);
            }
            showAttributeState = ShowAttributeState.Attributes;
            base.Show(unit, button);
            // Debug.Log("SHOW");
            if (boonBaneController != null)
            {
                boonBaneController.OnBoonSelected -= BoonSelected;
                boonBaneController.OnBaneSelected -= BaneSelected;
                boonBaneController.OnBoonSelected += BoonSelected;
                boonBaneController.OnBaneSelected += BaneSelected;
            }
        }

        public void ShowBoonBaneSelection()
        {
            if(boonBaneController!=null)
                boonBaneController.Show();
        }
        void BoonSelected(AttributeType boon)
        {
            unit.Stats.SetBoon(boon);
            UpdateUI(unit);
        }

        void BaneSelected(AttributeType bane)
        {
            unit.Stats.SetBane(bane);
            UpdateUI(unit);
        }
        public void NextClicked()
        {
        
            Player.Instance.Party.ActiveUnitIndex++;
            UpdateUI(Player.Instance.Party.ActiveUnit);
        
        }
        public void PrevClicked()
        {
            Player.Instance.Party.ActiveUnitIndex--;
            UpdateUI(Player.Instance.Party.ActiveUnit);
        }

        [SerializeField] private Color baneColor;
        [SerializeField] private Color boonColor;
        [SerializeField] private Color normalColor;
    
        protected override void UpdateUI(Unit unit)
        {
            if (!canvas.enabled)
                return;
            if(uiAnimationSpriteSwapper!=null)
                uiAnimationSpriteSwapper.Init(unit.visuals.CharacterSpriteSet);
            if (unit != base.unit&&boonBaneController!=null)
            {
                boonBaneController.SoftResetBoonBane();
            }

            if (showAttributeState==ShowAttributeState.Attributes)
            {
                if(attributeHeaderText!=null)
                    attributeHeaderText.SetText("Attributes");
            }
            else if(showAttributeState==ShowAttributeState.Growths)
            {
                if(attributeHeaderText!=null)
                    attributeHeaderText.SetText("Growths");
            }
            else
            {
                if(attributeHeaderText!=null)
                    attributeHeaderText.SetText("Combat Stats");

            }
            base.UpdateUI(unit);
            if(skillPoints!=null)
                skillPoints.text = ""+unit.SkillManager.SkillPoints;
            Lv.SetText("Lv. "+unit.ExperienceManager.Level);
            if(blessingEffect!=null)
                blessingEffect.gameObject.SetActive(unit.Blessing!=null);
            if(blessingButton!=null)
                blessingButton.gameObject.SetActive(unit.Blessing!=null);
            if (unit.Blessing != null)
            {
                if(blessingEffect!=null)
                    blessingEffect.color = unit.Blessing.God.Color;
                if(blessingEffect2!=null)
                    blessingEffect2.color = unit.Blessing.God.Color;
                if(blessingButton!=null)
                    blessingButton.SetValues(unit.Blessing);
            }
        
            // if(ExpBar!=null)
            //     ExpBar.SetValue(unit.ExperienceManager.Exp, ExperienceManager.MAX_EXP, false);
            skillsUI.Show(unit);
            STR_Label.color=normalColor;
            DEX_Label.color=normalColor;
            INT_Label.color=normalColor;
            AGI_Label.color=normalColor;
            CON_Label.color=normalColor;
            LCK_Label.color=normalColor;
            DEF_Label.color=normalColor;
            FTH_Label.color=normalColor;
            if(boonBaneController!=null)
                boonBaneController.UpdateUI();
       
            switch (unit.Stats.Bane)
            {
                case AttributeType.STR: STR_Label.color=baneColor;
                    break;
                case AttributeType.DEX: DEX_Label.color=baneColor;
                    break;
                case AttributeType.INT: INT_Label.color=baneColor;
                    break;
                case AttributeType.AGI: AGI_Label.color=baneColor;
                    break;
                case AttributeType.CON: CON_Label.color=baneColor;
                    break;
                case AttributeType.LCK: LCK_Label.color=baneColor;
                    break;
                case AttributeType.DEF: DEF_Label.color=baneColor;
                    break;
                case AttributeType.FTH: FTH_Label.color=baneColor;
                    break;
            }
            switch (unit.Stats.Boon)
            {
                case AttributeType.STR: STR_Label.color=boonColor;
                    break;
                case AttributeType.DEX: DEX_Label.color=boonColor;
                    break;
                case AttributeType.INT: INT_Label.color=boonColor;
                    break;
                case AttributeType.AGI: AGI_Label.color=boonColor;
                    break;
                case AttributeType.CON: CON_Label.color=boonColor;
                    break;
                case AttributeType.LCK: LCK_Label.color=boonColor;
                    break;
                case AttributeType.DEF: DEF_Label.color=boonColor;
                    break;
                case AttributeType.FTH: FTH_Label.color=boonColor;
                    break;
            }
            // if (unit.Blessing != null)
            // {
            //     
            //         var go = Instantiate(blessingPrefab, blessingParent);
            //         go.GetComponent<Image>().sprite = unit.Blessing .Skill.Icon;
            //         go.GetComponent<GeneralButtonController>().OnClicked += BlessingClicked;
            //     
            //   
            // }
            // if (unit.Curse != null)
            // {
            //     var go = Instantiate(cursePrefab, curseParent);
            //         go.GetComponent<Image>().sprite = unit.Curse .Skill.Icon;
            //         go.GetComponent<GeneralButtonController>().OnClicked += CurseClicked;
            //     
            // }
       
// Debug.Log("UPDATE UI");
            equipmentController.Show(unit);
            if(IdleAnimation!=null)
                IdleAnimation.runtimeAnimatorController = unit.visuals.Prefabs.UIAnimatorController;
        }

        void BlessingClicked(RectTransform clickedTransform)
        {
            ToolTipSystem.Show(unit.Blessing,true, clickedTransform.position);
        }
        // void CurseClicked(RectTransform clickedTransform)
        // {
        //     ToolTipSystem.Show(unit.Curse, clickedTransform.position);
        // }

        public override void Hide()
        {
            equipmentController.Hide();
            skillsUI.Hide();
            base.Hide();
        }

        public void Select()
        {
            frame.material = SelectedBorderMaterial;
        }

        public void Deselect()
        {
            frame.material = defaultMateril;
        }
    }
}