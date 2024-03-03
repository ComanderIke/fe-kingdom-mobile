using UnityEngine;

namespace Game.GameActors.Units.Visuals
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/BossSpriteSet", fileName = "BossspriteSet")]
    public class BossSpriteSet:CharacterSpriteSet
    {
        
        [field:SerializeField]public Sprite[] WalkSpritesState2VFX { get; set; }
        [field:SerializeField]public Sprite[] WalkSpritesWeapon2 { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesState2VFX { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesWeapon2 { get; set; }
        [field:SerializeField]public Sprite AttackSpriteState2VFX { get; set; }
        [field:SerializeField]public Sprite AttackSpriteWeapon2 { get; set; }
        [field:SerializeField]public Sprite CriticalSpriteState2VFX { get; set; }
        [field:SerializeField]public Sprite CriticalSpriteWeapon2 { get; set; }
        [field:SerializeField]public Sprite DodgeSpriteState2VFX { get; set; }
        [field:SerializeField]public Sprite  DodgeSpriteWeapon2 { get; set; }
        [field:SerializeField]public Sprite TakeDamageSpriteState2VFX { get; set; }
        [field:SerializeField]public Sprite TakeDamageWeapon2 { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesAlternate { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesAlternateWeapon { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesAlternateWeapon2 { get; set; }
        [field:SerializeField]public Sprite[] IdleSpritesAlternateVFX { get; set; }
        [field:SerializeField]public Sprite[] RunningSpritesAlternate { get; set; }
        [field:SerializeField]public Sprite[] RunningSpritesAlternateWeapon { get; set; }
        [field:SerializeField]public Sprite[] RunningSpritesAlternateWeapon2 { get; set; }
        [field:SerializeField]public Sprite[] RunningSpritesAlternateVFX { get; set; }
   
    }
}