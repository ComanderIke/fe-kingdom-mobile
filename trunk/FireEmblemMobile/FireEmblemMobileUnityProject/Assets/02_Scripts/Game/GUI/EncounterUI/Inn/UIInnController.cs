using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;

namespace Game.GUI.EncounterUI.Inn
{
    public class UIInnController : MonoBehaviour
    {
        // Start is called before the first frame update
        public Canvas canvas;
        [HideInInspector]
        public Party party;
        // public TextMeshProUGUI personName;
        //public TextMeshProUGUI talkText;
        [SerializeField] private NPCFaceController npcFaceController;
        
        [SerializeField] private List<UICharacterFace> characterFaces;
        [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
        [SerializeField] private TextMeshProUGUI restDescription;
        [SerializeField] private TextMeshProUGUI drinkDescription;
        [SerializeField] private TextMeshProUGUI eatDescription;
        [SerializeField] private TextMeshProUGUI restPriceText;
        [SerializeField] private TextMeshProUGUI drinkPriceText;
        [SerializeField] private TextMeshProUGUI eatPriceText;
        [SerializeField] private GameObject restCoinIcon;
        [SerializeField] private GameObject drinkCoinIcon;
        [SerializeField] private GameObject eatCoinIcon;

        [SerializeField] private TimeOfDayManager timeManager;
        public UIInnItem innItemRest;
        public UIInnItem innItemSpecial;
        public UIInnItem innItemSmall1;
        public UIInnItem innItemSmall2;
        public UIInnItem innItemSmall3;
        public UIInnItem innItemSmall4;
       // public UIInnItem innItemSmall5;
        //public UIInnItem innItemSmall6;
        [SerializeField] private Recipe restItem;
        [SerializeField] private List<Recipe> specialItemPool;
        [SerializeField] private List<Recipe> itemsPool;
        private List<Recipe> items;
        private Recipe specialItem;
        
        private List<Recipe> used;
        private InnEncounterNode node;
        public static float PriceRate { get; set; }
        public static int FoodSlots { get; set; }

        private void OnDestroy()
        {
            party.onActiveUnitChanged -= ActiveUnitChanged;
        }

        void CreateRandomItemsFromPool()
        {
            specialItem = specialItemPool[Random.Range(0, specialItemPool.Count)];
            items = new List<Recipe>();
            for (int i = 0; i < 6; i++)
            {
                var item = itemsPool[Random.Range(0, itemsPool.Count)];
                while (items.Contains(item))
                {
                    item = itemsPool[Random.Range(0, itemsPool.Count)];
                }
                items.Add(item);
            }
        }
        public void Show(InnEncounterNode node, Party party)
        {
            Debug.Log("Showing inn ui screen");
            npcFaceController.Show("Welcome how about a nice drink, some grilled meat and a warm bed after?");
            canvas.enabled = true;
            this.node = node;
            this.party = party;
            used = new List<Recipe>();
            party.onActiveUnitChanged -= ActiveUnitChanged;
            party.onActiveUnitChanged += ActiveUnitChanged;
            CreateRandomItemsFromPool();
            UpdateUI();
            // for (int i=0; i<inn.shopItems.Count; i++)
            // {
            //     bool affordable = party.money >= inn.shopItems[i].cost;
            //     shopItems[i].SetValues(inn.shopItems[i], affordable, this);
            // }
            //questOption.SetValues(inn.quest);
            //recruitCharacter.SetValues(inn.recruitableCharacter);
            // FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
        }
        public void NextClicked()
        {
            Player.Instance.Party.ActiveUnitIndex++;
        }
        public void Consume(Recipe item)
        {
            foreach(var unit in Player.Instance.Party.members)
                unit.Heal((int)((unit.MaxHp/100f)*item.heal));
            if (item.bonuses != 0)
            {
                if (item.bonusType == Recipe.InnBonusType.Exp)
                {
                    foreach (var member in party.members)
                    {
                        member.ExperienceManager.AddExp(item.bonuses);
                    }
                }
                else if (item.bonusType == Recipe.InnBonusType.Permanent)
                {
                    foreach (var member in party.members)
                    {
                        foreach(var attType in item.AttributeType)
                            member.Stats.BaseAttributes.IncreaseAttribute(item.bonuses,attType);
                    }
                }
                else if (item.bonusType == Recipe.InnBonusType.Temporary)
                {
                    foreach (var member in party.members)
                    {
                        foreach(var attType in item.AttributeType)
                            member.Stats.BonusAttributesFromFood.IncreaseAttribute(item.bonuses,attType);
                    }
                    Debug.Log("TODO Remove After Battle");
                }
                else if (item.bonusType == Recipe.InnBonusType.RefreshSkills)
                {
                    foreach (var member in party.members)
                    {
                        member.SkillManager.RefreshSkills();
                    }
                    Debug.Log("TODO Remove After Battle");
                }
            }
            Player.Instance.Party.AddGold(-item.price);
            used.Add(item);
            UpdateUI();
            
        }
        public void PrevClicked()
        {
            Player.Instance.Party.ActiveUnitIndex--;
        }
        public void Hide()
        {
            canvas.enabled = false;
            party.onActiveUnitChanged -= ActiveUnitChanged;
   
        }

        void ActiveUnitChanged()
        {
            UpdateUI();
        
        }
        public void ItemClicked(int index)
        {
            Consume(items[index]);
            UpdateUIValues();
        }
        public void SpecialCLicked()
        {
            Consume(specialItem);
            UpdateUIValues();
        }
        public void RestClicked()
        {
            Consume(restItem);
            timeManager.SetNoon();
            UpdateUIValues();
        }
        public void ContinueClicked()
        {
            canvas.enabled=false;
            node.Continue();
            FindObjectOfType<UICharacterViewController>().Hide();
        }
        public void AcceptQuestClicked()
        {
        
        }

        public void RecruitCharacterClicked()
        {
        
        }

        void UpdateUIValues()
        {
           // innItemRest
         
           innItemRest.SetValues(restItem);
           innItemSpecial.SetValues(specialItem);
           innItemSmall1.SetValues(items[0]);
           innItemSmall2.SetValues(items[1]);
           innItemSmall3.SetValues(items[2]);
           innItemSmall4.SetValues(items[3]);
           // innItemSmall5.SetValues(items[4]);
           // innItemSmall6.SetValues(items[5]);
           innItemRest.SetInteractable(!used.Contains((restItem)));
           innItemSpecial.SetInteractable(!used.Contains((specialItem)));
           innItemSmall1.SetInteractable(!used.Contains((items[0])));
           innItemSmall2.SetInteractable(!used.Contains((items[1])));
           innItemSmall3.SetInteractable(!used.Contains((items[2])));
           innItemSmall4.SetInteractable(!used.Contains((items[3])));
           // innItemSmall5.SetInteractable(!used.Contains((items[4])));
           // innItemSmall6.SetInteractable(!used.Contains((items[5])));
           
            
            
            
        }
        public void UpdateUI()
        {
            unitIdleAnimation.Show(party.ActiveUnit);
            int index = 0;
            if (party.members.Count == 4)
            {
                characterFaces[0].Show(party.members[0]);
                characterFaces[1].Show(party.members[1]);
                characterFaces[2].Hide();
                characterFaces[3].Show(party.members[2]);
                characterFaces[4].Show(party.members[3]);
                characterFaces[5].Hide();
            }
            else
            {
                for (int i = 0; i < characterFaces.Count; i++)
                {
                    characterFaces[i].Hide();
                    if (party.members.Count > i)
                    {
                        characterFaces[i].Show(party.members[i]);
                    }
                }
            }

            UpdateUIValues();

       
        }
       
       
    }
}