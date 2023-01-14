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
        private Unit user;
        public TextMeshProUGUI description;
        public new TextMeshProUGUI name;
        public TextMeshProUGUI level;
        public TextMeshProUGUI currentText;
        public TextMeshProUGUI upgText;
        public GameObject currentTextGo;
        public GameObject upgradeTextGo;
        public GameObject levelTextGo;

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

    public void SetSkill(Skill skill, Unit user)
        {

            this.skill = skill;
            this.user = user;
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

            level.text = "" + skill.Level;
            icon.sprite = skill.GetIcon();
            level.transform.gameObject.SetActive(true);

            description.transform.gameObject.SetActive(true);
            name.transform.gameObject.SetActive(true);
            currentTextGo.gameObject.SetActive(false);
            upgradeTextGo.gameObject.SetActive(false);
            levelTextGo.gameObject.SetActive(false);
            //user.SkillManager.UpdateSkillState(skill);
            currentText.SetText(skill.CurrentUpgradeText());
            upgText.SetText(skill.NextUpgradeText());


            levelTextGo.gameObject.SetActive(true);
            upgradeTextGo.gameObject.SetActive(true);
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
        Weapon
    }
}