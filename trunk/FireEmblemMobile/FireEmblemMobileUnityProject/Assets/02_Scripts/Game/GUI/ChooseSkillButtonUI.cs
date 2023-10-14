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
        public TextMeshProUGUI level;
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

   
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private GameObject lockedOverlay;
        [SerializeField] private TextMeshProUGUI lockedText;
        [SerializeField] private Image textureBackground;
        [SerializeField] private Image imageBackground;
        [SerializeField] private Sprite textureBackgroundPassive;
        [SerializeField] Sprite textureBackgroundCombat;
        [SerializeField] private Sprite textureBackgroundActive;
     
     
        [SerializeField] private UIAreaTypePreview areaTypePreview;

       
       
        [SerializeField] private GameObject moveSkill;
      
        private Vector3 defaultSkillPosition;
        private bool locked = false;
        private bool blessed;
        public void OnEnable()
        {
            defaultSkillPosition = moveSkill.GetComponent<RectTransform>().anchoredPosition;
            // if (testSkill != null)
            // {
            //     Debug.Log("ONENABLE");
            //     SetSkill(testSkill.Create());
            // }
        }
        void InstantiateSkill(Skill skill, bool blessing,bool showDeleteIfFull)
        {
            skillUIParent.DeleteAllChildren();
            var prefab = skillButtonPrefab;
            if (skill.activeMixins.Count > 0)
                prefab = activeSkillButtonPrefab;
            else if (skill.CombatSkillMixin != null)
                prefab = combatSkillButtonPrefab;
            var go = Instantiate(prefab, skillUIParent);
            var skillUI = go.GetComponent<SkillUI>();
            skillUI.SetSkill(skill, false, blessing);
       
        }
        public void SetSkill(Skill skill, bool blessed, bool locked =false)
        {
            if(defaultSkillPosition==Vector3.zero)
                defaultSkillPosition = moveSkill.GetComponent<RectTransform>().anchoredPosition;
            gameObject.SetActive(true);
            this.skill = skill;
           // this.skill.Level++;
           this.blessed = blessed;
            this.locked = locked;
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
                    castline.GetComponent<UISkillEffectLine>().SetValues("Castrange: ",""+castRange,upgcastRange==castRange);
                    // line = GameObject.Instantiate(linePrefab, lineContainer);
                    // line.GetComponent<UISkillEffectLine>().SetValues("Damage: ",""+damage,""+upgDamage);
                    var effectDescriptions = ptsm.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                    foreach (var effectDescription in effectDescriptions)
                    {
                        var line = GameObject.Instantiate(linePrefab, lineContainer);
                        line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, effectDescription.value, effectDescription.upgValue==effectDescription.value);
                    }
                }
                else if (skill.FirstActiveMixin is SingleTargetMixin stsm)
                {
                    areaTypePreview.Hide();
                    var castRange = skill.Level==0?stsm.GetRange(skill.Level):stsm.GetRange(skill.Level-1);
                    var upgcastRange= stsm.GetRange(skill.Level);


                    var castLine = GameObject.Instantiate(linePrefab, lineContainer);
                    castLine.GetComponent<UISkillEffectLine>().SetValues("Castrange: ",""+castRange,upgcastRange==castRange);
                    
                    var effectDescriptions = stsm.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                    foreach (var effectDescription in effectDescriptions)
                    {
                        var line = GameObject.Instantiate(linePrefab, lineContainer);
                        line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, effectDescription.value, effectDescription.upgValue==effectDescription.value);
                    }
                }
                else if (skill.FirstActiveMixin is SelfTargetSkillMixin sts)
                {
                    areaTypePreview.Hide();

                    var effectDescriptions = sts.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                    foreach (var effectDescription in effectDescriptions)
                    {
                        var line = GameObject.Instantiate(linePrefab, lineContainer);
                        line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, effectDescription.value, effectDescription.upgValue==effectDescription.value);
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
                    line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, effectDescription.value, effectDescription.upgValue==effectDescription.value);
                }
            }
            foreach (var passive in skill.passiveMixins)
            {
                var effectDescriptions = passive.GetEffectDescription(Player.Instance.Party.ActiveUnit,skill.Level);
                foreach (var effectDescription in effectDescriptions)
                {
                    var line = GameObject.Instantiate(linePrefab, lineContainer);
                    line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, effectDescription.value, effectDescription.upgValue==effectDescription.value);
                }
            }

            level.text = "" + skill.Level;
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
            }
            else if (skill.CombatSkillMixin != null)
            {
                imageBackground.sprite = textureBackgroundCombat;
            }
            else
            {
                imageBackground.sprite = textureBackgroundPassive;
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
            UpdateUI();
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