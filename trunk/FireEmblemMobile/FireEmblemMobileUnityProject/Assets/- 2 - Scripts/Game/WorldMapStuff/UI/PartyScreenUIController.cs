using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using UnityEngine;

public class PartyScreenUIController : IPartyScreenUI
{
    // Start is called before the first frame update
    [SerializeField]
    private Canvas canvas;

    public UIPartyOverViewController partyOverViewController;

    public override void Show(Party party)
    {
        canvas.enabled=true;
        partyOverViewController.Show(party);
    }

    public override void Hide()
    {
        canvas.enabled=false;
    }
}
