using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.UI;
using UnityEngine;

public class PartyScreenUIController : IPartyScreenUI
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Show(Party party)
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
