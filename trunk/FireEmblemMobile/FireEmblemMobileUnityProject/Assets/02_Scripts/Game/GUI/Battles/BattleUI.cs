using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
  
    // [SerializeField] private TextMeshProUGUI hpText = default;
    // [SerializeField] private TextMeshProUGUI hpTextRight = default;
    // public BattleUIHPBar leftHPBar;
    // public BattleUIHPBar rightHPBar;
    
    [SerializeField] private UIAttackPreviewContainer left = default;
    [SerializeField] private UIAttackPreviewContainer right = default;
    [SerializeField] private Button SurrenderButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private UIAttackOrder attackOrderUI;

    public static event Action onSurrender;
    public static event Action onSkip;
    // Start is called before the first frame update
    void OnEnable()
    {
        BattleAnimationRenderer.OnShow += Show;
    }

    private void OnDisable()
    {
        CharacterCombatAnimations.OnDamageDealt -= UpdateHpBars;
        BattleAnimationRenderer.OnShow -= Show;
    }

    private bool playerUnitIsAttacker;

    private void Show(BattleSimulation battleSimulation, IBattleActor attacker, IAttackableTarget defender)
    {
        Debug.Log("TEST2"+attacker+" "+defender);
        Show(battleSimulation, (Unit)attacker, (Unit)defender);
    }

    private int currentHpLeft;
    private int currentHpRight;

    public void SkipClicked()
    {
        onSkip?.Invoke();
        skipButton.gameObject.SetActive(false);
        SurrenderButton.gameObject.SetActive(false);
    }
    public void SurrenderClicked()
    {
        onSurrender?.Invoke();
        SurrenderButton.gameObject.SetActive(false);
       
        Debug.Log("SurrenderClicked");
    }
    public void Show(BattleSimulation battleSimulation, Unit attacker, Unit defender)
    {
        var leftCharacter = defender;
        var rightCharacter = attacker;
        if(attacker.Faction!=null)
            playerUnitIsAttacker = attacker.Faction.IsPlayerControlled;
        else
        {
            playerUnitIsAttacker = attacker.Party != null;
        }
        if (playerUnitIsAttacker)
        {
            leftCharacter = attacker;
            rightCharacter = defender;
        }
        skipButton.gameObject.SetActive(true);
        SurrenderButton.gameObject.SetActive(battleSimulation.combatRounds.Count > 1);

        var combatRound = battleSimulation.combatRounds[0];
        currentHpLeft = combatRound.AttackerStats.CurrentHp;
        currentHpRight = combatRound.DefenderStats.CurrentHp;
        left.ShowInBattleContext(leftCharacter.visuals.CharacterSpriteSet.FaceSprite, combatRound.AttackerStats.Damage,combatRound.AttackerStats.Hit,combatRound.AttackerStats.Crit, combatRound.AttackerStats.CurrentHp,combatRound.AttackerStats.MaxHp, combatRound.AttackerHP);
        right.ShowInBattleContext(rightCharacter.visuals.CharacterSpriteSet.FaceSprite, combatRound.DefenderStats.Damage,combatRound.DefenderStats.Hit,combatRound.DefenderStats.Crit, combatRound.DefenderStats.CurrentHp,combatRound.DefenderStats.MaxHp, combatRound.DefenderHP,combatRound.DefenderStats.AttackCount != 0);
        attackOrderUI.Show(combatRound.AttacksData);

        CharacterCombatAnimations.OnDamageDealt -= UpdateHpBars;
        CharacterCombatAnimations.OnDamageDealt += UpdateHpBars;
        
        // leftHPBar.SetValues(playerUnit.MaxHp,playerUnit.Hp);
        // rightHPBar.SetValues(enemyUnit.MaxHp,enemyUnit.Hp);
   
    }

   
   
    public void UpdateHpBars(AttackData attackData)
    {
      
        if (playerUnitIsAttacker)
        {
            if (attackData.attacker)
            {
               
                currentHpRight -= attackData.Dmg;
                right.UpdateHP(currentHpRight);
     
            }
            else
            {
                currentHpLeft -= attackData.Dmg;
                left.UpdateHP(currentHpLeft);
            }
        
        }
        else
        {
            if (attackData.attacker)
            {
                currentHpLeft-= attackData.Dmg;
                left.UpdateHP(currentHpLeft);
               
            }
            else
            {
                currentHpRight -= attackData.Dmg;
                right.UpdateHP(currentHpRight);
            }
        }
    }
}
