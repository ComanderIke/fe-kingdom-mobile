using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    public class PreferedMovementPath : ScriptableObject
    {
        public List<Vector2> path;
    }
}
