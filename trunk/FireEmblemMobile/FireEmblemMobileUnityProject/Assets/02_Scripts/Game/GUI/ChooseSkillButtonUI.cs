﻿using System;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    
    public class ChooseSkillButtonUI : MonoBehaviour
    {
        public SkillBp testSkill;
        public Skill skill;
        public TextMeshProUGUI description;
        public new TextMeshProUGUI name;
        public TextMeshProUGUI level;
        public GameObject linePrefab;
        public Transform lineContainer;
      
   

        public Image icon;
        [SerializeField] TMP_ColorGradient commonColorGradient;
        [SerializeField] TMP_ColorGradient rareColorGradient;
        [SerializeField] TMP_ColorGradient epicColorGradient;
        [SerializeField] TMP_ColorGradient legendaryColorGradient;
        [SerializeField] TMP_ColorGradient mythicColorGradient;
        [SerializeField] Color commonColorFrame;
        [SerializeField] Color rareColorFrame;
        [SerializeField] Color epicColorFrame;
        [SerializeField] Color legendaryColorFrame;
        [SerializeField] Color mythicColorFrame;
        [SerializeField] private Image iconFrame;
        [SerializeField] private TextMeshProUGUI rarityText;
        public Sprite lockedSprite;
        [SerializeField] private GameObject lockedOverlay;
        [SerializeField] private Image textureBackground;
        [SerializeField] private Image imageBackground;
        [SerializeField] private Color textureBackgroundColorCommon;
        [SerializeField] Color imageBackgroundColorCommon;
        [SerializeField] private Color textureBackgroundColorClass;
        [SerializeField] Color imageBackgroundColorClass;
        [SerializeField] private Color textureBackgroundColorWeapon;
        [SerializeField] Color imageBackgroundColorWeapon;
        [SerializeField] private UIAreaTypePreview areaTypePreview;
        [SerializeField] private TextMeshProUGUI hpCostText;
        [SerializeField] private TextMeshProUGUI usesText;
        [SerializeField] private Color upgTextColor;
        [SerializeField] private Color hpCostTextColor;
        [SerializeField] private Color usesTextColor;
        [SerializeField] private GameObject hpCostGo;
        [SerializeField] private GameObject usesGo;
        public void OnEnable()
        {
            if (testSkill != null)
            {
                Debug.Log("ONENABLE");
                SetSkill(testSkill.Create());
            }
        }

        public void SetSkill(Skill skill)
        {

            this.skill = skill;
            this.skill.Level++;
          
            UpdateUI();

        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        void UpdateUI()
        {
            if (skill == null)
            {
                gameObject.SetActive(false);
                return;
            }

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            
            lineContainer.DeleteAllChildrenImmediate();
            name.text = skill.Name;
            description.text = skill.Description;
            bool isActiveMixin = skill.activeMixin != null;
            hpCostGo.SetActive(isActiveMixin);
            usesGo.SetActive(isActiveMixin);

            if (!isActiveMixin)
            {
                areaTypePreview.Hide();
            }
            if (isActiveMixin)
            {
                hpCostText.text = "" + skill.activeMixin.hpCostPerLevel[skill.Level];
                usesText.text = "" + skill.ActiveMixinUses + "/" +
                                skill.activeMixin.maxUsesPerLevel[skill.Level];
                if(skill.Level>=1&&skill.activeMixin.maxUsesPerLevel[skill.Level]>skill.activeMixin.maxUsesPerLevel[skill.Level-1])
                    usesText.color = upgTextColor;
                else
                {
                    usesText.color = usesTextColor;
                }
                if(skill.Level>=1&&skill.activeMixin.hpCostPerLevel[skill.Level]>skill.activeMixin.hpCostPerLevel[skill.Level-1])
                    hpCostText.color = upgTextColor;
                else
                {
                    hpCostText.color = hpCostTextColor;
                }
                if (skill.activeMixin is PositionTargetSkillMixin ptsm)
                {
                    var castRange = skill.Level==0?ptsm.GetRange(skill.Level):ptsm.GetRange(skill.Level-1);
                    var upgcastRange= ptsm.GetRange(skill.Level);
                    var damage= skill.Level==0?ptsm.GetPower(skill.Level):ptsm.GetPower(skill.Level-1);
                    var upgDamage= ptsm.GetPower(skill.Level);
                    var size= skill.Level==0?ptsm.GetSize(skill.Level):ptsm.GetSize(skill.Level-1);
                    var upgSize= ptsm.GetSize(skill.Level);

                    areaTypePreview.Show(ptsm.TargetArea, size, EffectType.Heal,
                            upgSize, ptsm.Rooted);
                    
                    var line = GameObject.Instantiate(linePrefab, lineContainer);
                    line.GetComponent<UISkillEffectLine>().SetValues("Castrange: ",""+castRange,""+upgcastRange);
                    line = GameObject.Instantiate(linePrefab, lineContainer);
                    line.GetComponent<UISkillEffectLine>().SetValues("Damage: ",""+damage,""+upgDamage);
                }

                
            }

            foreach (var passive in skill.passiveMixins)
            {
                var effectDescriptions = passive.GetEffectDescription(skill.Level-1);
                foreach (var effectDescription in effectDescriptions)
                {
                    var line = GameObject.Instantiate(linePrefab, lineContainer);
                    line.GetComponent<UISkillEffectLine>().SetValues(effectDescription.label, effectDescription.value, effectDescription.upgValue);
                }
            }

            level.text = "" + skill.Level;
            icon.sprite = skill.GetIcon();
            level.transform.gameObject.SetActive(true);

            description.transform.gameObject.SetActive(true);
            name.transform.gameObject.SetActive(true);

            switch (skill.Tier)
            {
                case 0: //Mythic 
                    Debug.Log("Mythic");
                    iconFrame.color = mythicColorFrame;
                    rarityText.SetText("Mythic");
                    rarityText.colorGradientPreset = mythicColorGradient;
                    name.colorGradientPreset = mythicColorGradient;
                    break;
                case 1: //Legendary 
                    iconFrame.color = legendaryColorFrame;
                    rarityText.SetText("Legendary");
                    rarityText.colorGradientPreset = legendaryColorGradient;
                    name.colorGradientPreset = legendaryColorGradient;
                    break;
                case 2: //Epic 
                    iconFrame.color = epicColorFrame;
                    rarityText.SetText("Epic");
                    rarityText.colorGradientPreset = epicColorGradient;
                    name.colorGradientPreset = epicColorGradient;
                    break;
                case 3: //Rare 
                    iconFrame.color = rareColorFrame;
                    rarityText.SetText("Rare");
                    rarityText.colorGradientPreset = rareColorGradient;
                    name.colorGradientPreset = rareColorGradient;
                    break;
                case 4: //Common 
                    iconFrame.color = commonColorFrame;
                    rarityText.SetText("Common");
                    rarityText.colorGradientPreset = commonColorGradient;
                    name.colorGradientPreset = commonColorGradient;
                    break;
            }

            switch (skill.SkillType)
            {
                case SkillType.Common: //Mythic 
                    imageBackground.color = imageBackgroundColorCommon;
                    textureBackground.color = textureBackgroundColorCommon;
                    break;
            }
            
          
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