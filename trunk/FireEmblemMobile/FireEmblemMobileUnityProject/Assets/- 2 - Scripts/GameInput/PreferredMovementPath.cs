using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameInput
{
    [CreateAssetMenu]
    public class PreferredMovementPath : ScriptableObject
    {
        public List<Vector2> Path;
    }
}
