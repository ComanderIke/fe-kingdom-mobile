using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
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
        BattleAnimationRenderer.OnShow += Show;
    }

    private bool playerUnitIsAttacker;

    private void Show(BattleSimulation battleSimulation, IBattleActor attacker, IAttackableTarget defender)
    {
        Show(battleSimulation, (Unit)attacker, (Unit)defender);
    }
    public void Show(BattleSimulation battleSimulation, Unit attacker, Unit defender)
    {
        playerUnitIsAttacker = attacker.Faction==null||attacker.Faction.Id == 0;
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
        var combatRound = battleSimulation.combatRounds[0];
        
        
        dmgValue.text = playerUnitIsAttacker? ""+ combatRound.AttackerDamage:""+combatRound.DefenderDamage;
        hitValue.text = playerUnitIsAttacker?"" + combatRound.AttackerHit:""+combatRound.DefenderHit;
        critValue.text = playerUnitIsAttacker?"" + combatRound.AttackerCrit:""+combatRound.DefenderCrit;
        dmgValueRight.text = !playerUnitIsAttacker? ""+ combatRound.AttackerDamage:""+combatRound.DefenderDamage;
        hitValueRight.text = !playerUnitIsAttacker?"" + combatRound.AttackerHit:""+combatRound.DefenderHit;
        critValueRight.text = !playerUnitIsAttacker?"" + combatRound.AttackerCrit:""+combatRound.DefenderCrit;


        if(playerUnitIsAttacker){
                attackCountX.SetActive(combatRound.AttackerAttackCount > 1);
                attackCount.gameObject.SetActive(attackCountX.activeSelf);
                attackCount.text = "" + combatRound.AttackerAttackCount;
                attackCountRightX.SetActive(combatRound.DefenderAttackCount > 1);
                attackCountRight.gameObject.SetActive(attackCountRightX.activeSelf);
                attackCountRight.text = "" + combatRound.DefenderAttackCount;
                if (combatRound.DefenderAttackCount > 0)
                {
                    dmgValueRight.text = "" + combatRound.DefenderDamage;
                    hitValueRight.text = "" + combatRound.DefenderHit;
                    critValueRight.text = "" + combatRound.DefenderCrit;
                }
                else
                {
                    dmgValueRight.text = "-";
                    hitValueRight.text = "-" ;
                    critValueRight.text = "-" ;
                }
        }
        else{
            attackCountX.SetActive(combatRound.DefenderAttackCount > 1);
            attackCount.gameObject.SetActive(attackCountX.activeSelf);
            attackCount.text = "" + combatRound.DefenderAttackCount;
            attackCountRightX.SetActive(combatRound.AttackerAttackCount > 1);
            attackCountRight.gameObject.SetActive(attackCountRightX.activeSelf);
            attackCountRight.text = "" + combatRound.AttackerAttackCount;
            dmgValueRight.text = ""+ combatRound.AttackerDamage;
            hitValueRight.text = "" + combatRound.AttackerHit;
            critValueRight.text = "" + combatRound.AttackerCrit;
            if (combatRound.DefenderAttackCount > 0)
            {
                dmgValue.text = "" + combatRound.DefenderDamage;
                hitValue.text = "" + combatRound.DefenderHit;
                critValue.text = "" + combatRound.DefenderCrit;
            }
            else
            {
                dmgValue.text = "-";
                hitValue.text = "-" ;
                critValue.text = "-" ;
            }
        }

        CharacterCombatAnimations.OnDamageDealt -= UpdateDefenderHPBar;
        CharacterCombatAnimations.OnDamageDealt += UpdateDefenderHPBar;
        
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
