using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.Grid
{
    [System.Serializable]
    public class Tile
    {
        public GameObject GameObject;
        public Unit Unit;
        public int MovementCost = 1;
        public int X;
        public int Y;
        public bool IsActive = false;
        public bool IsAccessible = true;

        public Tile(int i, int j, GameObject gameObject)
        {
            this.GameObject = gameObject;
            X = i;
            Y = j;
        }
    }
}