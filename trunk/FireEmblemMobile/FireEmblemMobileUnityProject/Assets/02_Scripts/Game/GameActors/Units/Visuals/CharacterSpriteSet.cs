using Game.Graphics;
using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/Visual", fileName = "UnitVisual")]
    public class CharacterSpriteSet:ScriptableObject
    {
        public Sprite FaceSprite;
        public Sprite MapSprite;
        public GameObject battleAnimatedSprite;
        public GameObject animatedSprite;
        [field:SerializeField]public Sprite[] WalkSprites { get; set; }
        [field:SerializeField]public Sprite[] IdleSprites { get; set; }
        [field:SerializeField]public Sprite AttackSprite { get; set; }
        [field:SerializeField]public Sprite CriticalSprite { get; set; }
        [field:SerializeField]public Sprite DodgeSprite { get; set; }
        [field:SerializeField]public Sprite TakeDamageSprite { get; set; }
    }
}