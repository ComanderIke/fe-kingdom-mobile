using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/DialogSpriteSet", fileName = "DialogSpriteSet")]
    public class DialogSpriteSet : ScriptableObject
    {
        public Sprite FaceSprite;
        [field:SerializeField]public Sprite MouthClosed { get; set; }
        [field:SerializeField]public Sprite MouthOpen { get; set; }
        [field:SerializeField] public Sprite EyesClosed { get; set; }
        [field:SerializeField] public Sprite EyesOpen { get; set; }
    }
}