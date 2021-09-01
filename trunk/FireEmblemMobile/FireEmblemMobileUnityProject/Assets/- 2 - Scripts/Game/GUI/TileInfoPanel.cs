using System.Collections;
using System.Collections.Generic;
using Game.Grid;
using TMPro;
using UnityEngine;

public class TileInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI name;

    public TextMeshProUGUI boni1;

    public TextMeshProUGUI boni2;

    public void Show(Tile tile)
    {
        name.SetText(tile.TileType.TerrainType.ToString());
        boni1.SetText("+"+tile.TileType.avoidBonus+"% Avo");
        boni2.SetText("+"+tile.TileType.defenseBonus+" Def");
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
