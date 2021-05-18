using Game.AI;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameActors
{
    [ExecuteInEditMode]
    public class UnitSpawner : MonoBehaviour
    {

        public Unit unit;
        public int FactionId;
        public WeightSet AIWeightSet;
        public int id;
        public int X => (int) transform.localPosition.x;
        public int Y => (int)transform.localPosition.y;

        void Update()
        {
            transform.localPosition = new Vector3((int) transform.localPosition.x, (int) transform.localPosition.y,
                (int) transform.localPosition.z);
        }
    }
}
