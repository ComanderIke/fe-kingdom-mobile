using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MetaUpgradeDetailPanelController : MonoBehaviour
{
    
    private MetaUpgradeBP metaUpgradeBP;

    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI costLabel;  
    public Image costResourceIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI level;
    public GameObject currentTextGo;
    public GameObject upgradeTextGo;
    public GameObject levelTextGo;
    public GameObject costTextGo;
    public TextMeshProUGUI flameLevelReqLabel;
    public TextMeshProUGUI flameLevelReqValue;
    public Button learnButton;
    public TextMeshProUGUI learnButtonText;
    public Image icon;
    public TMP_ColorGradient red;
    public TMP_ColorGradient blue;
    
    

    public void Show(MetaUpgradeBP metaUpgrade)
    {
        Debug.Log("Show Detail Page");
        gameObject.SetActive(true);
        this.metaUpgradeBP = metaUpgrade;
        UpdateUI();
    }

    public void Hide()
    {
        Debug.Log("Hide Detail Page");
        gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        if (metaUpgradeBP == null)
            return;
        var metaUpgrade = Player.Instance.MetaUpgradeManager.GetUpgrade(metaUpgradeBP);
        nameText.text = metaUpgradeBP.name;
        description.text = metaUpgradeBP.Description;
        if (Player.Instance.CanAfford(metaUpgradeBP.costToLevel[0], metaUpgradeBP.costType))
        {
            costLabel.colorGradientPreset = null;
            cost.colorGradientPreset = null;
            learnButton.interactable = true;
        }
        else
        {
            costLabel.colorGradientPreset = red;
            cost.colorGradientPreset = red;
            learnButton.interactable = false;
        }

        costResourceIcon.sprite = metaUpgradeBP.costType == MetaUpgradeCost.Grace
            ?
            GameAssets.Instance.visuals.Icons.Grace
            :
            metaUpgradeBP.costType == MetaUpgradeCost.CorruptedGrace
                ? GameAssets.Instance.visuals.Icons.CorruptedGrace
                : GameAssets.Instance.visuals.Icons.DeathStones;
        cost.text = "" + metaUpgradeBP.costToLevel[0];
        if (Player.Instance.GetFlameLevel() < metaUpgradeBP.requiredFlameLevel)
        {
            flameLevelReqLabel.colorGradientPreset = red;
            flameLevelReqValue.colorGradientPreset = red;

        }
        else
        {
            flameLevelReqLabel.colorGradientPreset = null;
            flameLevelReqValue.colorGradientPreset = null;
        }
        flameLevelReqValue.text = "Flame Level "+metaUpgradeBP.requiredFlameLevel;
        learnButtonText.text = "Upgrade";
        icon.sprite = metaUpgradeBP.icon;
        level.transform.gameObject.SetActive(true);
        cost.transform.gameObject.SetActive(true);
        description.transform.gameObject.SetActive(true);
        nameText.transform.gameObject.SetActive(true);
        currentTextGo.gameObject.SetActive(false);
        upgradeTextGo.gameObject.SetActive(false);
        levelTextGo.gameObject.SetActive(false);
        level.text = "" + (metaUpgrade==null?"0":metaUpgrade.level )+ "/" + metaUpgradeBP.maxLevel;
        if (metaUpgrade!=null&&metaUpgrade.IsMaxed())
        {
            learnButtonText.text = "Maxed";
            learnButton.interactable = false;
            costTextGo.gameObject.SetActive(false);
        }
        else if (Player.Instance.HasLearned(metaUpgrade))
        {
            currentTextGo.gameObject.SetActive(true);
            upgradeTextGo.gameObject.SetActive(true);
            levelTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(true);
        }
        else
        {
            levelTextGo.gameObject.SetActive(true);
            upgradeTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        if (metaUpgradeBP == null)
            return;
        UpdateUI();
    }

    public void LearnClicked()
    {
        // Player.Instance.LearnMetaUpgrade(metaUpgradeBP);
        // UpdateUI();
        Debug.Log("Learn Clicked!");
    }

    public void SetButtonInteractable(bool value)
    {
        learnButton.interactable = value;
    }
}