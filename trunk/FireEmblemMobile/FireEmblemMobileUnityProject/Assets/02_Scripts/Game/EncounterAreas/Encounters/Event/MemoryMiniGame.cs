using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryMiniGame : MonoBehaviour
{
    public MemoryButton currentRevealedCard;
    public List<MemoryButton> revealedCards;
    

    private List<MemoryButton> allCards;
   // private List<MemoryButton>startCards;
    public TextMeshProUGUI triesleft;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    public MemoryGameData memoryData;
    [SerializeField] private GameObject memoryButtonPrefab;
    [SerializeField] private FlexibleGridLayout gridLayout;
    private List<ItemBP> shuffledItems;
    [SerializeField] private Canvas canvas;
    public int currentTries = 0;
    private Party party;
    

    // Start is called before the first frame update
    public void Show(MemoryGameData memoryGameData, Party party)
    {
        party.onActiveUnitChanged += UpdateUI;
        foreach (var member in party.members)
        {
            member.HpValueChanged += UpdateHealthRelatedUI;
        }
        this.party = party;
        this.canvas.enabled = true;
        this.memoryData = memoryGameData;
        gridLayout.columns = memoryGameData.columns;
        //startCards = GetComponentsInChildren<MemoryButton>().ToList();
        allCards = new List<MemoryButton>();
        shuffledItems = new List<ItemBP>(memoryGameData.items);
        shuffledItems.AddRange(memoryGameData.items);
        Randomize(shuffledItems);
        foreach (var item in shuffledItems)
        {
            var go=Instantiate(memoryButtonPrefab, gridLayout.transform);
            var memoryButton = go.GetComponent<MemoryButton>();
            allCards.Add(memoryButton);
            memoryButton.MemoryController = this;
            memoryButton.userData = item;
            memoryButton.itemSprite = item.sprite;
            if(CanTurnField())
                memoryButton.SetActive();
            else
            {
                memoryButton.SetInActive();

            }
        }
        UpdateUI();
        triesleft.gameObject.SetActive(memoryData.hpCost == 0);
        triesleft.text = "Tries left: " + currentTries + "/" + memoryData.MaxTries;

    }

    void UpdateUI()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);  
        UpdateHealthRelatedUI();
        triesleft.text = "Tries left: " + currentTries + "/" + memoryData.MaxTries;
    }

    void UpdateHealthRelatedUI()
    {
        if(!CanTurnField())
        {
            foreach (var card in allCards)
            {
                if(!card.revealed)
                    card.SetInActive();
            }
        }
        else
        {
            foreach (var card in allCards)
            {
                card.SetActive();
            }
        }
    }
    
    public void Hide()
    {
        party.onActiveUnitChanged -= UpdateUI;
        foreach (var member in party.members)
        {
            member.HpValueChanged -= UpdateHealthRelatedUI;
        }
        canvas.enabled = false;
    }
    
    public void Randomize<T>(List<T> items)
    {
        for (int i = 0; i < items.Count - 1; i++)
        {
            int j = UnityEngine.Random.Range(i, items.Count);
            (items[i], items[j]) = (items[j], items[i]);
        }
    }

    
    private void IncreaseTries()
    {
        currentTries++;
        UpdateHealthRelatedUI();
    }
    public bool TurnBack(MemoryButton revealed)
    {
        if (currentRevealedCard == null)
        {
            currentRevealedCard = revealed;
            return false;
        }
        else
        {
            IncreaseTries();
           
            foreach (var card in allCards)
            {
                card.SetInActive();
               
            }
            MonoUtility.DelayFunction(() => UpdateHealthRelatedUI(), 1.0f);
            
           
            if (currentRevealedCard.itemSprite == revealed.itemSprite&& currentRevealedCard.userData==revealed.userData)
            {
                revealedCards.Add(revealed);
                revealedCards.Add(currentRevealedCard);
                party.Convoy.AddItem(((ItemBP)currentRevealedCard.userData).Create());
                currentRevealedCard = null;
               
                return false;
            }
            else
            {
                MonoUtility.DelayFunction(()=>
                {
                    currentRevealedCard.TurnBack();
                    currentRevealedCard = null;
                },1.0f);
                
                return true;
            }
        }
        

        return true;
    }

    public void RevealField(MemoryButton memoryButton)
    {
        party.ActiveUnit.Hp -= memoryData.hpCost;
    }

    public bool CanTurnField()
    {
        if (memoryData.hpCost != 0)
        {
            if (party.ActiveUnit.Hp > memoryData.hpCost)
                return true;
            return false;
        }

        return currentTries < memoryData.MaxTries;
    }
}