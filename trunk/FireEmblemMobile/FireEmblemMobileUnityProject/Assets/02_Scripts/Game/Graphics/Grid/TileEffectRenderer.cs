using System.Collections.Generic;
using Game.Grid.Tiles;
using UnityEngine;

namespace Game.Graphics.Grid
{
    public class TileEffectRenderer : MonoBehaviour, ITileEffectVisualRenderer
    {
        [SerializeField]
        private TileEffectVisual attackable;
        [SerializeField]
        private TileEffectVisual swapable;


        private Dictionary<Tile, TileEffectVisual> attackableVFXs;
        private Dictionary<Tile, TileEffectVisual> swapAbleVFXs;

        // Start is called before the first frame update

        void Start()
        {
            attackableVFXs = new Dictionary<Tile, TileEffectVisual>();
            swapAbleVFXs = new Dictionary<Tile, TileEffectVisual>();
        }
        public void ShowSwapable(Tile tile)
        {

            if (swapAbleVFXs.ContainsKey(tile))
            {
                swapAbleVFXs[tile].Show(tile);
            }
            else
            {
                swapAbleVFXs.Add(tile, ScriptableObject.Instantiate(swapable));
                swapAbleVFXs[tile].Show(tile);
            }
        
        }

        public void ShowAttackable(Tile tile)
        {

            if (attackableVFXs.ContainsKey(tile))
            {
                attackableVFXs[tile].Show(tile);
            }
            else
            {
                attackableVFXs.Add(tile, ScriptableObject.Instantiate(attackable));
                attackableVFXs[tile].Show(tile);
            }
        }

        public void Hide(Tile tile)
        {
            if (swapAbleVFXs.ContainsKey(tile))
            {
                swapAbleVFXs[tile].Hide();
            }

            if (attackableVFXs.ContainsKey(tile))
            {
                attackableVFXs[tile].Hide();
            }
        }
    }
}


