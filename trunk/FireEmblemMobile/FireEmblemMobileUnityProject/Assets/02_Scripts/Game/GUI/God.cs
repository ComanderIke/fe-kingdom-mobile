using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/God", fileName = "God")]
    public class God :ScriptableObject
    {
        public string Name;
        public Sprite Face;
        public Sprite Body;
    }
}