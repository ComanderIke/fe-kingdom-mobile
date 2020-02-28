using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Characters;
using Assets.Scripts.AI.AttackReactions;
using Assets.Scripts.GameStates;

public class BattleState : GameState<NextStateTrigger>
{

    private Unit attacker;
    private Unit defender;
    private UISystem uiController;
    private UnitsSystem unitController;

    private string startMusic;
    public BattleSystem fightSystem;

    public BattleState(Unit attacker, Unit defender)
    {
        this.attacker = attacker;
        this.defender = defender;

        Debug.Log("FightState " + attacker.Name + " " + defender.Name);
        uiController = MainScript.instance.GetSystem<UISystem>();
        unitController = MainScript.instance.GetSystem<UnitsSystem>();
    }

    public override void Enter()
    {
        fightSystem = new BattleSystem(attacker, defender);
        ShowFightUI();
        unitController.HideUnits();
        BattleSystem.onStartAttack += fightSystem.DoAttack;
        UISystem.onCounterClicked = fightSystem.Counter;
        UISystem.onDodgeClicked = fightSystem.Dodge;
        UISystem.onGuardClicked = fightSystem.Guard;
        SetUpMusic();
    }
    public override GameState<NextStateTrigger> Update()
    {
        return nextState;
    }

    public override void Exit()
    {
        uiController.ShowMapUI();
        uiController.HideFightUI();
        uiController.HideReactUI();
        unitController.ShowUnits();

        BattleSystem.onStartAttack -= fightSystem.DoAttack;
        MainScript.instance.GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
    }



    private void ShowFightUI()
    {
        uiController.HideMapUI();
        if (!fightSystem.isDefense)
        {
            uiController.ShowFightUI(attacker, defender);
        }
        else
        {
            uiController.ShowReactUI(attacker, defender);
        }
    }
    private void SetUpMusic()
    {
        startMusic = MainScript.instance.GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
        MainScript.instance.GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
    }

}

