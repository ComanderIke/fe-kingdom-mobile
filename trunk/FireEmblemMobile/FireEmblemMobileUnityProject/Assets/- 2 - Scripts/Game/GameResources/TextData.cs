using UnityEngine;

namespace Game.GameResources
{
    [CreateAssetMenu(menuName = "GameData/TextData", fileName = "TextData")]
    public class TextData : ScriptableObject
    {
        public string[] Texts;
    }
}