using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MetaUpgradeDetailPanelController : MonoBehaviour
{
    public static MetaUpgradeDetailPanelController Instance;
     public MetaUpgrade metaUpgrade;

    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public GameObject currentTextGo;
    public GameObject upgradeTextGo;
    public GameObject levelTextGo;
    public GameObject costTextGo;

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
        Debug.Log("Show Detail Page");
        gameObject.SetActive(true);
        this.metaUpgrade = metaUpgrade;
        UpdateUI();
    }

    public void Hide()
    {
        Debug.Log("Hide Detail Page");
        gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        name.text = metaUpgrade.blueprint.name;
        description.text = metaUpgrade.blueprint.Description;
        cost.text = "" + metaUpgrade.blueprint.costToLevel[0];
        level.text = "" + metaUpgrade.level + "/" + metaUpgrade.blueprint.maxLevel;
        icon.sprite = metaUpgrade.blueprint.icon;
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


        if (metaUpgrade.locked)
        {
            Debug.Log("Locked");
            icon.sprite = lockedSprite;
            learnButton.gameObject.SetActive(false);
            level.transform.gameObject.SetActive(false);
            cost.transform.gameObject.SetActive(false);
            description.transform.gameObject.SetActive(false);
            name.transform.gameObject.SetActive(false);
            costTextGo.gameObject.SetActive(false);
        }
        else if (metaUpgrade.IsMaxed())
        {
            learnButtonText.text = "Maxed";
            learnButton.interactable = false;
            currentTextGo.gameObject.SetActive(true);
            levelTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(false);
        }
        else if (Player.Instance.HasLearned(metaUpgrade))
        {
            learnButtonText.text = "Upgrade";
            currentTextGo.gameObject.SetActive(true);
            upgradeTextGo.gameObject.SetActive(true);
            levelTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(false);
        }
        else
        {
            levelTextGo.gameObject.SetActive(true);
            upgradeTextGo.gameObject.SetActive(true);
            costTextGo.gameObject.SetActive(true);
        }
        Debug.Log("Weird End Page");
    }

    void OnEnable()
    {
        if (metaUpgrade == null)
            return;
        UpdateUI();
    }

    public void LearnClicked()
    {
        Player.Instance.LearnMetaUpgrade(metaUpgrade);
        UpdateUI();
        Debug.Log("Learn Clicked!");
    }
}