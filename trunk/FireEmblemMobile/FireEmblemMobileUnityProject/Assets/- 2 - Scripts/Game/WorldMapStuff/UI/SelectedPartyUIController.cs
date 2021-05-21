using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using TMPro;
using UnityEngine;

public class SelectedPartyUIController : MonoBehaviour,IPartyActionRenderer
{
    public GameObject PartyPrefab;

    public Transform partyParent;
    public Transform partyParent2;
    public GameObject party2Panel;
    public TextMeshProUGUI currentPartyText;
    public TextMeshProUGUI Party2Text;
    public GameObject SwapButtonPanel;
    public GameObject SplitJoinPanel;
    public GameObject SplitPartyButton;

    public GameObject JoinPartyButton;

    private List<PartyMemberUIController> partyMembers;
    private List<PartyMemberUIController> partyMembers2;
    // Start is called before the first frame update
    void Start()
    {
        partyMembers = new List<PartyMemberUIController>();
        partyMembers2 = new List<PartyMemberUIController>();
    }

    public void Show(Party party)
    {
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
                ?(Party) party.location.worldMapPosition.GetActors()[1]
                : (Party)party.location.worldMapPosition.GetActors()[0];
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

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void UpdatePanel()
    {
        if(SplitPartyButton.activeSelf||JoinPartyButton.activeSelf)
            SplitJoinPanel.SetActive(true);
        else
        {
            SplitJoinPanel.SetActive(false);
        }


        party2Panel.gameObject.SetActive(JoinPartyButton.activeSelf);
    }
    public void ShowJoinButton()
    {
        JoinPartyButton.SetActive(true);
       UpdatePanel();
    }
    public void HideJoinButton()
    {
        JoinPartyButton.SetActive(false);
        UpdatePanel();
    }
    public void ShowSplitButton()
    {
        SplitPartyButton.SetActive(true);
        UpdatePanel();
    }
    public void HideSplitButton()
    {
        SplitPartyButton.SetActive(false);
        UpdatePanel();
    }
    public void SwapClicked()
    {
        Debug.Log("Swap clicked!");
        //WM_PartyActionSystem.OnSplitClicked?.Invoke();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}