using UnityEngine;

namespace Game.DataAndReferences.Data
{
    [CreateAssetMenu(menuName = "GameData/TextData", fileName = "TextData")]
    public class TextData : ScriptableObject
    {
        public string[] Texts;
    }
}