using Game.GameActors.Units;
using UnityEngine;

namespace Game.Grid
{
    public class Tile
    {
        public GameObject GameObject;
        public Unit Unit;
        public int MovementCost = 1;
        public int X;
        public int Y;
        public bool IsActive = false;
        public bool IsAccessible = true;
        public bool IsAttackable = false;
        public SpriteRenderer spriteRenderer;
        public TileType TileType;

        public Tile(int i, int j, TileType tileType, GameObject gameObject)
        {
            this.GameObject = gameObject;
            X = i;
            Y = j;
            TileType = tileType;
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
    }
}