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
using Game.GameResources;
using Game.Mechanics;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using LostGrace;
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
        Debug.Log("Showing event ui screen");
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        randomEvent = node.randomEvent;
        if (!String.IsNullOrEmpty(Player.Instance.CurrentEventDialogID))
        {
            Debug.Log("Event visited previously");
            currentNode = GameBPData.Instance.GetEventData().GetEventById(Player.Instance.CurrentEventDialogID);
            if (currentNode is LGBattleEventDialogSO)
            {
                MapBattleEnded(Player.Instance.LastBattleOutcome == BattleOutcome.Victory);
            }

            
        }
        else
        {
            Debug.Log("First Time this Event");
            currentNode = randomEvent;
        }
        
        party.onActiveUnitChanged -= ActiveUnitChanged;
        party.onActiveUnitChanged += ActiveUnitChanged;
        UpdateUI();
    
    }

    private void OnDestroy()
    {
        party.onActiveUnitChanged -= ActiveUnitChanged;
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

    public void UpdateUIValues()
    {
        headline.SetText(randomEvent.HeadLine);
        layout.DeleteAllChildren();
        Debug.Log("CurrentNode: "+currentNode);
        description.text = currentNode.Text;
        ShowTextOptions(currentNode.Choices);
    }
    public void UpdateUI()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        UpdateUIValues();
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
            //make all requirements GO and check and if some of them are not met make them RED
            if (!SemiRequirementsMet(textOption))
            {
                
                textOptionType = TextOptionState.Locked;
               
            }
            if (HasSecredRequirements(textOption))
            {
                Debug.Log("SECRET REQUIREMENTS" +textOption.Text);
                if (SecretRequirementsMet(textOption))
                {
                    Debug.Log("MET REQUIREMENTS");
                    textOptionType = TextOptionState.Secret;
                    if (textOption.ResourceRequirements != null && textOption.ResourceRequirements.Count > 0)
                    {
                        foreach (var req in textOption.ResourceRequirements)
                        {
                            if (req.ResourceType == ResourceType.Morality)
                            {
                                if (req.Amount > 0)
                                    textOptionType = TextOptionState.Good;
                                else
                                {
                                    textOptionType = TextOptionState.Evil;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("NOTMET REQUIREMENTS");
                    textOptionType = TextOptionState.SecretHidden;
                }
            }
            
         
            GameObject prefab = textOptionPrefab;
            if (textOption.NextDialogue is LGFightEventDialogSO)
                prefab = fightOptionPrefab;


            if (textOptionType != TextOptionState.SecretHidden)
            {
                var go = Instantiate(prefab, layout);


                var textOptionController = go.GetComponent<TextOptionController>();

                textOptionController.Setup(textOption, textOption.Text, statText, textOptionType, this);
                if (textOptionType != TextOptionState.Locked)
                    textOptionController.SetIndex(index);
                index++;
            }



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
                chance = GetSuccessChanceOffAttRequirement(req.Amount, compareUnit.Stats.CombinedAttributes().MaxHp);
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


    void MapBattleEnded(bool won)
    {
        Debug.Log("MapBattleEnded: "+won);
        if(won)
            currentNode =(LGEventDialogSO)currentNode.Choices[0].NextDialogue;
        else
        {
            currentNode =(LGEventDialogSO)currentNode.Choices[1].NextDialogue;
        }

        CheckPossibleRewards();
        UpdateUI();
    }
    void BattleEnded(AttackResult result)
    {
        Debug.Log("BATTLE ENDED");
        BattleSystem.OnBattleFinishedBeforeAfterBattleStuff -= BattleEnded;
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
        return (choiceData.ItemRequirements!=null&&choiceData.ItemRequirements.Count>0)|| (choiceData.ResourceRequirements!=null&&choiceData.ResourceRequirements.Count>0)||(choiceData.AttributeRequirements!=null&&choiceData.AttributeRequirements.Count>0);
    }

    bool HasSecredRequirements(LGDialogChoiceData choiceData)
    {
        var resReq = false;
        if ((choiceData.ResourceRequirements != null && choiceData.ResourceRequirements.Count > 0))
        {
            foreach (var req in choiceData.ResourceRequirements)
            {
                if (req.ResourceType == ResourceType.Morality)
                    return true;
            }
        }

        return resReq||(choiceData.CharacterRequirements!=null&&choiceData.CharacterRequirements.Count>0)|| (choiceData.BlessingRequirements!=null&&choiceData.BlessingRequirements.Count>0);

    }
    bool SecretRequirementsMet(LGDialogChoiceData choiceData)
    {
        if (choiceData.CharacterRequirements != null&& choiceData.CharacterRequirements.Count>0)
        {
            foreach (var charReq in choiceData.CharacterRequirements)
            {
                if (party.ActiveUnit.bluePrintID==charReq.bluePrintID)
                    return true;
            }

            return false;

        }
        if (choiceData.BlessingRequirements != null&& choiceData.BlessingRequirements.Count>0)
        {
            foreach (var blessing in choiceData.BlessingRequirements)
            {
                if (party.ActiveUnit.Blessing!=null&&party.ActiveUnit.Blessing.Name==blessing.Name)
                    return true;
            }

            return false;

        }

        foreach (var res in choiceData.ResourceRequirements)
        {
            switch (res.ResourceType)
            {
                case ResourceType.Morality:
                    if (res.Amount > 0)
                    {
                        if (party.Morality.GetCurrentMoralityValue() < res.Amount)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (party.Morality.GetCurrentMoralityValue() > res.Amount)
                        {
                            return false;
                        }
                    }

                    break;
            }
        }

        return true;
    }
    bool SemiRequirementsMet(LGDialogChoiceData choiceData)
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
        foreach (var res in choiceData.ResourceRequirements)
        {
            switch (res.ResourceType)
            {
                case ResourceType.Gold: 
                    if (!party.CanAfford(res.Amount))
                    {
                        return false;
                    } break;
                case ResourceType.Grace:
                    if (party.CollectedGrace<res.Amount)
                    {
                        return false;
                    } break;
                case ResourceType.HP_Percent: 
                    if (party.ActiveUnit.Hp<party.ActiveUnit.MaxHp*res.Amount)
                    {
                        return false;
                    } 
                    break;
                
            }
            
        }
        
      
        return true;

    }

    bool CheckAttributeRequirementsSuccess(LGDialogChoiceData choiceData)
    {
        float combinedChance = 1.0f;
        foreach (var statReq in choiceData.AttributeRequirements)
        {
            Debug.Log("Stat Req: "+statReq.AttributeType+" "+statReq.Amount);
            float chance=GetSuccessChance(statReq, party.ActiveUnit);
            combinedChance *= chance;
        }

        float randomValue = UnityEngine.Random.value;
        Debug.Log("Roll: "+randomValue+" Goal<= "+combinedChance);
        
        return randomValue <= combinedChance;
    }

    void PayRequirements()
    {
        // if (current.CharacterRequirement != null)
        // {
        //     Player.Instance.Party.SetActiveUnit( Player.Instance.Party.GetUnitByName(current.CharacterRequirement.Name));
        // }
        foreach (var req in current.ResourceRequirements)
        {
            switch (req.ResourceType)
            {
                case ResourceType.Gold: Player.Instance.Party.AddGold(-req.Amount);break;
                case ResourceType.Grace: Player.Instance.Party.AddGrace(- req.Amount);break;
                case ResourceType.Morality:Player.Instance.Party.Morality.AddMorality(- req.Amount); break;
                case ResourceType.HP_Percent:
                    Player.Instance.Party.ActiveUnit.Hp -= Player.Instance.Party.ActiveUnit.MaxHp * req.Amount; break;
            }
        }
    }
    public void OptionClicked(TextOptionController textOptionController)
    {
        if (CheckAttributeRequirementsSuccess(textOptionController.Option))
        {
            
            current = textOptionController.Option;
            currentNode = (LGEventDialogSO)current.NextDialogue;
        }
        else
        {
    
            current = textOptionController.Option;
            currentNode = (LGEventDialogSO)current.NextDialogueFail;
        }

        if (currentNode!=null)
        {
            PayRequirements();
            CheckPossibleRewards();
            CheckEvents();
            if (currentNode is LGFightEventDialogSO)
            {
                StartFight();
            }
            else if (currentNode is LGBattleEventDialogSO)
            {
                StartBattle();
            }
            else
            {
                UpdateUIValues();
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

    void StartBattle()
    {
        Debug.Log("TODO Start Battle from EVENT");
        var battleDialogSO = ((LGBattleEventDialogSO)current.NextDialogue);
        var enemyData = battleDialogSO.EnemyArmy;
        Player.Instance.CurrentEventDialogID=(battleDialogSO.name);
        GameSceneController.Instance.LoadBattleLevel(Scenes.Battle1, enemyData);
        //Get Enemy Army Data/Layout and Map Prefab from Node
        //better: use BattleEncounterNodeData create one specifically for this event and add save and load to node Editor (same way as unitbp for enemytoFight)
        // Then start the battle scene and put data into saveGame so that on return UIEventNode is shown again at the right node...
    }
    void StartFight()
    {
        var battleSystem = AreaGameManager.Instance.GetSystem<BattleSystem>();
        var enemy = ((LGFightEventDialogSO)current.NextDialogue).Enemy.Create();
            Debug.Log("Start Event Battle: "+party.ActiveUnit+" "+enemy);
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
                dialogEvent.OnComplete += () =>
                {
                    HandleDialogEventRewards(dialogEvent);
                };
            }
        }
    }

    void HandleDialogEventRewards(DialogEvent dialogEvent)
    {
        var reward = dialogEvent.GetReward();
        if (reward != null)
        {
            if(reward.gold!=0)
                Player.Instance.Party.AddGold(reward.gold); 
            if(reward.experience!=0)
                Player.Instance.Party.ActiveUnit.ExperienceManager.AddExp(reward.experience);
            if(reward.grace!=0)
                Player.Instance.Party.AddGrace(reward.grace); 
            
            if(reward.itemBp!=null)
                foreach (var item in reward.itemBp)
                {
                    Player.Instance.Party.AddItem(item.Create()); 
                }
        }
    }
    void CheckPossibleRewards()
    {
        Debug.Log("CHECK REWARDS");
        if (currentNode.RewardResources != null)
        {
            Debug.Log(currentNode.RewardResources.Count);
            foreach (var resource in currentNode.RewardResources)
            {
                Debug.Log(resource.ResourceType);
                switch (resource.ResourceType)
                {
                    case ResourceType.Gold:
                        Debug.Log("ADD GOLD " + resource.Amount);
                        Player.Instance.Party.AddGold(resource.Amount);
                        break;
                    case ResourceType.Exp:
                        Player.Instance.Party.ActiveUnit.ExperienceManager.AddExp(resource.Amount);
                        break;
                    case ResourceType.Grace:
                        Player.Instance.Party.AddGrace(resource.Amount);
                        break;
                    case ResourceType.Morality:
                        Player.Instance.Party.Morality.AddMorality(resource.Amount);
                        break;
                    case ResourceType.HP_Percent:
                        if (resource.Amount < 0)
                        {
                            // Debug.Log(resource.Amount / 100f+" "+resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp+" "+Math.Ceiling(resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp));

                            Player.Instance.Party.ActiveUnit.InflictNonLethalTrueDamage(null,
                                (int)Math.Ceiling(-1 * resource.Amount / 100f *
                                                  Player.Instance.Party.ActiveUnit.MaxHp));
                        }
                        else
                        {
                            Player.Instance.Party.ActiveUnit.Heal(
                                (int)Math.Ceiling((resource.Amount / 100f * Player.Instance.Party.ActiveUnit.MaxHp)));
                        }

                        break;
                }

            }
        }

        if(currentNode.RewardItems!=null)
            foreach (var item in currentNode.RewardItems)
            {
                if(item !=null)
                    Player.Instance.Party.AddItem(item.Create());
            }
        
    }
   
   
   
}