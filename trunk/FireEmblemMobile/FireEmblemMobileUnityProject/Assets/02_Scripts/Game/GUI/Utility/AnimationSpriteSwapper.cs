using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    [ExecuteInEditMode]
    public class AnimationSpriteSwapper : MonoBehaviour
    {
        [SerializeField] CharacterSpriteSet spriteSet;
        [SerializeField]private SpriteRenderer spriteRenderer;
        [SerializeField]private SpriteRenderer weaponSpriteRenderer;

     
        public void Init(CharacterSpriteSet spriteSet)
        {
            this.spriteSet = spriteSet;
      
        }
        
        public void OnWalkAnimation(int frame)
        {
            spriteRenderer.sprite=spriteSet.WalkSprites[frame];
            if(weaponSpriteRenderer!=null&&spriteSet.WalkSpritesWeapon!=null&& spriteSet.WalkSpritesWeapon.Length>frame)
                weaponSpriteRenderer.sprite = spriteSet.WalkSpritesWeapon[frame];
        }
        public void OnIdleAnimation(int frame)
        {
            spriteRenderer.sprite=spriteSet.IdleSprites[frame];
            if(weaponSpriteRenderer!=null&&spriteSet.IdleSpritesWeapon!=null&& spriteSet.IdleSpritesWeapon.Length>frame)
                weaponSpriteRenderer.sprite = spriteSet.IdleSpritesWeapon[frame];
        }
        public void OnAttackAnimation()
        {
         
            spriteRenderer.sprite=spriteSet.AttackSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.AttackSpriteWeapon;
        }
        public void OnDodgeAnimation()
        {
            spriteRenderer.sprite=spriteSet.DodgeSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.DodgeSpriteWeapon;
        }
        public void OnTakeDamageAnimation()
        {
            spriteRenderer.sprite=spriteSet.TakeDamageSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.TakeDamageWeapon;
        }
        public void OnCriticalAnimation()
        {
            spriteRenderer.sprite=spriteSet.CriticalSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.CriticalSpriteWeapon;
        }
    }
}
