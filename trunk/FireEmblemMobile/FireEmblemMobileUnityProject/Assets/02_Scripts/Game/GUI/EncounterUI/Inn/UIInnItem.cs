using System;
using Game.GameActors.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.EncounterUI.Inn
{
    public class UIInnItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI bonus;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private Image coinIcon;
        private bool affordable = false;
        private Recipe recipe;
        
        public void SetInteractable(bool interactable)
        {
            if (affordable && interactable)
                button.interactable = true;
            else
            {
                button.interactable = false;
            }
        } 
        void UpdateUI()
        {
            this.price.SetText(recipe.price==0?"Free":""+recipe.price);
            price.color = affordable ? Color.white : Color.red;
            coinIcon.gameObject.SetActive(recipe.price!=0);
            name.text = recipe.name;
            description.text = "Heal " + recipe.heal + " % Hp";
            bonus.text = recipe.bonuses;
            bonus.gameObject.SetActive(!String.IsNullOrEmpty(recipe.bonuses));
            icon.sprite = recipe.icon;
            button.interactable = affordable;
        }

        public void SetValues(Recipe item)
        {
            this.recipe = item;
            this.affordable = Player.Instance.Party.CanAfford(item.price);
            
            UpdateUI();
        }
    }
}