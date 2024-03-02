using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.External.Editor.Data.Save;
using __2___Scripts.Game.Utility;
using _02_Scripts.Game.Dialog.DialogSystem;
using Febucci.UI;
using Game.AI;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameResources;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.Mechanics;
using Game.Mechanics.Battle;
using Game.States;
using Game.Systems;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using Utility;
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
    [SerializeField] private float textOptionsDelay = .5f;
    private LGEventDialogSO randomEvent;
    private LGEventDialogSO currentNode;
    private LGDialogChoiceData current;
    public IAttackPreviewUI attackPreviewUI;
    
    
    public void Show(EventEncounterNode node, Party party)
    {
        // Debug.Log("Showing event ui screen");
        if (textOptionStates == null)
            textOptionStates = new List<TextOptionVisualData>();
        textOptionStates.Clear();
        textAnimationFinished = false;
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
            // Debug.Log("First Time this Event");
            currentNode = randomEvent;
        }
        
        party.onActiveUnitChanged -= ActiveUnitChanged;
        party.onActiveUnitChanged += ActiveUnitChanged;
        UpdateUI();
    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var typeWriter = description.GetComponent<TypewriterByCharacter>();
            typeWriter.SkipTypewriter();
        }
    }

    private void UpdateAttackPreviewUI(BattlePreview battlePreview)
    {
        if (battlePreview.Attacker is Unit attacker)
        {
            if (battlePreview.Defender is Unit defender)
                attackPreviewUI.Show(battlePreview, attacker, defender, "Attack");
            else if (battlePreview.TargetObject != null &&
                     battlePreview.TargetObject is Destroyable dest)
            {
         
                attackPreviewUI.Show(battlePreview, attacker,"Attack", dest.Sprite);
            }

        }
    }
    private void ShowAttackPreviewUI(BattlePreview battlePreview)
    {
        if (battlePreview.Attacker is Unit attacker)
        {
            if (battlePreview.Defender is Unit defender)
                attackPreviewUI.Show(battlePreview, attacker, defender, "Attack");
            else if (battlePreview.TargetObject != null &&
                     battlePreview.TargetObject is Destroyable dest)
            {
         
                attackPreviewUI.Show(battlePreview, attacker,"Attack", dest.Sprite);
            }

        }
    }
    private void OnDestroy()
    {
        party.onActiveUnitChanged -= ActiveUnitChanged;
    }

    public void NextClicked()
    {
        party.ActiveUnitIndex++;
      
    }

    public void TextAnimaterTextShowed()
    {
        MyDebug.LogTest("TEXT SHOWED");
        textAnimationFinished = true;
        if(currentNode!=null&&!(currentNode is LGFightEventDialogSO))
            ShowTextOptions(currentNode.Choices);
    }
   
    public void PrevClicked()
    {
        party.ActiveUnitIndex--;
     
    }

    public void UpdateUIValues()
    {
        headline.SetText(randomEvent.HeadLine);
        layout.DeleteAllChildren();
        // Debug.Log("CurrentNode: "+currentNode);
        if (String.Compare(description.text, "<noparse></noparse>"+currentNode.Text, StringComparison.Ordinal)!=0)
        {
            // Debug.Log("Update Text");
            textAnimationFinished = false;
            description.text = currentNode.Text;
        }

        
    }

    private bool textAnimationFinished = false;
    public void UpdateUICharacterRelated()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        if(textAnimationFinished&&!(currentNode is LGFightEventDialogSO))
            ShowTextOptions(currentNode.Choices);
    }
    public void UpdateUI()
    {
        UpdateUICharacterRelated();
       
        UpdateUIValues();
    }

    float GetSuccessChanceOffAttRequirement(int goal, int current)
    {
        if (goal <= current)
            return 1.0f;
        else
        {
            int diff= goal - current;
            if (diff >= 5)
                return 0.0f;
            return 1.0f - diff*2/10f;
        }
    }

    private List<TextOptionVisualData> textOptionStates;
    void ShowTextOptions(List<LGDialogChoiceData> textOptions)
    {
        int index = 0;
        int delayIndex = 0;
        
      
        var localTextOptionStates = new List<TextOptionVisualData>();
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
                // Debug.Log("SECRET REQUIREMENTS" +textOption.Text);
                if (SecretRequirementsMet(textOption))
                {
                    // Debug.Log("MET REQUIREMENTS");
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
                    // Debug.Log("NOTMET REQUIREMENTS");
                    textOptionType = TextOptionState.SecretHidden;
                }
            }
            
         
            GameObject prefab = textOptionPrefab;
            if (textOption.NextDialogue is LGFightEventDialogSO fightEvent)
            {
                prefab = fightOptionPrefab;
                battlePreview =  ServiceProvider.Instance.GetSystem<BattleSystem>().GetBattlePreview(party.ActiveUnit, fightEvent.Enemy.Create(Guid.NewGuid()), new GridPosition(0,0), false);
                ShowAttackPreviewUI(battlePreview);
            }
              
            localTextOptionStates.Add(new TextOptionVisualData(textOption,textOptionType,statText, prefab, index, delayIndex));
            index++;
            delayIndex++;
        }

        if (!ListsAreEqual(localTextOptionStates,textOptionStates))
        {
            layout.DeleteAllChildren();
            textOptionStates = new List<TextOptionVisualData>(localTextOptionStates);
            UpdateTextOptions();
        }

       

    }

    private bool ListsAreEqual(List<TextOptionVisualData> list1, List<TextOptionVisualData> list2)
    {
        if (list1.Count != list2.Count)
            return false;
        for (int i = 0; i < list1.Count; i++)
        {
            if (!list1[i].Equals(list2[i]))
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateTextOptions( )
    {
        if (textOptionStates != null)
        {
            textOptionIndex = 0;
            SpawnTextOption(textOptionStates[0]);
        }
        
    }

    struct TextOptionVisualData: IEquatable<TextOptionVisualData>
    {
        public LGDialogChoiceData textOption;
        public TextOptionState textOptionType;
        public string statText;
        public GameObject prefab;
        public int index;
        public int delayIndex;

        public TextOptionVisualData(LGDialogChoiceData lgDialogChoiceData, TextOptionState textOptionState, string s, GameObject o, int i, int delayIndex1)
        {
            textOption = lgDialogChoiceData;
            textOptionType = textOptionState;
            this.statText = s;
            prefab = o;
            index = i;
            delayIndex = delayIndex1;
        }


        public bool Equals(TextOptionVisualData other)
        {
            return Equals(textOption, other.textOption) && textOptionType == other.textOptionType && statText == other.statText && Equals(prefab, other.prefab) && index == other.index && delayIndex == other.delayIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is TextOptionVisualData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(textOption, (int)textOptionType, statText, prefab, index, delayIndex);
        }
    }

    private void SpawnTextOption(TextOptionVisualData textOptionVisualData)
    {
        textOptionIndex++;
        if (textOptionVisualData.textOptionType != TextOptionState.SecretHidden)
        {
            
            var go = Instantiate(textOptionVisualData.prefab, layout);
            var textOptionController = go.GetComponent<TextOptionController>();
            textOptionController.Setup(textOptionVisualData.textOption, textOptionVisualData.textOption.Text, textOptionVisualData.statText, textOptionVisualData.textOptionType, this);
            if (textOptionVisualData.textOptionType != TextOptionState.Locked)
                textOptionController.SetIndex(textOptionVisualData.index);
            textOptionController.onTextAppeared += TextOptionAppeared;
                
        }
        else
        {
            TextOptionAppeared();
        }
    }

    private int textOptionIndex = 0;
    void TextOptionAppeared()
    {
        if (textOptionIndex >= textOptionStates.Count)
            return;
        SpawnTextOption(textOptionStates[textOptionIndex]);
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

    private BattlePreview battlePreview;
    void ActiveUnitChanged()
    {
        UpdateUICharacterRelated();
        if(battlePreview!=null)
            UpdateAttackPreviewUI(battlePreview);
    }
    void Hide()
    {
        canvas.enabled = false;
        party.onActiveUnitChanged -= ActiveUnitChanged;
      
    }


    void MapBattleEnded(bool won)
    {
        Debug.Log("MapBattleEnded: "+won);
        MyDebug.LogTODO("CHANGE BACK TO WON");
        if(true)//TODO CHANGE BACK TO won
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
        AfterBattleTasksFinished();
       
    }

    void AfterBattleTasksFinished()
    {
        AfterBattleTasks.OnFinished -= AfterBattleTasksFinished;
        CheckPossibleRewards();
        AnimationQueue.OnAllAnimationsEnded += AnimationsEnded;
        if(AnimationQueue.IsNoAnimationRunning())
            AnimationsEnded();
       
    }

    void AnimationsEnded()
    {
        AnimationQueue.OnAllAnimationsEnded -= AnimationsEnded;
        //Having some delay here looks nicer
        MonoUtility.DelayFunction(UpdateUI, .75f);
       
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
                case ResourceType.Supplies:
                    if (party.Supplies<res.Amount)
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
            // Debug.Log("Stat Req: "+statReq.AttributeType+" "+statReq.Amount);
            float chance=GetSuccessChance(statReq, party.ActiveUnit);
            combinedChance *= chance;
        }

        float randomValue = UnityEngine.Random.value;
        // Debug.Log("Roll: "+randomValue+" Goal<= "+combinedChance);
        
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
                case ResourceType.Supplies: Player.Instance.Party.AddSupplies(- req.Amount);
                    break;
                case ResourceType.Grace: Player.Instance.Party.AddGrace(- req.Amount);break;
                case ResourceType.Morality: break;
                case ResourceType.HP_Percent:
                     break;
            }
        }
    }
    public void OptionClicked(TextOptionController textOptionController)
    {
        if (Player.Instance.Flags.EventBonds)
        {
            Player.Instance.Party.ActiveUnit.Bonds.Increase(GameBPData.Instance.GetGod("Hermes"),10);
        }
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
                UpdateUIValues();
            }
            else if (currentNode is LGBattleEventDialogSO)
            {
                StartBattle();
            }
            else if (currentNode is LGRandomOutcomeEventDialogSO randomOutcomeEventDialogSo)
            {
                // Debug.Log("RANDOM OUTCOME NODE");
                RandomOutcome(randomOutcomeEventDialogSo);
            }
            else if (currentNode.HeadLine == "Skip")
            {
                EndEvent();
            }
            else
            {
                // Debug.Log("Normal EVENT NODE");
                UpdateUIValues();
            }
            
        }
        else
        {
           EndEvent();
        }
    }


    void RandomOutcome(LGRandomOutcomeEventDialogSO randomNode)
    {
        int sum = 0;
        foreach (var choice in randomNode.Choices)
        {
            if(choice.NextDialogue!=null)
                sum += choice.RandomRate;
        }

        MyDebug.LogTest("Roll Between: "+1+"(inclusive) and "+sum+"(inclusive)");
        int rand = UnityEngine.Random.Range(1, sum+1);
        MyDebug.LogTest("RandomRoll: "+rand);
        int currentSum = 0;
        foreach (var choice in randomNode.Choices)
        {
            if(choice.NextDialogue!=null)
            {
                if (rand <= choice.RandomRate + currentSum)
                {
                    
                    currentNode =(LGEventDialogSO)choice.NextDialogue;
                    UpdateUIValues();
                    return;
                }
                 
                UpdateUIValues();
                //Change to this node
                currentSum += choice.RandomRate;
            }
            
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
        var enemyData = battleDialogSO.battleMap;
        Player.Instance.CurrentEventDialogID=(battleDialogSO.name);
        GameSceneController.Instance.LoadBattleLevel(Scenes.Battle1, enemyData);
        //Get Enemy Army Data/Layout and Map Prefab from Node
        //better: use BattleEncounterNodeData create one specifically for this event and add save and load to node Editor (same way as unitbp for enemytoFight)
        // Then start the battle scene and put data into saveGame so that on return UIEventNode is shown again at the right node...
    }
    void StartFight()
    {
        var battleSystem = AreaGameManager.Instance.GetSystem<BattleSystem>();
        var enemy = ((LGFightEventDialogSO)current.NextDialogue).Enemy.Create(Guid.NewGuid());
            MyDebug.LogLogic("Start Event Battle: "+party.ActiveUnit+" "+enemy);
        battleSystem.StartBattle(party.ActiveUnit, enemy, false, true);
        BattleSystem.OnBattleFinishedBeforeAfterBattleStuff += BattleEnded;
        // AfterBattleTasks.OnFinished += AfterBattleTasksFinished;
        
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
        // Debug.Log("CHECK REWARDS");
        if (currentNode.RewardResources != null)
        {
            // Debug.Log(currentNode.RewardResources.Count);
            foreach (var resource in currentNode.RewardResources)
            {
                // Debug.Log(resource.ResourceType);
                switch (resource.ResourceType)
                {
                    case ResourceType.Gold:
                        // Debug.Log("ADD GOLD " + resource.Amount);
                        Player.Instance.Party.AddGold(resource.Amount);
                        break;
                    case ResourceType.Exp:
                        Player.Instance.Party.ActiveUnit.ExperienceManager.AddExp(resource.Amount);
                        break;
                    case ResourceType.Grace:
                        Player.Instance.Party.AddGrace(resource.Amount);
                        break;
                    case ResourceType.Supplies:
                        Player.Instance.Party.AddSupplies(resource.Amount);
                        break;
                    case ResourceType.Morality:
                        Player.Instance.Party.Morality.AddMorality(resource.Amount);
                        break;
                    case ResourceType.STR:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.STR);
                        break;
                    case ResourceType.DEX:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.DEX);
                        break;
                    case ResourceType.INT:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.INT);
                        break;
                    case ResourceType.AGI:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.AGI);
                        break;
                    case ResourceType.LCK:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.LCK);
                        break;
                    case ResourceType.MaxHP:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.CON);
                        break;
                    case ResourceType.DEF:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.DEF);
                        break;
                    case ResourceType.FTH:
                        Player.Instance.Party.ActiveUnit.Stats.BaseAttributes.IncreaseAttribute(resource.Amount, AttributeType.FTH);
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