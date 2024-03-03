using UnityEngine;

namespace Game.GameActors.Units.Visuals
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/CharacterSpriteset", fileName = "SpriteSet")]
    public class CharacterSpriteSet:ScriptableObject
    {
        public Sprite FaceSprite;
        public DialogSpriteSet DialogSpriteSet;
        
        public Sprite MapSprite;
        public GameObject battleAnimatedSprite;
        public GameObject animatedSprite;
        [field:SerializeField]public Sprite[] WalkSprites { get; set; }
        [field:SerializeField]public Sprite[] WalkSpritesWeapon { get; set; }
        [field:SerializeField]public Sprite[] IdleSprites { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesWeapon { get; set; }
        [field:SerializeField]public Sprite AttackSprite { get; set; }
        [field:SerializeField]public Sprite AttackSpriteWeapon { get; set; }
        [field:SerializeField]public Sprite CriticalSprite { get; set; }
        [field:SerializeField]public Sprite CriticalSpriteWeapon { get; set; }
        [field:SerializeField]public Sprite DodgeSprite { get; set; }
        [field:SerializeField]public Sprite  DodgeSpriteWeapon { get; set; }
        [field:SerializeField]public Sprite TakeDamageSprite { get; set; }
        [field:SerializeField]public Sprite TakeDamageWeapon { get; set; }
        [field:SerializeField]public Sprite BlockSprite { get; set; }
        [field:SerializeField]public Sprite BlockSpriteWeapon { get; set; }
        
    }
}