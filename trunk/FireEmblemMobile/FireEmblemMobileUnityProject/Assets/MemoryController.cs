using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MemoryController : MonoBehaviour
{
    public MemoryButtonController currentRevealedCard;
    public List<MemoryButtonController> revealedCards;

    public List<Sprite> spriteData;

    private List<MemoryButtonController> allCards;
    private List<MemoryButtonController>startCards;
    public TextMeshProUGUI triesleft;

    public int MaxTries = 5;

    public int currentTries = 0;
    // Start is called before the first frame update
    void Start()
    {
        startCards = GetComponentsInChildren<MemoryButtonController>().ToList();
        allCards = new List<MemoryButtonController>();
        foreach (var sprite in spriteData)
        {
            int rng = Random.Range(0, startCards.Count);
            var card = startCards[rng];
            allCards.Add(card);
            card.MemoryController = this;
            card.itemSprite = sprite;
            startCards.RemoveAt(rng);
            rng = Random.Range(0, startCards.Count);
            card = startCards[rng];
            allCards.Add(card);
            card.MemoryController = this;
            card.itemSprite = sprite;
            startCards.RemoveAt(rng);
        }
        triesleft.text = "Tries left: " + currentTries + "/" + MaxTries;
    }

    
    private void IncreaseTries()
    {
        currentTries++;
        if (currentTries >= MaxTries)
        {
            foreach (var card in allCards)
            {
                card.SetInActive();
            }
        }

        triesleft.text = "Tries left: " + currentTries + "/" + MaxTries;
    }
    public bool TurnBack(MemoryButtonController revealed)
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
                if( currentTries<MaxTries)
                    MonoUtility.DelayFunction(() => card.SetActive(), 1.0f);
            }
            
           
            if (currentRevealedCard.itemSprite == revealed.itemSprite)
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
