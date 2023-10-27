using System;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    
    public class ChooseSkillButtonUI : MonoBehaviour
    {
        
        public Skill skill;
        public TextMeshProUGUI description;
        public new TextMeshProUGUI name;
        [SerializeField] TextMeshProUGUI level;
        [SerializeField] GameObject levelUpgArrow;
        [SerializeField] private TMP_ColorGradient defaultColorGradient;
        [SerializeField] private TMP_ColorGradient upgColorGradient;
        public GameObject linePrefab;
        public Transform lineContainer;
        [SerializeField] private GameObject skillButtonPrefab;
        [SerializeField] private GameObject activeSkillButtonPrefab;
        [SerializeField] private GameObject combatSkillButtonPrefab;
        [SerializeField] private Transform skillUIParent;

       
        [SerializeField] TMP_ColorGradient commonColorGradient;
        [SerializeField] TMP_ColorGradient rareColorGradient;
        [SerializeField] TMP_ColorGradient epicColorGradient;
        [SerializeField] TMP_ColorGradient legendaryColorGradient;
        [SerializeField] TMP_ColorGradient mythicColorGradient;


        [SerializeField] private Image selectedBorder;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private GameObject lockedOverlay;
        [SerializeField] private TextMeshProUGUI lockedText;
        [SerializeField] private Image textureBackground;
        [SerializeField] private Image imageBackground;
        [SerializeField] private Sprite textureBackgroundPassive;
        [SerializeField] Sprite textureBackgroundCombat;
        [SerializeField] private Sprite textureBackgroundActive;
        [SerializeField] private Image frameBackground;
        [SerializeField] private Sprite frameBackgroundPassive;
        [SerializeField] Sprite frameBackgroundCombat;
        [SerializeField] private Sprite frameBackgroundActive;
     
     
        [SerializeField] private UIAreaTypePreview areaTypePreview;

       
       
        [SerializeField] private GameObject moveSkill;
        public event Action<ChooseSkillButtonUI> OnClick;
      
        private Vector3 defaultSkillPosition;
        private bool locked = false;
        private bool blessed;
        public bool upgrade;
        public void OnEnable()
        {
            defaultSkillPosition = moveSkill.GetComponent<RectTransform>().anchoredPosition;
            // if (testSkill != null)
            // {
            //     Debug.Log("ONENABLE");
            //     SetSkill(testSkill.Create());
            // }
        }
        void InstantiateSkill(Skill skill, bool blessing, bool showDeleteIfFull)
        {
            skillUIParent.DeleteAllChildren();
            var prefab = skillButtonPrefab;
            if (skill.activeMixins.Count > 0)
                prefab = activeSkillButtonPrefab;
            else if (skill.CombatSkillMixin != null)
                prefab = combatSkillButtonPrefab;
            var go = Instantiate(prefab, skillUIParent);
            var skillUI = go.GetComponent<SkillUI>();
            skillUI.SetSkill(skill, true, blessing);
       
        }
        public void SetSkill(Skill skill, bool blessed,bool upgrade, bool locked =false)
        {
            if(defaultSkillPosition==Vector3.zero)
                defaultSkillPosition = moveSkill.GetComponent<RectTransform>().anchoredPosition;
            gameObject.SetActive(true);
            this.skill = skill;
           // this.skill.Level++;
           this.blessed = blessed;
           this.upgrade = upgrade;
            this.locked = locked;
            if (locked && upgrade)
                this.locked = false;
            moveSkill.GetComponent<RectTransform>().anchoredPosition = defaultSkillPosition;
            moveSkill.gameObject.SetActive(true);
            UpdateUI();

        }

        
        public void Hide()
        {
            gameObject.SetActive(false);
            moveSkill.GetComponent<RectTransform>().anchoredPosition = defaultSkillPosition;
            moveSkill.gameObject.SetActive(true);
        }

        
        public void MoveSkill(Vector3 position, Action after)
        {
       
            LeanTween.move(moveSkill, position, .7f).setEaseOutQuad().setOnComplete(() =>
            {
                after?.Invoke();
                moveSkill.gameObject.SetActive(false);
            });
        }
        void UpdateUI()
        {
            if (skill == null)
            {
                gameObject.SetActive(false);
                return;
            }

            InstantiateSkill(skill,blessed, false);
           

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            
            lockedOverlay.gameObject.SetActive(locked);
            lockedText.gameObject.SetActive(locked);
            
            
            lineContainer.DeleteAllChildrenImmediate();
            name.text = skill.Name;
            description.text = skill.Description;
            bool isActiveMixin = skill.IsActive();

            if (!isActiveMixin)
            {
                areaTypePreview.Hide();
            }
            if (isActiveMixin)
            {
               
                if (skill.FirstActiveMixin is PositionTargetSkillMixin ptsm)
                {
                    var castRange = skill.Level==0?ptsm.GetRange(skill.Level):ptsm.GetRange(skill.Level-1);
                    var upgcastRange= ptsm.GetRange(skill.Level);
                    // var damage= skill.Level==0?ptsm.GetPower(skill.Level):ptsm.GetPower(skill.Level-1);
                    // var upgDamage= ptsm.GetPower(skill.Level);
                    var size= skill.Level==0?ptsm.GetSize(skill.Level):ptsm.GetSize(skill.Level-1);
                    var upgSize= ptsm.GetSize(skill.Level);

                    areaTypePreview.Show(ptsm.TargetArea, size, EffectType.Heal,
                            upgSize, ptsm.Rooted);
                    
                    var castline = GameObject.Instantiate(linePrefab, lineContainer);
                    bool castUpgrade = castRange != upgcastRange;
                    castline.GetComponent<UISkillEffectLine>().SetValues("Castrange: ",""+(castUpgrade?upgcastRange:castRange),castUpgrade);
                    // line = GameObject.Instantiate(linePrefab, lineContainer);
                    // line.GetComponent<UISkillEffectLine>().SetValues("Damage: ",""+damage,""+upgDamage);
                    var effectDescriptions = ptsm.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                    foreach (var effectDescription in effectDescriptions)
                    {
                        var line = GameObject.Instantiate(linePrefab, lineContainer);
                        bool upg = effectDescription.upgValue != effectDescription.value;
                        line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, upg?effectDescription.upgValue: effectDescription.value, upg);
                    }
                }
                else if (skill.FirstActiveMixin is SingleTargetMixin stsm)
                {
                    areaTypePreview.Hide();
                    var castRange = skill.Level==0?stsm.GetRange(skill.Level):stsm.GetRange(skill.Level-1);
                    var upgcastRange= stsm.GetRange(skill.Level);


                    var castLine = GameObject.Instantiate(linePrefab, lineContainer);
                    castLine.GetComponent<UISkillEffectLine>().SetValues("Castrange: ",""+castRange,upgcastRange!=castRange);
                    
                    var effectDescriptions = stsm.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                    foreach (var effectDescription in effectDescriptions)
                    {
                        var line = GameObject.Instantiate(linePrefab, lineContainer);
                        bool upg = effectDescription.upgValue != effectDescription.value;
                        line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, upg?effectDescription.upgValue: effectDescription.value, upg);
                    }
                }
                else if (skill.FirstActiveMixin is SelfTargetSkillMixin sts)
                {
                    areaTypePreview.Hide();

                    var effectDescriptions = sts.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                    foreach (var effectDescription in effectDescriptions)
                    {
                        var line = GameObject.Instantiate(linePrefab, lineContainer);
                        bool upg = effectDescription.upgValue != effectDescription.value;
                        line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, upg?effectDescription.upgValue: effectDescription.value, upg);
                    }
                }

                
            }

            if (skill.CombatSkillMixin != null)
            {
                var effectDescriptions = skill.CombatSkillMixin.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                foreach (var effectDescription in effectDescriptions)
                {
                    if(effectDescription==null)
                        continue;
                    var line = GameObject.Instantiate(linePrefab, lineContainer);
                    bool upg = effectDescription.upgValue != effectDescription.value;
                    line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, upg?effectDescription.upgValue: effectDescription.value, upg);
                }
            }
            foreach (var passive in skill.passiveMixins)
            {
                var effectDescriptions = passive.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                foreach (var effectDescription in effectDescriptions)
                {
                    var line = GameObject.Instantiate(linePrefab, lineContainer);
                    Debug.Log(effectDescription.label+" "+effectDescription.value+" "+effectDescription.upgValue);
                    bool upg = effectDescription.upgValue != effectDescription.value;
                    line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label,upg?effectDescription.upgValue: effectDescription.value, upg);
                }
            }

            level.text = "" + (upgrade?skill.Level+2:skill.Level+1);
            level.colorGradientPreset = upgrade ? upgColorGradient:defaultColorGradient;
            levelUpgArrow.gameObject.SetActive(upgrade);
           // icon.sprite = skill.GetIcon();
            level.transform.gameObject.SetActive(true);

            description.transform.gameObject.SetActive(true);
            name.transform.gameObject.SetActive(true);

            switch (skill.Tier)
            {
                case 0: //Mythic 
                    Debug.Log("Mythic");
                    
                    rarityText.SetText("Mythic");
                    rarityText.colorGradientPreset = mythicColorGradient;
                    
                    break;
                case 1: //Legendary 
                   
                    rarityText.SetText("Legendary");
                    rarityText.colorGradientPreset = legendaryColorGradient;
                   
                    break;
                case 2: //Epic 
                 
                    rarityText.SetText("Epic");
                    rarityText.colorGradientPreset = epicColorGradient;
                
                    break;
                case 3: //Rare 
                    
                    rarityText.SetText("Rare");
                    rarityText.colorGradientPreset = rareColorGradient;
                   
                    break;
                case 4: //Common 
                   
                    rarityText.SetText("Common");
                    rarityText.colorGradientPreset = commonColorGradient;
                  
                    break;
            }

            // switch (skill.SkillType)
            // {
            //     case SkillType.Common: //Mythic 
            //         imageBackground.color = imageBackgroundColorCommon;
            //         textureBackground.color = textureBackgroundColorCommon;
            //         break;
            // }

            if (skill.FirstActiveMixin != null)
            {
                imageBackground.sprite = textureBackgroundActive;
                frameBackground.sprite = frameBackgroundActive;
            }
            else if (skill.CombatSkillMixin != null)
            {
                imageBackground.sprite = textureBackgroundCombat;
                frameBackground.sprite = frameBackgroundCombat;
            }
            else
            {
                imageBackground.sprite = textureBackgroundPassive;
                frameBackground.sprite = frameBackgroundPassive;
            }
            // if (skill is Blessing)
            // {
            //     imageBackground.color = imageBackgroundColorBlessing;
            //     textureBackground.color = textureBackgroundColorBlessing;
            //     frameIcon.material = blessingFrameMaterial;
            //     frameIcon.color = Color.white;
            //     frameButton.color = Color.white;
            //     frameButton.material = blessingFrameMaterial;
            // }
            // else if (skill is Curse)
            // {
            //     imageBackground.color = imageBackgroundColorCurse;
            //     textureBackground.color = textureBackgroundColorCurse;
            //     frameIcon.material = curseFrameMaterial;
            //     frameIcon.color = Color.white;
            //     frameButton.color = Color.white;
            //     frameButton.material = curseFrameMaterial;
            // }
            
          
        }


      

        public void Clicked()
        {
            OnClick?.Invoke(this);
            UpdateUI();
        }

        public void Select()
        {
            LeanTween.scale(gameObject, selectedScale, .1f).setEaseOutQuad();
            selectedBorder.enabled = true;
            //transform.localScale = selectedScale;
        }

        [SerializeField] private Vector3 selectedScale;

        public void Deselect()
        {
            selectedBorder.enabled = false;
            LeanTween.scale(gameObject, new Vector3(1,1,1), .1f).setEaseInQuad();
            //transform.localScale = new Vector3(1,1,1);
        }
    }

    public enum SkillType
    {
        Common,
        Class,
        Weapon,
        Blessing,
        Curse
    }
}