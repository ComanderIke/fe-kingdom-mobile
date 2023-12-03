using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace LostGrace
{
    public class SkillUpgradeDetailPanel : MonoBehaviour
    {
        public static SkillUpgradeDetailPanel Instance;
        public SkillTreeEntry SkillEntry;
        [FormerlySerializedAs("SkillUI")] public SkillTreeEntryUI skillTreeEntryUI;
        private Unit user;

        public TextMeshProUGUI description;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI level;
        public TextMeshProUGUI currentText;
        public TextMeshProUGUI upgText;
        public GameObject currentTextGo;
        public GameObject upgradeTextGo;
        public GameObject levelTextGo;

        public Button learnButton;
        public TextMeshProUGUI learnButtonText;
        public Image icon;

        public Sprite lockedSprite;
        void Start()
        {
            Instance = this;
        }

     public void Show( SkillTreeEntryUI skillTreeEntryUI, Unit user)
    {

        this.skillTreeEntryUI = skillTreeEntryUI;
        SkillEntry = skillTreeEntryUI.skillEntry;
        this.user = user;
        UpdateUI();
       
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        if (SkillEntry == null || SkillEntry.Skill == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        nameText.text = SkillEntry.Skill.Name;
        description.text =  SkillEntry.Skill.Description;
      
        level.text = "" +  SkillEntry.Skill.Level;
        icon.sprite =  SkillEntry.Skill.GetIcon();
        level.transform.gameObject.SetActive(true);
   
        description.transform.gameObject.SetActive(true);
        nameText.transform.gameObject.SetActive(true);
        learnButton.gameObject.SetActive(true);
        learnButton.interactable = true;
        learnButtonText.text = "Learn";
        currentTextGo.gameObject.SetActive(false);
        upgradeTextGo.gameObject.SetActive(false);
        levelTextGo.gameObject.SetActive(false);
        currentText.SetText(SkillEntry.Skill.CurrentUpgradeText());
        upgText.SetText(SkillEntry.Skill.NextUpgradeText());
   
        Debug.Log("SkillState; "+SkillEntry.SkillState);

        switch (SkillEntry.SkillState)
        {
            case SkillState.Learned:
                learnButtonText.text = "Upgrade";
                currentTextGo.gameObject.SetActive(true);
                upgradeTextGo.gameObject.SetActive(true);
                levelTextGo.gameObject.SetActive(true);
                break;
            case SkillState.Learnable: levelTextGo.gameObject.SetActive(true);
                upgradeTextGo.gameObject.SetActive(true);break;
            case SkillState.NotLearnable: levelTextGo.gameObject.SetActive(true);
                upgradeTextGo.gameObject.SetActive(true);
                learnButton.interactable = false;
                break;
            case SkillState.Maxed:
                learnButtonText.text = "Maxed";
                learnButton.interactable = false;
                currentTextGo.gameObject.SetActive(true);
                levelTextGo.gameObject.SetActive(true);
                break;
            case SkillState.Locked:
                icon.sprite = lockedSprite;
                learnButton.gameObject.SetActive(false);
                level.transform.gameObject.SetActive(false);
                description.transform.gameObject.SetActive(false);
                nameText.transform.gameObject.SetActive(false);
               
                break;
        }
        
    }

    void OnEnable()
    {
        
        UpdateUI();
    }

    public void LearnClicked()
    {
        
        skillTreeEntryUI.LearnClicked();
        UpdateUI();
    
      
    }
    }
}
