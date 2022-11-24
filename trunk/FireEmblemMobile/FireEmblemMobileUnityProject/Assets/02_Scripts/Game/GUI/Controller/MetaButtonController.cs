using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Game.GameActors.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class MetaButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public MetaUpgrade metaSkill;

    public TextMeshProUGUI header;
   
    public TextMeshProUGUI level;
    public Image background;
    public Image icon;
    public CanvasGroup CanvasGroup;
    public Color normalColor;
    public Color learnedColor;
    public Color secretBgColor;
    public Sprite secretSprite;
    public float secretAlpha = .5f;
    public Color secretIconColor;
    public Color normalIconColor;
    public Color maxTextColor;
    public Color normalTextColor;
   
    
    private void OnEnable()
    {
       

    }

    public void OnClick()
    {
        Debug.Log("Upgrade Clicked: "+metaSkill.blueprint.name);
        SelectCursorController.Instance.Show(gameObject);
        MetaUpgradeDetailPanelController.Instance.Show(metaSkill);
    }

    public void SetUpgrade(MetaUpgrade upg)
    {
        metaSkill = upg;
        UpdateUI();

    }

    public void UpdateUI()
    {
        icon.sprite = metaSkill.blueprint.icon;
        level.color = normalTextColor;
        header.text = metaSkill.blueprint.name;
        header.transform.gameObject.SetActive(true);
        level.transform.gameObject.SetActive(true);
        level.text = metaSkill.level + "/" + metaSkill.blueprint.maxLevel;
        background.color = normalColor;
        CanvasGroup.alpha = 1;
        icon.color = normalIconColor;
        if (metaSkill.locked)
        {
            header.transform.gameObject.SetActive(false);icon.sprite = secretSprite;
            level.transform.gameObject.SetActive(false);
            CanvasGroup.alpha = secretAlpha;
            icon.color = secretIconColor;
            background.color = secretBgColor;
        }
        else if (metaSkill.IsMaxed())
        {
            background.color = learnedColor;
            level.color = maxTextColor;
        }
        else if (Player.Instance!=null&&Player.Instance.HasLearned(metaSkill))
        {
            background.color = learnedColor;
        }

    }
}