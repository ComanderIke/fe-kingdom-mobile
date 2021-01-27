using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/Visual", fileName = "UnitVisual")]
    public class CharacterSpriteSet:ScriptableObject
    {
        public Sprite FaceSprite;
        public Sprite MapSprite;
    }
}