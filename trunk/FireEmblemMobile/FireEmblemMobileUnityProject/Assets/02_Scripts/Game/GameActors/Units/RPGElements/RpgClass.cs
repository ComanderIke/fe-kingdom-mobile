using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Unit/Class", fileName = "Class1")]
    public class RpgClass : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
    }
}