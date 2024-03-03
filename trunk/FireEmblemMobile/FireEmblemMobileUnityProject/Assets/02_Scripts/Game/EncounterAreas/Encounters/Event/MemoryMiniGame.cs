using System;
using System.Collections.Generic;
using Game.EncounterAreas.Model;
using Game.GameActors.Items;
using Game.GUI.Layouts;
using Game.GUI.Screens;
using Game.Utility;
using TMPro;
using UnityEngine;

namespace Game.EncounterAreas.Encounters.Event
{
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
        private List<ItemBP> revealedItems;
    

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
            revealedItems = new List<ItemBP>();
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
            OnComplete?.Invoke();
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
                    revealedItems.Add((ItemBP)currentRevealedCard.userData);
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

        public Reward GetRewards()
        {
            Reward reward = new Reward();
            List<ItemBP> ret = new List<ItemBP>();
            foreach(var item in revealedItems)
            {
                Debug.Log("Revealed Item: "+item);
                ret.Add(item);
            }

            reward.itemBp = ret;
            revealedCards.Clear();
            revealedItems.Clear();
            return reward;
        }

        public event Action OnComplete;
    }
}