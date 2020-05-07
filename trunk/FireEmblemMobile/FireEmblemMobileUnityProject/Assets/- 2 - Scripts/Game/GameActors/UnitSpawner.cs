using Assets.GameActors.Units;
using UnityEngine;

namespace Assets.GameActors
{
    public class UnitSpawner : MonoBehaviour
    {

        public Unit Unit;
        public int FactionId;
        public int X => (int) transform.localPosition.x;
        public int Y => (int)transform.localPosition.y;
    }
}
