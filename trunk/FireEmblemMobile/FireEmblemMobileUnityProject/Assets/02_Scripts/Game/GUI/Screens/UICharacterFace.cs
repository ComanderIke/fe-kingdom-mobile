using System.Collections;
using Game.GameActors.Units;
using Game.GUI;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterFace : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private UIStatBar hpBar;
  //  [SerializeField] private UIStatBar expBar;
    [SerializeField] private Image faceImage;
    [SerializeField] private MMF_Player feedbacks;
    [SerializeField] private ExpBarController expBar = default;
    private Unit unit;
    public void Show(Unit unit)
    {

        gameObject.SetActive(true);
        if (unit != null)
        {
            unit.HpValueChanged -= UpdateHpBar;
            unit.ExperienceManager.ExpGained -= UpdateExpBar;
        }

        this.unit = unit;
        unit.HpValueChanged += UpdateHpBar;
        
        unit.ExperienceManager.ExpGained += UpdateExpBar;
        InitHpBar();
        faceImage.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        nameText.SetText(unit.name);
    }

    private void OnDisable()
    {
       
        if(unit!=null)
            unit.HpValueChanged -= UpdateHpBar;
    }
    void InitHpBar()
    {
        // if (hpBar.currentValue > unit.Hp)
        // {
        //     feedbacks.PlayFeedbacks();
        // }
        //
        hpBar.SetValue(unit.Hp, unit.MaxHp, false);
        if(expBar!=null)
            expBar.UpdateInstant(unit.ExperienceManager.Exp);
    }

    void UpdateHpBar()
    {
        if (hpBar.currentValue > unit.Hp)
        {
            feedbacks.PlayFeedbacks();
        }
        
        hpBar.SetValue(unit.Hp, unit.MaxHp, true);
    }
    void UpdateExpBar(int expBefore, int expGained)
    {
        if (expBar == null)
            return;
        // if (expBar.currentValue > unit.ExperienceManager.Exp)
        // {
        //     feedbacks.PlayFeedbacks();
        // }
        
        expBar.UpdateInstant(expBefore);
        expBar.UpdateWithAnimatedTextOnly(expGained);
    }


    public void Hide()
    {
       gameObject.SetActive(false);
    }
}