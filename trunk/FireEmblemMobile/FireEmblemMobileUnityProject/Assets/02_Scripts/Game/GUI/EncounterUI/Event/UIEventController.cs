using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIEventController : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] EventEncounterNode node;
    [HideInInspector]
    [SerializeField] Party party;
  
    [SerializeField] TextMeshProUGUI headline;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Transform layout;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    [SerializeField] GameObject textOptionPrefab;
    [SerializeField] GameObject fightOptionPrefab;
    private RandomEvent randomEvent;
    private EventScene currentScene;
    private ResponseOption current;
    
    public void Show(EventEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        randomEvent = node.randomEvent;
        currentScene = randomEvent.scenes[0];
        party.onActiveUnitChanged -= ActiveUnitChanged;
        party.onActiveUnitChanged += ActiveUnitChanged;
        UpdateUI();
    
    }
    public void NextClicked()
    {
        party.ActiveUnitIndex++;
        UpdateUI();
    }

    public void PrevClicked()
    {
        party.ActiveUnitIndex--;
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        headline.SetText(randomEvent.headline);
        layout.DeleteAllChildren();
        description.text = currentScene.MainText;
        ShowTextOptions(currentScene.textOptions);
    }

    void ShowTextOptions(List<ResponseOption> textOptions)
    {
        int index = 0;
        foreach (var textOption in textOptions)
        {
            GameObject prefab = textOptionPrefab;
            if (textOption.fightData!=null)
                prefab = fightOptionPrefab;
    
            var go = Instantiate(prefab, layout);
            go.GetComponent<TextOptionController>().SetIndex(index);

            var textOptionController = go.GetComponent<TextOptionController>();
            ConfigureTextOption(textOption, textOptionController);
            
            index++;
        }
    }

    void ConfigureTextOption(ResponseOption textOption, TextOptionController textOptionController)
    {
        if (textOption.statcheck)
        {
            int stat = party.ActiveUnit.Stats.BaseAttributes.GetFromIndex(textOption.StatIndex);
            string statText = stat + " " + Attributes.GetAsText(textOption.StatIndex);
            TextOptionState state = TextOptionState.Normal;
            if (stat < textOption.StatRequirement)
                state = TextOptionState.Impossible;
            else if (stat >= (textOption.StatRequirement + 10))
                state = TextOptionState.High;
            textOptionController.Setup(textOption, textOption.Text,statText,state, this);
        }
        else
        {
            textOptionController.Setup(textOption, textOption.Text, this);
        }
    }
    
    void ActiveUnitChanged()
    {
        UpdateUI();
    }
    void Hide()
    {
        canvas.enabled = false;
        party.onActiveUnitChanged -= ActiveUnitChanged;
    }
    
    
    
    void BattleEnded(AttackResult result)
    {
        Debug.Log("BATTLE ENDED");
        BattleSystem.OnBattleFinished -= BattleEnded;
        var battleOutcome = GetBattleOutcome(result);

       
        currentScene = randomEvent.scenes[battleOutcome.nextSceneIndex];
        UpdateUI();
    }

    EventOutcome GetBattleOutcome(AttackResult result)
    {
        var battleOutcome = current.outcomes[0];
        switch (result)
        {
            case AttackResult.Draw: 
                if(current.outcomes.Count>=3)
                    battleOutcome = current.outcomes[2];
                break;
            case AttackResult.Win: battleOutcome = current.outcomes[0];
                break;
            case AttackResult.Loss: if(current.outcomes.Count>=2)
                    battleOutcome = current.outcomes[1];
                break;
        }
        return battleOutcome;
    }

   
    public void OptionClicked(TextOptionController textOptionController)
    {
        current = textOptionController.Option;
        CheckPossibleRewards();
        if (current.outcomes[0].nextSceneIndex != -1)
        {
            if (current.type == EventSceneType.Fight)
            {
                StartFight();
            }
            else
            {
                currentScene = randomEvent.scenes[current.outcomes[0].nextSceneIndex];
                
                UpdateUI();
            }
        }
        else
        {
           EndEvent();
        }
    }

    void EndEvent()
    {
        Hide();
        node.Continue();
    }

    void StartFight()
    {
        var battleSystem = AreaGameManager.Instance.GetSystem<BattleSystem>();
        var enemy = current.fightData.EnemyToFight.Create();
            
        battleSystem.StartBattle(party.ActiveUnit, enemy, false, true);
        BattleSystem.OnBattleFinishedBeforeAfterBattleStuff += BattleEnded;
    }
    void CheckPossibleRewards()
    {
        if (current.reward.gold != 0||current.reward.experience!=0||current.reward.grace!=0)
        {
            Player.Instance.Party.AddGold(current.reward.gold);
            //Player.Instance.Party.AddSmithingStones(current.reward.smithingStones);
            Debug.Log("Node Position: "+node.gameObject.transform.position);
            Player.Instance.Party.ActiveUnit.ExperienceManager.AddExp(current.reward.experience);
        }
    }
   
   
   
}