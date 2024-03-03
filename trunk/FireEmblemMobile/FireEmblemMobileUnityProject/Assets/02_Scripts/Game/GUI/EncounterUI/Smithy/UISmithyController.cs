using Game.DataAndReferences.Data;
using Game.EncounterAreas.Encounters.Smithy;
using Game.EncounterAreas.Model;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Player;
using Game.GUI.CharacterScreen;
using Game.GUI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.EncounterUI.Smithy
{
    public enum WeaponUpgradeMode
    {
        Power,
        Accuracy,
        Critical,
        Special
    }
    public class UISmithyController : MonoBehaviour
    {
        public Canvas canvas;
        public SmithyEncounterNode node;

        [HideInInspector] public Party party;

        //public List<UIShopItemController> shopItems;
        private EncounterAreas.Encounters.Smithy.Smithy smithy;
        [SerializeField] CombineGemUI combineGemUI;
        [SerializeField] InsertGemUI insertGemUI;
        [SerializeField] UpgradeItemUI smithingArea;
        [SerializeField] private UICharacterFace characterFace;
        [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
        [SerializeField] private SmithingSlot relicSlot;

        [SerializeField] private Button smithingButtonAlpha;
        [SerializeField] private Button insertGemsButtonAlpha;
        [SerializeField] private Button combineGemsButtonAlpha;
        [SerializeField] private Button powerSmithingButton;
        [SerializeField] private Button accuracy;
        [SerializeField] private Button criticalSmithingButton;
        [SerializeField] private Button specialSmithingButton;
        [SerializeField] private Image powerSmithingButtonBorder;
        [SerializeField] private Image accuracyBorder;
        [SerializeField] private Image criticalSmithingButtonBorder;
        [SerializeField] private Image specialSmithingButtonBorder;
        [SerializeField] private Material selectedBorderMaterial;
        [SerializeField] private Material defaultBorderMaterial;
        private Weapon selectedWeapon;
        private Relic selectedRelic;
        public SmithyUIState state = SmithyUIState.Smithing;
        [SerializeField] private NPCFaceController npcFaceController;
        private WeaponUpgradeMode upgradeMode;
        public void Show(SmithyEncounterNode node, Party party)
        {
            this.node = node;
            npcFaceController.Show("Welcome, have any weapons you want to upgrade?");
            canvas.enabled = true;
            this.party = party;
            this.smithy = node.smithy;
            this.selectedWeapon = party.ActiveUnit.equippedWeapon;
            specialSmithingButton.gameObject.SetActive(Player.Instance.Flags.SpecialUpgradeUnlocked);
            
            UpdateUI();
            party.onActiveUnitChanged -= ActiveUnitChanged;
            party.onActiveUnitChanged += ActiveUnitChanged;
        }

        public void Hide()
        {
            canvas.enabled = false;
            party.onActiveUnitChanged-= ActiveUnitChanged;
        }
        private void OnDestroy()
        {
            party.onActiveUnitChanged -= ActiveUnitChanged;
        }
        public void UpdateUI()
        {
            if (selectedRelic == null)
            {
                selectedRelic = party.ActiveUnit.EquippedRelic;
            }
            unitIdleAnimation.Show(party.ActiveUnit);
            characterFace.Show(party.ActiveUnit);

            specialSmithingButton.interactable = true;
            powerSmithingButton.interactable =  true;
            accuracy.interactable =  true;
            criticalSmithingButton.interactable = true;
            specialSmithingButtonBorder.material = WeaponUpgradeMode.Special == upgradeMode ? selectedBorderMaterial:defaultBorderMaterial;
        
            powerSmithingButtonBorder.material = WeaponUpgradeMode.Power == upgradeMode ? selectedBorderMaterial:defaultBorderMaterial;

            accuracyBorder.material = WeaponUpgradeMode.Accuracy == upgradeMode ? selectedBorderMaterial:defaultBorderMaterial;

            criticalSmithingButtonBorder.material = WeaponUpgradeMode.Critical == upgradeMode ? selectedBorderMaterial:defaultBorderMaterial;

            combineGemsButtonAlpha.interactable = Player.Instance.Party.Storage.HasGems();
            insertGemsButtonAlpha.interactable = selectedRelic != null;
        
            if (state == SmithyUIState.Smithing)
            {
            
                if (selectedWeapon == null)
                    selectedWeapon = party.ActiveUnit.equippedWeapon;
                // weaponSlot.Show(party.ActiveUnit.equippedWeapon, selectedWeapon == party.ActiveUnit.equippedWeapon);
                insertGemUI.Hide();
                combineGemUI.Hide();
                smithingArea.Show(selectedWeapon, upgradeMode, smithy.GetGoldUpgradeCost(selectedWeapon),
                    smithy.GetStoneUpgradeCost(selectedWeapon), smithy.GetDragonScaleUpgradeCost(selectedWeapon),
                    party.CanAfford(smithy.GetGoldUpgradeCost(selectedWeapon)));

            
           
            }

            if (state == SmithyUIState.InsertGems)
            {
                smithingArea.Hide();
          
                combineGemUI.Hide();
         

                if (selectedRelic != null)
                {
                    insertGemUI.Show(selectedRelic);
                }
                else
                {
                    insertGemUI.Hide();
                }

                // relicSlot.Show(party.ActiveUnit.EquippedRelic, selectedRelic == party.ActiveUnit.EquippedRelic);
          
            }

            if (state == SmithyUIState.CombineGems)
            {
                insertGemUI.Hide();
                smithingArea.Hide();
            
                if (Player.Instance.Party.Storage.HasGems())
                {
                    combineGemUI.Show();
                }
                else
                {
                    combineGemUI.Hide(); 
                }
            }
        }

        public void NextClicked()
        {
            Player.Instance.Party.ActiveUnitIndex++;
       
        }

        public void PrevClicked()
        {
            Player.Instance.Party.ActiveUnitIndex--;
       
        }

        void ActiveUnitChanged()
        {   
            this.selectedWeapon = null;
            this.selectedRelic = null;
            UpdateUI();
        
        }

        public void UpgradeClicked()
        {
        
            party.Money -=smithy.GetGoldUpgradeCost(party.ActiveUnit.equippedWeapon);
            party.Storage.RemoveSmithingStones(smithy.GetStoneUpgradeCost(party.ActiveUnit.equippedWeapon));
            party.Storage.RemoveDragonScales(smithy.GetDragonScaleUpgradeCost(party.ActiveUnit.equippedWeapon));
            party.ActiveUnit.equippedWeapon.Upgrade(upgradeMode);
            if(Player.Instance.Flags.SmithingBonds)
                party.ActiveUnit.Bonds.Increase(GameBPData.Instance.GetGod("Hephaestus"),10);
        
       
            UpdateUI();
        }

  

        public void ContinueClicked()
        {
            FindObjectOfType<UICharacterViewController>().Hide();
            canvas.enabled = false;
            node.Continue();
        }

  
        public void InsertGemsClicked()
        {
            MyDebug.LogInput("InsertGemsClicked");
            state = SmithyUIState.InsertGems;
            UpdateUI();
        }
        public void SmithingClicked()
        {
            MyDebug.LogInput("Smithing Tab Clicked");
            state = SmithyUIState.Smithing;
            UpdateUI();
        }
        public void CombineGemsClicked()
        {
            MyDebug.LogInput("CombineGems");
            state = SmithyUIState.CombineGems;
            UpdateUI();
        }

        public void WeaponClicked()
        {
            if (party.ActiveUnit.equippedWeapon == null)
                return;
            this.selectedWeapon = party.ActiveUnit.equippedWeapon;
            UpdateUI();
        }
        public void Relic1Clicked()
        {
            this.selectedRelic = party.ActiveUnit.EquippedRelic;
            UpdateUI();
        }

        public void PowerSmithingClicked()
        {
            upgradeMode = WeaponUpgradeMode.Power;
            UpdateUI();
        }
        public void AccuracySmithingClicked()
        {
            upgradeMode = WeaponUpgradeMode.Accuracy;
            UpdateUI();
        }
        public void CriticalSmithingClicked()
        {
            upgradeMode = WeaponUpgradeMode.Critical;
            UpdateUI();
        }
        public void SpecialSmithingClicked()
        {
            upgradeMode = WeaponUpgradeMode.Special;
      
        
            UpdateUI();
        }

        public enum SmithyUIState
        {
            InsertGems,
            CombineGems,
            Smithing
        }
    }
}