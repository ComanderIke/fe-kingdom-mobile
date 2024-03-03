using Game.EncounterAreas.Model;
using Game.GUI.EncounterUI.Church;
using Game.GUI.EncounterUI.Event;
using Game.GUI.EncounterUI.Inn;
using Game.GUI.EncounterUI.Merchant;
using Game.GUI.EncounterUI.Smithy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GUI.Controller
{
    public class EncounterUIController : MonoBehaviour
    {
        [FormerlySerializedAs("Gold")] public UIRessourceAmount goldAmount;
        public UIRessourceAmount graceAmount;
        public UIRessourceAmount suppliesAmount;
        public UIInnController UIInnController;
        public UISmithyController UISmithyController;
        public UIChurchController UIChurchController;
        public UIEventController UIEventController;
        public UIMerchantController UIMerchantController;

        public UIMoralityBar MoralityBar;
        private Party party;
        // Start is called before the first frame update
        public void Init(Party party)
        {
            this.party = party;
            party.onGoldChanged += GoldChanged;
            party.onSuppliesChanged += SuppliesChanged;
            party.onGraceChanged += GraceChanged;
            GoldChanged(party.Money);
            GraceChanged(party.CollectedGrace);
            SuppliesChanged(party.Supplies);
            MoralityBar.Show(party.Morality);
        }

        private void OnDestroy()
        {
            if (party != null)
            {
                party.onGoldChanged -= GoldChanged;
                party.onGraceChanged -= GraceChanged;
                party.onSuppliesChanged -= SuppliesChanged;
            }
        }

        void GoldChanged(int gold)
        {
            goldAmount.Amount = gold;
        }
        void SuppliesChanged(int supplies)
        {
            suppliesAmount.Amount = supplies;
        }
        void GraceChanged(int grace)
        {
            graceAmount.Amount = grace;
        }

        public void UpdateUIScreens()
        {
            if (UIInnController.canvas.enabled)
            {
                UIInnController.UpdateUI();
            }
            if (UISmithyController.canvas.enabled)
            {
                UISmithyController.UpdateUI();
            }
            if (UIMerchantController.canvas.enabled)
            {
                UIMerchantController.UpdateUI();
            }
            if (UIChurchController.canvas.enabled)
            {
                UIChurchController.UpdateUI();
            }
            if (UIEventController.canvas.enabled)
            {
                UIEventController.UpdateUI();
            }
   
        }
    }
}
