using System;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    [ExecuteInEditMode]
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
            name.text = skill.Name;
            description.text = skill.Description;
            if (skill.activeMixin != null) 
            {
                if (skill.activeMixin is PositionTargetSkillMixin ptsm)
                {
                    areaTypePreview.Show(ptsm.TargetArea, ptsm.GetSize(skill.Level), EffectType.Heal, ptsm.GetSize(skill.Level));
                    var line = GameObject.Instantiate(linePrefab, lineContainer);
                    line.GetComponent<UISkillEffectLine>().SetValues("Castrange: ",ptsm.GetRange(skill.Level),ptsm.GetRange(skill.Level+1));
                    line = GameObject.Instantiate(linePrefab, lineContainer);
                    line.GetComponent<UISkillEffectLine>().SetValues("Damage: ",ptsm.GetPower(skill.Level),ptsm.GetRange(skill.Level+1));
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