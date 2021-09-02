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
        name.SetText(tile.TileType.name.ToString());
        if (tile.TileType.avoidBonus != 0)
        {
            boni1.gameObject.SetActive(true);
            boni1.SetText("+" + tile.TileType.avoidBonus + "% Avo");
        }
        else
        {
            boni1.gameObject.SetActive(false);
        }

        if (tile.TileType.defenseBonus != 0)
        {
            boni2.gameObject.SetActive(true);
            boni2.SetText("+" + tile.TileType.defenseBonus + " Def");
        }
        else
        {
            boni2.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
