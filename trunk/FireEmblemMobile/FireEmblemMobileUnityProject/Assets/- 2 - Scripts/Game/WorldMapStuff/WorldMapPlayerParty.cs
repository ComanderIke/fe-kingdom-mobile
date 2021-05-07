using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class WorldMapPlayerParty : MonoBehaviour
{
    // Start is called before the first frame update
    private bool selected;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private WorldMapPosition currentLocation;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Clicked on Party");

        selected = !selected;

        
        UpdateGraphics();

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
