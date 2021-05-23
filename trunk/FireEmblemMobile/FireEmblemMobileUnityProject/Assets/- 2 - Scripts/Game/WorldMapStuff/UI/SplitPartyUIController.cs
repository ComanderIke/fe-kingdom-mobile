using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;
using IPartyActionRenderer = Game.WorldMapStuff.Interfaces.IPartyActionRenderer;

public class SplitPartyUIController : IPartyActionRenderer
{
    public GameObject PartyPrefab;

    public Transform partyParent;
    public Transform partyParent2;
    public GameObject party2Panel;
    private List<PartyMemberUIController> partyMembers;
    private List<PartyMemberUIController> partyMembers2;
    void Start()
    {
        partyMembers = new List<PartyMemberUIController>();
        partyMembers2 = new List<PartyMemberUIController>();
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    public override void Show(Party party)
    {

        gameObject.SetActive(true);
        partyMembers.Clear();
        foreach (Transform tfm in partyParent)
        {
            Destroy(tfm.gameObject);
        }

        foreach (var member in party.members)
        {
            var go = Instantiate(PartyPrefab, partyParent, false);
            partyMembers.Add(go.GetComponent<PartyMemberUIController>());
        }

        gameObject.SetActive(true);
        if (party.location.worldMapPosition.GetActors().Count == 2)
        {
            var party2 = party.location.worldMapPosition.GetActors()[0] == party
                ? (Party) party.location.worldMapPosition.GetActors()[1]
                : (Party) party.location.worldMapPosition.GetActors()[0];
            foreach (Transform tfm in partyParent2)
            {
                Destroy(tfm.gameObject);
            }

            foreach (var member in party2.members)
            {
                var go = Instantiate(PartyPrefab, partyParent2, false);
                var controller = go.GetComponent<PartyMemberUIController>();
                partyMembers2.Add(controller);
                controller.SetText(member.name);
                controller.SetSprite(member.visuals.CharacterSpriteSet.MapSprite);

            }

            party2Panel.SetActive(true);
        }
        else
        {
            party2Panel.SetActive(false);
        }
    }
    public void SplitClicked()
    {
        Debug.Log("Split clicked!");
        WM_PartyActionSystem.OnSplitClicked?.Invoke();
    }
    public void JoinClicked()
    {
        Debug.Log("Join Clicked!");
        WM_PartyActionSystem.OnJoinClicked?.Invoke();
    }
}
