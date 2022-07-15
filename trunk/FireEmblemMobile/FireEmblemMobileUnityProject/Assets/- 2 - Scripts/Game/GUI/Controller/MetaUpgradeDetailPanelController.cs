using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MetaUpgradeDetailPanelController : MonoBehaviour
{
    public static MetaUpgradeDetailPanelController Instance;
    public MetaUpgrade MetaUpgrade;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void Show(MetaUpgrade metaUpgrade)
    {
        gameObject.SetActive(true);
        this.MetaUpgrade = metaUpgrade;
        UpdateUI();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        name.text = MetaUpgrade.name;
        description.text = MetaUpgrade.Description;
        cost.text = "" + MetaUpgrade.costToLevel[0];
        level.text = "" + MetaUpgrade.level + "/" + MetaUpgrade.maxLevel;
        icon.sprite = MetaUpgrade.icon;
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
       
      

        switch (MetaUpgrade.state)
        {
            case UpgradeState.Learned:
                learnButtonText.text = "Upgrade";
                currentTextGo.gameObject.SetActive(true);
                upgradeTextGo.gameObject.SetActive(true);
                levelTextGo.gameObject.SetActive(true);
                break;
            case UpgradeState.NotLearned: levelTextGo.gameObject.SetActive(true);
                upgradeTextGo.gameObject.SetActive(true);break;
            case UpgradeState.Maxed:
                learnButtonText.text = "Maxed";
                learnButton.interactable = false;
                currentTextGo.gameObject.SetActive(true);
                levelTextGo.gameObject.SetActive(true);
                break;
            case UpgradeState.Locked:
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
        if (MetaUpgrade == null)
            return;
        UpdateUI();
    }

    public void LearnClicked()
    {
        Debug.Log("Learn Clicked!");
    }
}