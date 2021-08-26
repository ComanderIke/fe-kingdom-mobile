using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Image faceSpriteLeft;

    public Image faceSpriteRight;
    [SerializeField] private TextMeshProUGUI attackCount = default;
    [SerializeField] private GameObject attackCountX = default;
    [SerializeField] private TextMeshProUGUI dmgValue = default;
    [SerializeField] private TextMeshProUGUI hitValue = default;
    [SerializeField] private TextMeshProUGUI dmgValueRight = default;
    [SerializeField] private TextMeshProUGUI hitValueRight = default;
    [SerializeField] private TextMeshProUGUI attackCountRight = default;
    [SerializeField] private TextMeshProUGUI hpText = default;
    [SerializeField] private TextMeshProUGUI hpTextRight = default;
    [SerializeField] private GameObject attackCountRightX = default;

    public BattleUIHPBar leftHPBar;
    public BattleUIHPBar rightHPBar;

    private int maxHPLeft;

    private int currentHPLeft;

    private int maxHPRight;

    private int currentHPRight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Show(BattleSimulation battleSimulation, Unit attacker, Unit defender)
    {
        currentHPLeft = attacker.Hp;
        currentHPRight = defender.Hp;
        maxHPLeft = attacker.Stats.MaxHp;
        maxHPRight = defender.Stats.MaxHp;
        hpText.SetText(""+currentHPLeft);
        hpTextRight.SetText(""+currentHPRight);
        faceSpriteLeft.sprite = attacker.visuals.CharacterSpriteSet.FaceSprite;
        faceSpriteRight.sprite = defender.visuals.CharacterSpriteSet.FaceSprite;
        dmgValue.text = "" + battleSimulation.AttackerDamage;
        hitValue.text = "" + battleSimulation.AttackerHit;
        dmgValueRight.text = "" + battleSimulation.DefenderDamage;
        hitValueRight.text = "" + battleSimulation.DefenderHit;
        attackCountX.SetActive(battleSimulation.AttackerAttackCount>1);
        attackCount.gameObject.SetActive(attackCountX.activeSelf);
        attackCount.text = "" + battleSimulation.AttackerAttackCount;
        attackCountRightX.SetActive(battleSimulation.DefenderAttackCount > 1);
        attackCountRight.gameObject.SetActive(attackCountRightX.activeSelf);
        attackCountRight.text = "" + battleSimulation.DefenderAttackCount;
        leftHPBar.SetValues(battleSimulation.Attacker.Stats.MaxHp,battleSimulation.Attacker.Hp);
        rightHPBar.SetValues(battleSimulation.Defender.Stats.MaxHp,battleSimulation.Defender.Hp);
    }

   
    public void UpdateAttackerHPBar(AttackData attackData)
    {
        currentHPLeft -= attackData.Dmg;
        hpText.SetText(""+currentHPLeft);
        leftHPBar.SetValues(maxHPLeft,currentHPLeft);
    }
    public void UpdateDefenderHPBar(AttackData attackData)
    {
        currentHPRight -= attackData.Dmg;
      
        hpTextRight.SetText(""+currentHPRight);
        rightHPBar.SetValues(maxHPRight,currentHPRight);
    }
}
