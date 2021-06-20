using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
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
        foreach (var cond in Campaign.Instance.victoryDefeatConditions)
        {
            condStr += cond.description+", ";
        }
        partyName.SetTextWithoutNotify(party.name);
        conditions.SetText(condStr);
        UnitClicked(selectedUnitIndex);
        instantiatedMembers = new List<PartyMemberUIController>();
        instantiatedMembers.Clear();
        for(int i=partyMemberParent.childCount-1; i >=0;i--)
        {
            DestroyImmediate(partyMemberParent.GetChild(i).gameObject);
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
        foreach (var member in instantiatedMembers)
        {
            if (member.UnitIndex == selectedUnitIndex)
            {
                member.Select();
            }
            else
            {
                member.ResetVisuals();
            }
        }

       
        characterView.Show(party.members[index]);
    }

    public void UpdatePartyOrder()
    {
        Debug.Log("Before:");
       
        PartyMemberUIController[] tmpUnits= partyMemberParent.GetComponentsInChildren<PartyMemberUIController>();
        foreach (var member in party.members)
        {
            Debug.Log(member.name);
        }

        List<Unit> indexLookUp = new List<Unit>(party.members);

        for (int i = 0; i < tmpUnits.Length; i++)
        {
            var member = indexLookUp[tmpUnits[i].UnitIndex];
            if (indexLookUp.IndexOf(member) != i)
            {
                Debug.Log("i: "+i+" index: "+party.members.IndexOf(member) +"member: "+member.name);
                party.members.Remove(member);
                party.members.Insert(i, member);
            }
           
        }
Debug.Log("After:");
        foreach (var member in party.members)
        {
            Debug.Log(member.name);
        }
        
     
        Show(party);
    }
}