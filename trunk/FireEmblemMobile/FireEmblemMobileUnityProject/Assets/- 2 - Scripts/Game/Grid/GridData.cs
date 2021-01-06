using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Grid
{
    [CreateAssetMenu(menuName = "Map/GridData", fileName = "GridData")]
    public class GridData :ScriptableObject
    {
        public int width;
        public int height;

    }
}