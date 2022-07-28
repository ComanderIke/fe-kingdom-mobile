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
    [SerializeField] private TextMeshProUGUI critValue = default;
    [SerializeField] private TextMeshProUGUI dmgValueRight = default;
    [SerializeField] private TextMeshProUGUI hitValueRight = default;
    [SerializeField] private TextMeshProUGUI critValueRight = default;
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

    private bool playerUnitIsAttacker;
    public void Show(BattleSimulation battleSimulation, Unit attacker, Unit defender)
    {
        playerUnitIsAttacker = attacker.Faction.Id == 0;
        var playerUnit = playerUnitIsAttacker? attacker : defender;
        var enemyUnit= playerUnitIsAttacker? defender : attacker;
        currentHPLeft = playerUnit.Hp;
        currentHPRight = enemyUnit.Hp;
        maxHPLeft = playerUnit.MaxHp;
        maxHPRight = enemyUnit.MaxHp;
      //  Debug.Log(currentHPLeft+" "+currentHPRight+" "+battleSimulation.Attacker.Stats.MaxHp+" "+battleSimulation.Attacker.Hp);
        hpText.SetText(""+currentHPLeft);
        hpTextRight.SetText(""+currentHPRight);
        faceSpriteLeft.sprite = playerUnit.visuals.CharacterSpriteSet.FaceSprite;
        faceSpriteRight.sprite = enemyUnit.visuals.CharacterSpriteSet.FaceSprite;
        
        
        dmgValue.text = playerUnitIsAttacker? ""+ battleSimulation.AttackerDamage:""+battleSimulation.DefenderDamage;
        hitValue.text = playerUnitIsAttacker?"" + battleSimulation.AttackerHit:""+battleSimulation.DefenderHit;
        critValue.text = playerUnitIsAttacker?"" + battleSimulation.AttackerCrit:""+battleSimulation.DefenderCrit;
        dmgValueRight.text = !playerUnitIsAttacker? ""+ battleSimulation.AttackerDamage:""+battleSimulation.DefenderDamage;
        hitValueRight.text = !playerUnitIsAttacker?"" + battleSimulation.AttackerHit:""+battleSimulation.DefenderHit;
        critValueRight.text = !playerUnitIsAttacker?"" + battleSimulation.AttackerCrit:""+battleSimulation.DefenderCrit;


        if(playerUnitIsAttacker){
                attackCountX.SetActive(battleSimulation.AttackerAttackCount > 1);
                attackCount.gameObject.SetActive(attackCountX.activeSelf);
                attackCount.text = "" + battleSimulation.AttackerAttackCount;
                attackCountRightX.SetActive(battleSimulation.DefenderAttackCount > 1);
                attackCountRight.gameObject.SetActive(attackCountRightX.activeSelf);
                attackCountRight.text = "" + battleSimulation.DefenderAttackCount;
                if (battleSimulation.DefenderAttackCount > 0)
                {
                    dmgValueRight.text = "" + battleSimulation.DefenderDamage;
                    hitValueRight.text = "" + battleSimulation.DefenderHit;
                    critValueRight.text = "" + battleSimulation.DefenderCrit;
                }
                else
                {
                    dmgValueRight.text = "-";
                    hitValueRight.text = "-" ;
                    critValueRight.text = "-" ;
                }
        }
        else{
            attackCountX.SetActive(battleSimulation.DefenderAttackCount > 1);
            attackCount.gameObject.SetActive(attackCountX.activeSelf);
            attackCount.text = "" + battleSimulation.DefenderAttackCount;
            attackCountRightX.SetActive(battleSimulation.AttackerAttackCount > 1);
            attackCountRight.gameObject.SetActive(attackCountRightX.activeSelf);
            attackCountRight.text = "" + battleSimulation.AttackerAttackCount;
            dmgValueRight.text = ""+ battleSimulation.AttackerDamage;
            hitValueRight.text = "" + battleSimulation.AttackerHit;
            critValueRight.text = "" + battleSimulation.AttackerCrit;
            if (battleSimulation.DefenderAttackCount > 0)
            {
                dmgValue.text = "" + battleSimulation.DefenderDamage;
                hitValue.text = "" + battleSimulation.DefenderHit;
                critValue.text = "" + battleSimulation.DefenderCrit;
            }
            else
            {
                dmgValue.text = "-";
                hitValue.text = "-" ;
                critValue.text = "-" ;
            }
        }


           
        
        leftHPBar.SetValues(playerUnit.MaxHp,playerUnit.Hp);
        rightHPBar.SetValues(enemyUnit.MaxHp,enemyUnit.Hp);
   
    }

   
    public void UpdateAttackerHPBar(AttackData attackData)
    {
        if (playerUnitIsAttacker)
        {
            currentHPLeft -= attackData.Dmg;
            hpText.SetText("" + currentHPLeft);
            leftHPBar.SetValues(maxHPLeft, currentHPLeft);
        }
        else
        {
            currentHPRight -= attackData.Dmg;
      
            hpTextRight.SetText(""+currentHPRight);
            rightHPBar.SetValues(maxHPRight,currentHPRight);
        }
    }
    public void UpdateDefenderHPBar(AttackData attackData)
    {
        if (playerUnitIsAttacker)
        {currentHPRight -= attackData.Dmg;
      
            hpTextRight.SetText(""+currentHPRight);
            rightHPBar.SetValues(maxHPRight,currentHPRight);
           
        }
        else
        {
            currentHPLeft -= attackData.Dmg;
            hpText.SetText("" + currentHPLeft);
            leftHPBar.SetValues(maxHPLeft, currentHPLeft);
        }
    }
}
