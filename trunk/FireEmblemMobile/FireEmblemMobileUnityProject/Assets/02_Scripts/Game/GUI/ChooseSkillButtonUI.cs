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

        public Sprite lockedSprite;
        [SerializeField] private GameObject lockedOverlay;

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
            Debug.Log("UpdateSkillButton");
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
            
          
        }



        public void Clicked()
        {
            UpdateUI();
        }
    }
}