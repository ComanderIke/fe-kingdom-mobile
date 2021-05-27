using System;
using System.Collections;
using System.Collections.Generic;
using Game.Mechanics;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;

public class UIPartyOverViewController : MonoBehaviour
{
    public TMP_InputField partyName;

    public TextMeshProUGUI turnCount;

    public TextMeshProUGUI conditions;

    public TextMeshProUGUI resources;

    public GameObject nextPartyButton;

    public GameObject previousPartyButton;
   

    public UICharacterViewController characterView;

    public Transform partyMemberParent;
    public GameObject partyMemberPrefab;
    public List<PartyMemberUIController> instantiatedMembers;
    
    [HideInInspector]
    public int selectedUnitIndex;
    [HideInInspector]
    public int currentPartyIndex = 0;
    [HideInInspector]
    public WM_Faction playerFaction;
    [HideInInspector]
    public Party party;

    public void Show(Party party)
    {
        this.party = party;
        playerFaction = party.Faction;
        turnCount.SetText("Turn: "+WorldMapGameManager.Instance.GetSystem<TurnSystem>().TurnCount);
        string condStr = "";
        foreach (var cond in FindObjectOfType<CampaignConfig>().campaign.victoryDefeatConditions)
        {
            condStr += cond.description+", ";
        }
        partyName.SetTextWithoutNotify(party.name);
        conditions.SetText(condStr);
        UnitClicked(selectedUnitIndex);
        instantiatedMembers = new List<PartyMemberUIController>();
        instantiatedMembers.Clear();
        foreach (Transform tfm in partyMemberParent)
        {
            Destroy(tfm.gameObject);
        }

        int index = 0;
        foreach (var member in party.members)
        {
            var go = Instantiate(partyMemberPrefab, partyMemberParent, false);
            instantiatedMembers.Add(go.GetComponent<PartyMemberUIController>());
            var controller = go.GetComponent<PartyMemberUIController>();
            controller.OverviewController = this;
            controller.UnitIndex = index;
            index++;
            controller.SetText(member.name);
            controller.SetSprite(member.visuals.CharacterSpriteSet.MapSprite);
        }

        
    }

    public void NextPartyClicked()
    {
        currentPartyIndex++;
        if (currentPartyIndex >= playerFaction.Parties.Count)
            currentPartyIndex = 0;
        Show(playerFaction.Parties[currentPartyIndex]);
    }

    public void PrevPartyClicked()
    {
        currentPartyIndex--;
        if (currentPartyIndex < 0)
            currentPartyIndex = playerFaction.Parties.Count - 1;
        Show(playerFaction.Parties[currentPartyIndex]);
    }
    public void UnitClicked(int index)
    {
        selectedUnitIndex = index;
        characterView.Show(party.members[index]);
    }
    
}