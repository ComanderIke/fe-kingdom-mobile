using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using TMPro;
using UnityEngine;

public class MemoryMiniGame : MonoBehaviour
{
    public MemoryButton currentRevealedCard;
    public List<MemoryButton> revealedCards;
    

    private List<MemoryButton> allCards;
   // private List<MemoryButton>startCards;
    public TextMeshProUGUI triesleft;
    
    public MemoryGameData memoryData;
    [SerializeField] private GameObject memoryButtonPrefab;
    [SerializeField] private FlexibleGridLayout gridLayout;
    private List<ItemBP> shuffledItems;
    [SerializeField] private Canvas canvas;
    public int currentTries = 0;
    

    // Start is called before the first frame update
    public void Show(MemoryGameData memoryGameData)
    {
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
        }
        triesleft.text = "Tries left: " + currentTries + "/" + memoryData.MaxTries;
    }

    public void Hide()
    {
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
        if (currentTries >= memoryData.MaxTries)
        {
            foreach (var card in allCards)
            {
                card.SetInActive();
            }
        }

        triesleft.text = "Tries left: " + currentTries + "/" + memoryData.MaxTries;
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
                if( currentTries<memoryData.MaxTries)
                    MonoUtility.DelayFunction(() => card.SetActive(), 1.0f);
            }
            
           
            if (currentRevealedCard.itemSprite == revealed.itemSprite&& currentRevealedCard.userData==revealed.userData)
            {
                revealedCards.Add(revealed);
                revealedCards.Add(currentRevealedCard);
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
}