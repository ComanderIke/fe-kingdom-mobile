using System;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
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

            if (price.text == "Free")
                price.text = button.interactable ? "<bounce>Free" : "</bounce>Free";
        } 
        void UpdateUI()
        {
            this.price.SetText(recipe.price==0?"Free":""+recipe.price);
            price.color = affordable ? Color.white : Color.red;
            coinIcon.gameObject.SetActive(recipe.price!=0);
            name.text = recipe.name;
            description.text = "+ " + recipe.heal + "% Hp";
            string attributeText = "";
            foreach (var attType in recipe.AttributeType)
            {
                attributeText+=Attributes.GetAsText((int)attType)+"/";
            }
            if(attributeText.Length>=2)
                attributeText=attributeText.Remove(attributeText.Length - 1);
            bonus.text = "+ "+recipe.bonuses +" "+attributeText+( recipe.bonusType == Recipe.InnBonusType.Permanent?" permanent":"");
            if(recipe.bonusType == Recipe.InnBonusType.Exp)
                bonus.text = "+ "+recipe.bonuses +" exp";
            if(recipe.bonusType == Recipe.InnBonusType.RefreshSkills)
                bonus.text = "Refresh Skills";
            bonus.gameObject.SetActive(recipe.bonuses!=0);
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