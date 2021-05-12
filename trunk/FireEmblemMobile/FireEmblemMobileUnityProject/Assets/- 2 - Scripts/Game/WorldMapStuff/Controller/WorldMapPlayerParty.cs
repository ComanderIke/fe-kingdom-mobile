using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;
using Utility;

public class WorldMapPlayerParty : MonoBehaviour
{
    // Start is called before the first frame update
    private bool selected;
    private SpriteRenderer spriteRenderer;
    public Party party;
    [SerializeField] private WorldMapPosition currentLocation;
    public IWorldMapUnitInputReceiver inputReceiver { get; set; }
    void Start()
    {
        Debug.Log("WTF: "+party.Faction.Id);
        spriteRenderer = GetComponent<SpriteRenderer>();
        party.location = currentLocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        inputReceiver.PartyClicked(party);

        // selected = !selected;
        //
        //
        // UpdateGraphics();

    }

    void UpdateGraphics()
    {
        if (selected)
        {
            currentLocation.DrawInteractableConnections();
            spriteRenderer.material.SetColor("_OutLineColor", Color.yellow);
        }
        else
        {
            currentLocation.HideInteractableConnections();
            spriteRenderer.material.SetColor("_OutLineColor", Color.blue);
        }
    }
}