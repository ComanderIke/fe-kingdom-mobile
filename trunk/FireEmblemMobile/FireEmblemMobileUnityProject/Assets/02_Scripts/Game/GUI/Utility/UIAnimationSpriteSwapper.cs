using Game.GameActors.Units.Visuals;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Utility
{
    [ExecuteInEditMode]
    public class UIAnimationSpriteSwapper : MonoBehaviour
    {
        [SerializeField] CharacterSpriteSet spriteSet;
        [SerializeField]private Image image;
        [SerializeField]private Image weaponImage;

     
        public void Init(CharacterSpriteSet spriteSet)
        {
            this.spriteSet = spriteSet;
            weaponImage.gameObject.SetActive(spriteSet.AttackSpriteWeapon!=null);
        }
        
        public void OnWalkAnimation(int frame)
        {
            image.sprite=spriteSet.WalkSprites[frame];
            if(weaponImage!=null&&spriteSet.WalkSpritesWeapon!=null&& spriteSet.WalkSpritesWeapon.Length>frame)
                weaponImage.sprite = spriteSet.WalkSpritesWeapon[frame];
        }
        public void OnIdleAnimation(int frame)
        {
            image.sprite=spriteSet.IdleSprites[frame];
            if (weaponImage != null && spriteSet.IdleSpritesWeapon != null &&
                spriteSet.IdleSpritesWeapon.Length > frame)
            {
                
                weaponImage.sprite = spriteSet.IdleSpritesWeapon[frame];
            }
        }
        public void OnAttackAnimation()
        {
         
            image.sprite=spriteSet.AttackSprite;
            if(weaponImage!=null)
                weaponImage.sprite = spriteSet.AttackSpriteWeapon;
        }
        public void OnDodgeAnimation()
        {
            image.sprite=spriteSet.DodgeSprite;
            if(weaponImage!=null)
                weaponImage.sprite = spriteSet.DodgeSpriteWeapon;
        }
        public void OnTakeDamageAnimation()
        {
            image.sprite=spriteSet.TakeDamageSprite;
            if(weaponImage!=null)
                weaponImage.sprite = spriteSet.TakeDamageWeapon;
        }
        public void OnCriticalAnimation()
        {
            image.sprite=spriteSet.CriticalSprite;
            if(weaponImage!=null)
                weaponImage.sprite = spriteSet.CriticalSpriteWeapon;
        }
    }
}