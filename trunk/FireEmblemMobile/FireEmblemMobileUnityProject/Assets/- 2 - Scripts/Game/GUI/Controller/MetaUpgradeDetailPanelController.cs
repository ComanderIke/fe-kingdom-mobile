using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class MetaUpgradeDetailPanelController : MonoBehaviour
{
    public MetaUpgrade MetaUpgrade;

    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI name;
    public TextMeshProUGUI level;

    public Button learnButton;
    public TextMeshProUGUI learnButtonText;
    public Image icon;

    public Sprite lockedSprite;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (MetaUpgrade == null)
            return;
        name.text = MetaUpgrade.name;
        description.text = MetaUpgrade.Description;
        cost.text = ""+MetaUpgrade.costToLevel[0];
        level.text = "" + MetaUpgrade.level + "/" + MetaUpgrade.maxLevel;
        icon.sprite = MetaUpgrade.icon;
        level.transform.gameObject.SetActive(true); 
        cost.transform.gameObject.SetActive(true);
        description.transform.gameObject.SetActive(true);
        name.transform.gameObject.SetActive(true);
        learnButton.gameObject.SetActive(true);
        learnButton.interactable = true;
        learnButtonText.text = "Learn";
        switch (MetaUpgrade.state)
        {
            case UpgradeState.Learned:   learnButtonText.text = "Upgrade"; break;
            case UpgradeState.NotLearned: break;
            case UpgradeState.Maxed:
                learnButtonText.text = "Maxed";
                learnButton.interactable = false;
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

    public void LearnClicked()
    {
        Debug.Log("Learn Clicked!");
    }
}
