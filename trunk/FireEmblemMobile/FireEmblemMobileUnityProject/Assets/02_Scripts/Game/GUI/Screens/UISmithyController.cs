using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units.Humans;
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
    [SerializeField] UpgradeItemUI selectedItemUI;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    [SerializeField] private SmithingSlot weaponSlot;
    [SerializeField] private SmithingSlot relicSlot;
    [SerializeField] private SmithingSlot relicSlot2;
    private EquipableItem currentEquipment;

    public void Show(SmithyEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.smithy = node.smithy;
        this.currentEquipment = party.ActiveUnit.equippedWeapon;
        UpdateUI();
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

    public void UpdateUI()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
      
        weaponSlot.Show(party.ActiveUnit.equippedWeapon, currentEquipment==party.ActiveUnit.equippedWeapon);
        relicSlot.Show(party.ActiveUnit.EquippedRelic1,currentEquipment==party.ActiveUnit.EquippedRelic1);
        relicSlot2.Show(party.ActiveUnit.EquippedRelic2,currentEquipment==party.ActiveUnit.EquippedRelic2);
        selectedItemUI.Show(currentEquipment,
            party.money >= currentEquipment.GetUpgradeCost() &&
            party.SmithingStones >= currentEquipment.GetUpgradeSmithingStoneCost());
    }

    public void NextClicked()
    {
        Player.Instance.Party.ActiveUnitIndex++;
        this.currentEquipment = party.ActiveUnit.equippedWeapon;
        UpdateUI();
    }

    public void PrevClicked()
    {
        Player.Instance.Party.ActiveUnitIndex--;
        this.currentEquipment = party.ActiveUnit.equippedWeapon;
        UpdateUI();
    }

    public void UpgradeClicked()
    {
        party.Money -= party.ActiveUnit.equippedWeapon.GetUpgradeCost();
        party.SmithingStones -= party.ActiveUnit.equippedWeapon.GetUpgradeSmithingStoneCost();
        party.ActiveUnit.equippedWeapon.Upgrade();
        UpdateUI();
    }

  

    public void ContinueClicked()
    {
        FindObjectOfType<UICharacterViewController>().Hide();
        canvas.enabled = false;
        node.Continue();
    }

    public void WeaponClicked()
    {
        if (party.ActiveUnit.equippedWeapon == null)
            return;
        this.currentEquipment = party.ActiveUnit.equippedWeapon;
        UpdateUI();
    }
    public void Relic1Clicked()
    {
        if (party.ActiveUnit.EquippedRelic1 == null)
            return;
        this.currentEquipment = party.ActiveUnit.EquippedRelic1;
        UpdateUI();
    }
    public void Relic2Clicked()
    { 
        if (party.ActiveUnit.EquippedRelic2 == null)
            return;
        this.currentEquipment = party.ActiveUnit.EquippedRelic2;
        UpdateUI();
    }
}