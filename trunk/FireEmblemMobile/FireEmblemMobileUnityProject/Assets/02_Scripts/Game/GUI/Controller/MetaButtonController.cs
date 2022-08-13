using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
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
    public float XPosMult;
    public float YPosMult;
    public float XOffset;
    public float YOffset;
    
    private void OnEnable()
    {
        if (metaSkill == null)
            return;
        icon.sprite = metaSkill.icon;
        level.color = normalTextColor;
        header.text = metaSkill.name;
        header.transform.gameObject.SetActive(true);
        level.transform.gameObject.SetActive(true);
        level.text = metaSkill.level + "/" + metaSkill.maxLevel;
        background.color = normalColor;
        CanvasGroup.alpha = 1;
        icon.color = normalIconColor;
        switch (metaSkill.state)
        {
            case UpgradeState.Learned:  background.color = learnedColor;break;
            case UpgradeState.NotLearned: break;
            case UpgradeState.Locked:
                header.transform.gameObject.SetActive(false);icon.sprite = secretSprite;
                level.transform.gameObject.SetActive(false);
                CanvasGroup.alpha = secretAlpha;
                icon.color = secretIconColor;
                background.color = secretBgColor;break;
            case UpgradeState.Maxed:
                level.color = maxTextColor; break;
        }

        GetComponent<RectTransform>().anchoredPosition = new Vector2(metaSkill.xPosInTree*XPosMult+XOffset, metaSkill.yPosInTree*YPosMult+YOffset);

    }

    public void OnClick()
    {
        SelectCursorController.Instance.Show(gameObject);
        MetaUpgradeDetailPanelController.Instance.Show(metaSkill);
    }
}