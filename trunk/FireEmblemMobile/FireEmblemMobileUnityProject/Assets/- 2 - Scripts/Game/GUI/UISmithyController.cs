using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units.Humans;
using Game.WorldMapStuff.Model;
using Pathfinding;
using UnityEngine;

public class UISmithyController : MonoBehaviour
{
    public Canvas canvas;
    public SmithyEncounterNode node;
    [HideInInspector]
    public Party party;
    //public List<UIShopItemController> shopItems;
    private Smithy smithy;
    public UpgradeWeaponUI upgradeWeaponUI;
    private Weapon currentWeapon;

    public void Show(SmithyEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.smithy = node.smithy;
        UpdateUI();
    }

    public void UpdateUI()
    {
        this.currentWeapon = ((Human)party.ActiveUnit).EquippedWeapon;
        upgradeWeaponUI.Show(currentWeapon,  party.money >= currentWeapon.GetUpgradeCost()&&party.SmithingStones>=currentWeapon.GetUpgradeSmithingStoneCost());
        
    }
    public void UpgradeClicked()
    {
        party.Money -= ((Human)party.ActiveUnit).EquippedWeapon.GetUpgradeCost();
        party.SmithingStones -= ((Human)party.ActiveUnit).EquippedWeapon.GetUpgradeSmithingStoneCost();
        ((Human)party.ActiveUnit).EquippedWeapon.Upgrade();
        UpdateUI();
    }
    public void ContinueClicked()
    {
        FindObjectOfType<UICharacterViewController>().Hide();
        canvas.enabled = false;
        node.Continue();
    }
}
