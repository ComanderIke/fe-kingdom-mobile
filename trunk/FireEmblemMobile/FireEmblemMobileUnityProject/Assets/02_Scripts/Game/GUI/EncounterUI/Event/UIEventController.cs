using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.AI;
using Game.GameActors.Items;
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
    private LGEventDialogSO randomEvent;
    private LGEventDialogSO currentNode;
    private LGDialogChoiceData current;
    
    public void Show(EventEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        randomEvent = node.randomEvent;
        currentNode = randomEvent;
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
        headline.SetText(randomEvent.HeadLine);
        layout.DeleteAllChildren();
        description.text = currentNode.Text;
        ShowTextOptions(currentNode.Choices);
    }

    void ShowTextOptions(List<LGDialogChoiceData> textOptions)
    {
        int index = 0;
        foreach (var textOption in textOptions)
        {
            GameObject prefab = textOptionPrefab;
            if (textOption.NextDialogue is LGFightEventDialogSO)
                prefab = fightOptionPrefab;
    
            var go = Instantiate(prefab, layout);
            go.GetComponent<TextOptionController>().SetIndex(index);

            var textOptionController = go.GetComponent<TextOptionController>();
            ConfigureTextOption(textOption, textOptionController);
            
            index++;
        }
    }

    void ConfigureTextOption(LGDialogChoiceData textOption, TextOptionController textOptionController)
    {
        // if (textOption.statcheck)
        // {
        //     int stat = party.ActiveUnit.Stats.BaseAttributes.GetFromIndex(textOption.StatIndex);
        //     string statText = stat + " " + Attributes.GetAsText(textOption.StatIndex);
        //     TextOptionState state = TextOptionState.Normal;
        //     if (stat < textOption.StatRequirement)
        //         state = TextOptionState.Impossible;
        //     else if (stat >= (textOption.StatRequirement + 10))
        //         state = TextOptionState.High;
        //     textOptionController.Setup(textOption, textOption.Text,statText,state, this);
        // }
        // else
        // {
            textOptionController.Setup(textOption, textOption.Text, this);
        // }
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
        //var battleOutcome = GetBattleOutcome(result);
        if(result == AttackResult.Win)
            currentNode =(LGEventDialogSO)currentNode.Choices[0].NextDialogue;
        else
        {
            currentNode =(LGEventDialogSO)currentNode.Choices[1].NextDialogue;
        }
        UpdateUI();
    }

    // EventOutcome GetBattleOutcome(AttackResult result)
    // {
    //     var battleOutcome = current.outcomes[0];
    //     switch (result)
    //     {
    //         case AttackResult.Draw: 
    //             if(current.outcomes.Count>=3)
    //                 battleOutcome = current.outcomes[2];
    //             break;
    //         case AttackResult.Win: battleOutcome = current.outcomes[0];
    //             break;
    //         case AttackResult.Loss: if(current.outcomes.Count>=2)
    //                 battleOutcome = current.outcomes[1];
    //             break;
    //     }
    //     return battleOutcome;
    // }

   
    public void OptionClicked(TextOptionController textOptionController)
    {
        current = textOptionController.Option;
        currentNode = (LGEventDialogSO)current.NextDialogue;
        
        if (currentNode!=null)
        {
            CheckPossibleRewards();
            if (currentNode is LGFightEventDialogSO)
            {
                StartFight();
            }
            else
            {
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
        var enemy = ((LGFightEventDialogSO)current.NextDialogue).Enemy.Create();
            
        battleSystem.StartBattle(party.ActiveUnit, enemy, false, true);
        BattleSystem.OnBattleFinishedBeforeAfterBattleStuff += BattleEnded;
    }
    void CheckPossibleRewards()
    {
        if(currentNode.RewardResources!=null)
            foreach (var resource in currentNode.RewardResources)
            {
                switch (resource.ResourceType)
                {
                    case ResourceType.Gold:  Player.Instance.Party.AddGold(resource.Amount); break;
                    case ResourceType.Exp:  Player.Instance.Party.ActiveUnit.ExperienceManager.AddExp(resource.Amount); break;
                    case ResourceType.Grace:  Player.Instance.Party.AddGrace(resource.Amount); break;
                }
               
            }
        if(currentNode.RewardItems!=null)
            foreach (var item in currentNode.RewardItems)
            {
                 Player.Instance.Party.Convoy.AddItem(item.Create()); break;
            }
        
    }
   
   
   
}