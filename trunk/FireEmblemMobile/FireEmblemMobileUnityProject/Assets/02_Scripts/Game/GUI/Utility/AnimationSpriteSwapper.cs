using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class AnimationSpriteSwapper : MonoBehaviour
    {
        [SerializeField] CharacterSpriteSet spriteSet;
        [SerializeField]private SpriteRenderer spriteRenderer;

        public void Init(CharacterSpriteSet spriteSet)
        {
            this.spriteSet = spriteSet;
        }
        
        public void OnWalkAnimation(int frame)
        {
            spriteRenderer.sprite=spriteSet.WalkSprites[frame];
        }
        public void OnIdleAnimation(int frame)
        {
            spriteRenderer.sprite=spriteSet.IdleSprites[frame];
        }
        public void OnAttackAnimation()
        {
            spriteRenderer.sprite=spriteSet.AttackSprite;
        }
        public void OnDodgeAnimation()
        {
            spriteRenderer.sprite=spriteSet.DodgeSprite;
        }
        public void OnTakeDamageAnimation()
        {
            spriteRenderer.sprite=spriteSet.TakeDamageSprite;
        }
        public void OnCriticalAnimation()
        {
            spriteRenderer.sprite=spriteSet.CriticalSprite;
        }
    }
}
