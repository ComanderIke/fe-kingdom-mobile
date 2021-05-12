using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;
using Utility;

[RequireComponent(typeof(PartyRenderer))]
[RequireComponent(typeof(SpriteRenderer))]
public class PartyController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool selected;
    private SpriteRenderer spriteRenderer;
    public Party party;
    [SerializeField] private WorldMapPosition currentLocation;
    public IWorldMapUnitInputReceiver inputReceiver { get; set; }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        party.location = currentLocation;
        currentLocation.Actor = party;
    }
    void OnMouseDown()
    {
        inputReceiver.PartyClicked(party);

    }
}