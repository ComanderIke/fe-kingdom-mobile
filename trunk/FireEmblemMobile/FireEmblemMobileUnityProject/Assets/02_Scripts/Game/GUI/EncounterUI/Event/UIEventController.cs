using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Game.AI;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

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

    float GetSuccessChanceOffAttRequirement(int goal, int current)
    {
        if (goal <= current)
            return 1.0f;
        else
        {
            int diff= goal - current;
            if (diff >= 10)
                return 0.0f;
            return 1.0f - diff/10f;
        }
    }
    void ShowTextOptions(List<LGDialogChoiceData> textOptions)
    {
        int index = 0;
        
        foreach (var textOption in textOptions)
        {
            string statText = "";
            TextOptionState textOptionType = TextOptionState.Normal;
            if (textOption.CharacterRequirement != null)
            {
                bool contains = false;
               
                    if (party.MembersContainsByBluePrintID(textOption.CharacterRequirement.bluePrintID))
                    {
                        contains = true;
                        textOptionType = TextOptionState.Secret;
                    }
                
                if(!contains)
                    continue;
                
            }
            if (textOption.ItemRequirements != null&&textOption.ItemRequirements.Count>0)
            {
               
                bool contains = false;
                foreach (var req in textOption.ItemRequirements)
                {
                    if (party.Convoy.ContainsItem(req.Create()))
                    {
                        contains = true;
                        textOptionType = TextOptionState.Secret;
                    }
                }
                if(!contains)
                    continue;
                
            }

            
            if (textOption.AttributeRequirements != null && textOption.AttributeRequirements.Count > 0)
            {
                float combinedChance = 1;
                Unit compareUnit = party.ActiveUnit;
                if(textOption.CharacterRequirement!=null&&party.MembersContainsByBluePrintID(textOption.CharacterRequirement.bluePrintID))
                 compareUnit = party.GetMembersContainsBluePrintID(textOption.CharacterRequirement.bluePrintID);
                foreach (var req in textOption.AttributeRequirements)
                {
                    float chance = GetSuccessChance(req, compareUnit);

                    combinedChance *= chance;
                   
                }

                if (combinedChance >= 0.8f)
                    textOptionType = TextOptionState.High;
                else if (combinedChance <= 0.2f)
                    textOptionType = TextOptionState.Low;
                else if (combinedChance <= 0.5f)
                    textOptionType = TextOptionState.Lowish;
                else
                {
                    textOptionType = TextOptionState.Normal;
                }

                statText = combinedChance * 100f + " %";

            }
           
            GameObject prefab = textOptionPrefab;
            if (textOption.NextDialogue is LGFightEventDialogSO)
                prefab = fightOptionPrefab;
    
            var go = Instantiate(prefab, layout);
            go.GetComponent<TextOptionController>().SetIndex(index);

            var textOptionController = go.GetComponent<TextOptionController>();
            
            textOptionController.Setup(textOption, textOption.Text,statText,textOptionType, this);
            
            
            index++;
        }
    }

    private float GetSuccessChance(ResponseStatRequirement req, Unit compareUnit)
    {
        float chance = 0;
        switch (req.AttributeType)
        {
            case AttributeType.LVL:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.ExperienceManager.Level);
                break;
            case AttributeType.STR:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().STR);
                break;
            case AttributeType.DEX:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().DEX);
                break;
            case AttributeType.DEF:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().DEF);
                break;
            case AttributeType.AGI:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().AGI);
                break;
            case AttributeType.CON:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().CON);
                break;
            case AttributeType.LCK:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().LCK);
                break;
            case AttributeType.INT:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().INT);
                break;
            case AttributeType.FTH:
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().FAITH);
                break;
        }

        return chance;
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

    bool HasRequirement(LGDialogChoiceData choiceData)
    {
        return choiceData.CharacterRequirement != null|| (choiceData.ItemRequirements!=null&&choiceData.ItemRequirements.Count>0)||(choiceData.AttributeRequirements!=null&&choiceData.AttributeRequirements.Count>0);
    }

    bool RequirementSuccess(LGDialogChoiceData choiceData)
    {
        if (!HasRequirement(choiceData))
            return true;
        foreach (var item in choiceData.ItemRequirements)
        {
            if (!party.Convoy.ContainsItem(item.Create()))
            {
                return false;
            }
        }

        Unit compareUnit = party.ActiveUnit;
        if(choiceData.CharacterRequirement!=null)
          compareUnit = party.GetMembersContainsBluePrintID(choiceData.CharacterRequirement.bluePrintID);
        float combinedChance = 1.0f;
        foreach (var statReq in choiceData.AttributeRequirements)
        {
            float chance=GetSuccessChance(statReq, compareUnit);
            combinedChance *= chance;
        }

        party.SetActiveUnit(compareUnit);
        return UnityEngine.Random.value <= combinedChance;

    }
   
    public void OptionClicked(TextOptionController textOptionController)
    {
        if (RequirementSuccess(textOptionController.Option))
        {
            Debug.Log("SUCCESS");
            current = textOptionController.Option;
            currentNode = (LGEventDialogSO)current.NextDialogue;
        }
        else
        {
            Debug.Log("FAILED");
            current = textOptionController.Option;
            currentNode = (LGEventDialogSO)current.NextDialogueFail;
        }

        if (currentNode!=null)
        {
            CheckPossibleRewards();
            CheckEvents();
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
    private void CheckEvents()
    {
        if (currentNode.Events != null&& currentNode.Events.Count>0)
        {
            foreach (var dialogEvent in currentNode.Events)
            {
                dialogEvent.Action();
            }
        }
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
                    case ResourceType.HP_Percent:
                        if (resource.Amount < 0)
                        {
                           // Debug.Log(resource.Amount / 100f+" "+resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp+" "+Math.Ceiling(resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp));
                           
                            Player.Instance.Party.ActiveUnit.InflictNonLethalTrueDamage(
                                (int)Math.Ceiling(-1*resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp));
                        }
                        else
                        {
                            Player.Instance.Party.ActiveUnit.Heal((int)Math.Ceiling((resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp)));
                        }

                        break;
                }
               
            }
        if(currentNode.RewardItems!=null)
            foreach (var item in currentNode.RewardItems)
            {
                 Player.Instance.Party.Convoy.AddItem(item.Create()); break;
            }
        
    }
   
   
   
}