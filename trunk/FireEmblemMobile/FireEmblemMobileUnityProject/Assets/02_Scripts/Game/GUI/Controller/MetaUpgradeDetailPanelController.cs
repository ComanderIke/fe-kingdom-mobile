using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Players;
using Game.GameResources;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MetaUpgradeDetailPanelController : MonoBehaviour
{
    
    private MetaUpgradeBP metaUpgradeBP;

    public GameObject detailLinePrefab;
    public Transform lineContainer;
    public LayoutGroup layout;
    public int lineSiblingIndex = 3;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI costLabel;  
    public Image costResourceIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI level;
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

    public void UpdateUI()
    {
        if (metaUpgradeBP == null)
            return;
        learnButtonText.text = "<bounce>Upgrade";
        var metaUpgrade = Player.Instance.MetaUpgradeManager.GetUpgrade(metaUpgradeBP);
        nameText.text = metaUpgradeBP.name;
        description.text = metaUpgradeBP.Description;
        if (Player.Instance.CanAfford(metaUpgradeBP.GetCost(metaUpgrade==null?0:metaUpgrade.level+1), metaUpgradeBP.costType))
        {
            costLabel.colorGradientPreset = null;
            cost.colorGradientPreset = null;
            learnButton.interactable = true;
            learnButtonText.text = "<bounce>Upgrade";
            MyDebug.LogTest("LearnButton interactable");
        }
        else
        {
            costLabel.colorGradientPreset = red;
            cost.colorGradientPreset = red;
            learnButton.interactable = false;
            learnButtonText.text = "</bounce>Upgrade";
            MyDebug.LogTest("LearnButton not interactable");
        }

        costResourceIcon.sprite = metaUpgradeBP.costType == MetaUpgradeCost.Grace
            ?
            GameAssets.Instance.visuals.Icons.Grace
            :
            metaUpgradeBP.costType == MetaUpgradeCost.CorruptedGrace
                ? GameAssets.Instance.visuals.Icons.CorruptedGrace
                : GameAssets.Instance.visuals.Icons.DeathStones;
        cost.text = "" + metaUpgradeBP.GetCost((metaUpgrade==null?0:metaUpgrade.level+1));
      
        if (Player.Instance.GetFlameLevel() < metaUpgradeBP.GetRequiredFlameLevel(metaUpgrade==null?0:metaUpgrade.level+1))
        {
            flameLevelReqLabel.colorGradientPreset = red;
            flameLevelReqValue.colorGradientPreset = red;
            learnButton.interactable = false;
            learnButtonText.text = "</bounce>Upgrade";

        }
        else
        {
            flameLevelReqLabel.colorGradientPreset = null;
            flameLevelReqValue.colorGradientPreset = null;
        }
        flameLevelReqValue.text = "Flame Level "+metaUpgradeBP.GetRequiredFlameLevel(metaUpgrade==null?0:metaUpgrade.level+1);
       
        icon.sprite = metaUpgradeBP.icon;
        level.transform.gameObject.SetActive(true);
        cost.transform.gameObject.SetActive(true);
        description.transform.gameObject.SetActive(true);
        nameText.transform.gameObject.SetActive(true);
        lineContainer.DeleteAllChildren();
        levelTextGo.gameObject.SetActive(false);
        level.text = "" + (metaUpgrade==null?"0":metaUpgrade.level+1 )+ "/" + metaUpgradeBP.maxLevel;
        
        var effects=metaUpgradeBP.GetEffectDescriptions(metaUpgrade==null?0:metaUpgrade.level);
        flameLevelReqLabel.gameObject.SetActive(true);
        flameLevelReqValue.gameObject.SetActive(true);
        if (metaUpgrade!=null&&metaUpgrade.IsMaxed())
        {
            learnButtonText.text = "</bounce>Maxed";
            learnButton.interactable = false;
            costTextGo.gameObject.SetActive(false);
            flameLevelReqLabel.gameObject.SetActive(false);
            flameLevelReqValue.gameObject.SetActive(false);
            foreach (var effect in effects)
            {
                var go = Instantiate(detailLinePrefab, lineContainer);
                go.GetComponent<UIMetaUpgradeLine>().SetValues(effect.label, effect.value, false);

            }
        }
        else if (Player.Instance.HasLearned(metaUpgrade))
        {
           
            levelTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(true);
          
            foreach (var effect in effects)
            {
                bool upgrade = false;
                var go = Instantiate(detailLinePrefab, lineContainer);
                bool upg = effect.upgValue != effect.value && upgrade;
                go.GetComponent<UIMetaUpgradeLine>().SetValues(effect.label, effect.value, false);

            }
            var effectsUpgrade=metaUpgradeBP.GetEffectDescriptions(metaUpgrade.level+1);
            foreach (var effect in effectsUpgrade)
            {
                bool upgrade = true;
                var go = Instantiate(detailLinePrefab, lineContainer);
                //bool upg = effect.upgValue != effect.value && upgrade;
                go.GetComponent<UIMetaUpgradeLine>().SetValues("Upgrade: ", effect.value, true);

            }
        }
        else
        {
            levelTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(true);
            foreach (var effect in effects)
            {
               
                var go = Instantiate(detailLinePrefab, lineContainer);
                go.GetComponent<UIMetaUpgradeLine>().SetValues(effect.label, effect.value, false);

            }
        }

       
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());

    }

    void OnEnable()
    {
        if (metaUpgradeBP == null)
            return;
        UpdateUI();
    }

    public void LearnClicked()
    {
        Player.Instance.LearnMetaUpgrade(metaUpgradeBP); 
        UpdateUI();
        Debug.Log("Learn Clicked!");
    }

    public void SetButtonInteractable(bool value)
    {
        // learnButton.interactable = value;
    }
}