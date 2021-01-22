using System;
using Game.Graphics;
using Game.Grid;
using UnityEngine;

namespace Game.GameResources
{
    [Serializable]
    public class GridResources
    {
        public GridData gridData;
        public Material gridMaterial;
        public TileType standardTileType;
        public Sprite standardSprite;
        public MoveArrowVisual moveArrowVisual;
    }
}