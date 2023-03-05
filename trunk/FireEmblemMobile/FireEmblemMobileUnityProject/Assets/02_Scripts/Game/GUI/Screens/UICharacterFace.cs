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
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private UIStatBar hpBar;
    [SerializeField] private Image faceImage;
    [SerializeField] private MMF_Player feedbacks;
    private Unit unit;
    public void Show(Unit unit)
    {
        if(unit!=null)
            unit.HpValueChanged -= UpdateHpBar;
        this.unit = unit;
        unit.HpValueChanged += UpdateHpBar;
        InitHpBar();
        faceImage.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        name.SetText(unit.name);
    }

    private void OnDisable()
    {
       
        if(unit!=null)
            unit.HpValueChanged -= UpdateHpBar;
    }
    void InitHpBar()
    {
        if (hpBar.currentValue > unit.Hp)
        {
            feedbacks.PlayFeedbacks();
        }
        Debug.Log("INIT HP");
        hpBar.SetValue(unit.Hp, unit.MaxHp, false);
    }

    void UpdateHpBar()
    {
        if (hpBar.currentValue > unit.Hp)
        {
            feedbacks.PlayFeedbacks();
        }
        Debug.Log("UPDATE HP");
        hpBar.SetValue(unit.Hp, unit.MaxHp, true);
    }


    
  

 
}