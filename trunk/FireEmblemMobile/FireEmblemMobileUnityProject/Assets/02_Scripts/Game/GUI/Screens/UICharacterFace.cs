using Game.GameActors.Units;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterFace : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private UIStatBar hpBar;
    [SerializeField] private Image faceImage;

    private Unit unit;
    public void Show(Unit unit)
    {
        if(unit!=null)
            unit.HpValueChanged -= UpdateHpBar;
        this.unit = unit;
        unit.HpValueChanged += UpdateHpBar;
        UpdateHpBar();
        faceImage.sprite = unit.visuals.CharacterSpriteSet.FaceSprite;
        name.SetText(unit.name);
    }

    private void OnDisable()
    {
        if(unit!=null)
            unit.HpValueChanged -= UpdateHpBar;
    }

    void UpdateHpBar()
    {
        hpBar.SetValue(unit.Hp, unit.MaxHp);
    }
}