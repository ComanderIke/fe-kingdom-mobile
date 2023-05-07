using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units.Humans;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using Pathfinding;
using UnityEngine;

public class UISmithyController : MonoBehaviour
{
    public Canvas canvas;
    public SmithyEncounterNode node;

    [HideInInspector] public Party party;

    //public List<UIShopItemController> shopItems;
    private Smithy smithy;
    [SerializeField] CombineGemUI combineGemUI;
    [SerializeField] InsertGemUI insertGemUI;
    [SerializeField] UpgradeItemUI smithingArea;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    [SerializeField] private SmithingSlot weaponSlot;
    [SerializeField] private SmithingSlot relicSlot;
    [SerializeField] private SmithingSlot relicSlot2;
    [SerializeField] private CanvasGroup smithingButtonAlpha;
    [SerializeField] private CanvasGroup insertGemsButtonAlpha;
    [SerializeField] private CanvasGroup combineGemsButtonAlpha;
    private Weapon selectedWeapon;
    private Relic selectedRelic;
    public SmithyUIState state = SmithyUIState.Smithing;
    public void Show(SmithyEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.smithy = node.smithy;
        this.selectedWeapon = party.ActiveUnit.equippedWeapon;
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
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        if (state == SmithyUIState.Smithing)
        {
            if (selectedWeapon == null)
                selectedWeapon = party.ActiveUnit.equippedWeapon;
            weaponSlot.Show(party.ActiveUnit.equippedWeapon, selectedWeapon == party.ActiveUnit.equippedWeapon);
            insertGemUI.Hide();
            combineGemUI.Hide();
            smithingArea.Show(selectedWeapon, smithy.GetGoldUpgradeCost(selectedWeapon),
                smithy.GetStoneUpgradeCost(selectedWeapon), smithy.GetDragonScaleUpgradeCost(selectedWeapon),
                party.CanAfford(smithy.GetGoldUpgradeCost(selectedWeapon)));
            smithingButtonAlpha.alpha = 1.0f;
            combineGemsButtonAlpha.alpha = 0.6f;
            insertGemsButtonAlpha.alpha = 0.6f;
        }

        if (state == SmithyUIState.InsertGems)
        {
            smithingArea.Hide();
            combineGemUI.Hide();
            if (selectedRelic == null)
            {
                selectedRelic = party.ActiveUnit.EquippedRelic1;
                if(selectedRelic==null)
                    selectedRelic = party.ActiveUnit.EquippedRelic2;
            }

            if (selectedRelic != null)
            {
                insertGemUI.Show(selectedRelic);
            }
            else
            {
                insertGemUI.Hide();
            }

            relicSlot.Show(party.ActiveUnit.EquippedRelic1, selectedRelic == party.ActiveUnit.EquippedRelic1);
            relicSlot2.Show(party.ActiveUnit.EquippedRelic2, selectedRelic == party.ActiveUnit.EquippedRelic2);
            smithingButtonAlpha.alpha = 0.6f;
            combineGemsButtonAlpha.alpha = 0.6f;
            insertGemsButtonAlpha.alpha = 1.0f;
        }

        if (state == SmithyUIState.CombineGems)
        {
            insertGemUI.Hide();
            smithingArea.Hide();
            smithingButtonAlpha.alpha = 0.6f;
            combineGemsButtonAlpha.alpha = 1.0f;
            insertGemsButtonAlpha.alpha = 0.6f;
            combineGemUI.Show();
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
        party.Convoy.RemoveSmithingStones(smithy.GetStoneUpgradeCost(party.ActiveUnit.equippedWeapon));
        party.Convoy.RemoveDragonScales(smithy.GetDragonScaleUpgradeCost(party.ActiveUnit.equippedWeapon));
        party.ActiveUnit.equippedWeapon.Upgrade();
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
        Debug.Log("InsertGemsClicked");
        state = SmithyUIState.InsertGems;
        UpdateUI();
    }
    public void SmithingClicked()
    {
        Debug.Log("SmithingClicked");
        state = SmithyUIState.Smithing;
        UpdateUI();
    }
    public void CombineGemsClicked()
    {
        Debug.Log("CombineGems");
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
        this.selectedRelic = party.ActiveUnit.EquippedRelic1;
        UpdateUI();
    }
    public void Relic2Clicked()
    { 

        this.selectedRelic = party.ActiveUnit.EquippedRelic2;
        UpdateUI();
    }

    public enum SmithyUIState
    {
        InsertGems,
        CombineGems,
        Smithing
    }
}