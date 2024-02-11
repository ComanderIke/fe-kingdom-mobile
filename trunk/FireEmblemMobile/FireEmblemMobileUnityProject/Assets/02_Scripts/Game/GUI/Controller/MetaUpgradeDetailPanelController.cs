using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
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
        cost.text = "" + metaUpgradeBP.costToLevel[0];
       
        icon.sprite = metaUpgradeBP.icon;
        level.transform.gameObject.SetActive(true);
        cost.transform.gameObject.SetActive(true);
        flameLevelReqValue.text = ""+metaUpgradeBP.requiredFlameLevel;
        description.transform.gameObject.SetActive(true);
        nameText.transform.gameObject.SetActive(true);
        learnButton.gameObject.SetActive(true);
        learnButton.interactable = true;
        learnButtonText.text = "Learn";
        currentTextGo.gameObject.SetActive(false);
        upgradeTextGo.gameObject.SetActive(false);
        levelTextGo.gameObject.SetActive(false);
        level.text = "" + (metaUpgrade==null?"0":metaUpgrade.level )+ "/" + metaUpgradeBP.maxLevel;


        // if (metaUpgrade.locked)
        // {
        //     Debug.Log("Locked");
        //     icon.sprite = lockedSprite;
        //     learnButton.gameObject.SetActive(false);
        //     level.transform.gameObject.SetActive(false);
        //     cost.transform.gameObject.SetActive(false);
        //     description.transform.gameObject.SetActive(false);
        //     nameText.transform.gameObject.SetActive(false);
        //     costTextGo.gameObject.SetActive(false);
        // }
        // else if (metaUpgrade.IsMaxed())
        // {
        //     learnButtonText.text = "Maxed";
        //     learnButton.interactable = false;
        //     currentTextGo.gameObject.SetActive(true);
        //     levelTextGo.gameObject.SetActive(true);
        //     costTextGo.gameObject.SetActive(false);
        // }
        // else if (Player.Instance.HasLearned(metaUpgrade))
        // {
        //     learnButtonText.text = "Upgrade";
        //     currentTextGo.gameObject.SetActive(true);
        //     upgradeTextGo.gameObject.SetActive(true);
        //     levelTextGo.gameObject.SetActive(true);
        //     costTextGo.gameObject.SetActive(false);
        // }
        // else
        // {
        //     levelTextGo.gameObject.SetActive(true);
        //     upgradeTextGo.gameObject.SetActive(true);
        //     costTextGo.gameObject.SetActive(true);
        // }
        Debug.Log("Weird End Page");
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