using TMPro;
using UnityEngine;

namespace Game.GUI.Tile
{
    public class TileInfoPanel : MonoBehaviour
    {
        public new TextMeshProUGUI name;

        public TextMeshProUGUI boni1;

        public TextMeshProUGUI boni2;
        public TextMeshProUGUI boni3;

        public TextMeshProUGUI status;
        public void Show(Grid.Tiles.Tile tile)
        {
            name.SetText(tile.TileData.name.ToString());
            if (tile.TileData.avoBonus != 0)
            {
                boni1.gameObject.SetActive(true);
                boni1.SetText( tile.TileData.avoBonus + "% Avo");
            }
            else
            {
                boni1.gameObject.SetActive(false);
            }
            if (tile.TileData.speedMalus != 0)
            {
                boni2.gameObject.SetActive(true);
                boni2.SetText( tile.TileData.speedMalus + " Spd");
            }
            else
            {
                boni2.gameObject.SetActive(false);
            }
            if (tile.TileData.defenseBonus != 0)
            {
                boni3.gameObject.SetActive(true);
                boni3.SetText( tile.TileData.defenseBonus + " Def");
            }
            else
            {
                boni3.gameObject.SetActive(false);
            }
            if (!tile.TileData.walkable)
            {
                status.gameObject.SetActive(true);
                status.SetText( "Impassable");
            }
            else
            {
                status.gameObject.SetActive(false);
            }
            if(tile.TileData.avoBonus != 0||tile.TileData.speedMalus != 0||tile.TileData.defenseBonus != 0||!tile.TileData.walkable)
                gameObject.SetActive(true);
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void Hide()
        {
            if (this != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
