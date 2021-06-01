using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using UnityEngine;

[ExecuteInEditMode]
public class PartySpawn : MonoBehaviour
{
    public Party party;

    private SpriteRenderer spriteRenderer;
    public WorldMapPosition location;
    // Start is called before the first frame update
    void OnEnable()
    {
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = party.members[0].visuals.CharacterSpriteSet.MapSprite;
        transform.position = location.transform.position;
    }
}
