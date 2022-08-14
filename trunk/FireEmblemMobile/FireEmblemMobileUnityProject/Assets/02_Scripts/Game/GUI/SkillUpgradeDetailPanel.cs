using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace LostGrace
{
    public class SkillUpgradeDetailPanel : MonoBehaviour
    {
        public static SkillUpgradeDetailPanel Instance;
        public SkillTreeEntry SkillEntry;
        public SkillUI SkillUI;
        private Unit user;

        public TextMeshProUGUI description;
        public TextMeshProUGUI cost;
        public TextMeshProUGUI name;
        public TextMeshProUGUI level;
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

     public void Show( SkillUI skillUI, Unit user)
    {
        gameObject.SetActive(true);

        this.SkillUI = skillUI;
        SkillEntry = skillUI.skillEntry;
        this.user = user;
        UpdateUI();
       
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        Debug.Log("UpdateDetail");
        if (SkillEntry == null || SkillEntry.skill == null)
            return;
        Debug.Log("NotNull");
        name.text = SkillEntry.skill.name;
        description.text =  SkillEntry.skill.Description;
        cost.text = "" + 1;
        level.text = "" +  SkillEntry.skill.Level + "/" +  SkillEntry.skill.MaxLevel;
        icon.sprite =  SkillEntry.skill.GetIcon();
        level.transform.gameObject.SetActive(true);
        cost.transform.gameObject.SetActive(true);
        description.transform.gameObject.SetActive(true);
        name.transform.gameObject.SetActive(true);
        learnButton.gameObject.SetActive(true);
        learnButton.interactable = true;
        learnButtonText.text = "Learn";
        currentTextGo.gameObject.SetActive(false);
        upgradeTextGo.gameObject.SetActive(false);
        levelTextGo.gameObject.SetActive(false);
        user.SkillManager.UpdateSkillState(SkillEntry);
   
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
                upgradeTextGo.gameObject.SetActive(true);break;
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
                cost.transform.gameObject.SetActive(false);
                description.transform.gameObject.SetActive(false);
                name.transform.gameObject.SetActive(false);
               
                break;
        }
    }

    void OnEnable()
    {
        if ( SkillEntry == null)
            return;
        UpdateUI();
    }

    public void LearnClicked()
    {
        
        SkillUI.LearnClicked();
        UpdateUI();
    
      
    }
    }
}
