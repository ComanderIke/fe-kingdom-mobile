using Game.GameActors.Units.Visuals;
using UnityEngine;

namespace Game.GUI.Utility
{
    [ExecuteInEditMode]
    public class AnimationSpriteSwapper : MonoBehaviour
    {
        [SerializeField] CharacterSpriteSet spriteSet;
        [SerializeField]private SpriteRenderer spriteRenderer;
        [SerializeField]private SpriteRenderer weaponSpriteRenderer;
        [SerializeField] private SpriteRenderer specialStateSpriteRenderer;
        private bool specialState = false;

        public void SetSpecialState(bool state)
        {
            this.specialState = state;
        }

     
        public void Init(CharacterSpriteSet spriteSet)
        {
            this.spriteSet = spriteSet;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.flipX = spriteRenderer.flipX;
            if(specialStateSpriteRenderer!=null)
                specialStateSpriteRenderer.flipX = spriteRenderer.flipX;

        }
        public void OnChargeAnimation(int frame)
        {
            if (spriteSet is BossSpriteSet bossSpriteSet)
            {
                spriteRenderer.sprite = bossSpriteSet.RunningSpritesAlternate[frame];
                if (weaponSpriteRenderer != null && bossSpriteSet.RunningSpritesAlternateWeapon != null &&
                    bossSpriteSet.RunningSpritesAlternateWeapon.Length > frame)
                    weaponSpriteRenderer.sprite = bossSpriteSet.RunningSpritesAlternateWeapon[frame];
                if (specialStateSpriteRenderer != null && bossSpriteSet.RunningSpritesAlternateVFX != null &&
                    bossSpriteSet.RunningSpritesAlternateVFX.Length > frame)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.RunningSpritesAlternateVFX[frame];
            }
        }
        public void OnWalkAnimation(int frame)
        {
            spriteRenderer.sprite=spriteSet.WalkSprites[frame];
            if(weaponSpriteRenderer!=null&&spriteSet.WalkSpritesWeapon!=null&& spriteSet.WalkSpritesWeapon.Length>frame)
                weaponSpriteRenderer.sprite = spriteSet.WalkSpritesWeapon[frame];
            if (spriteSet is BossSpriteSet bossSpriteSet&& specialState)
            {
                if (specialStateSpriteRenderer != null && bossSpriteSet.WalkSpritesState2VFX != null &&
                    bossSpriteSet.WalkSpritesState2VFX.Length > frame)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.WalkSpritesState2VFX[frame];
            }
        }
        public void OnIdleAnimation(int frame)
        {
            spriteRenderer.sprite=spriteSet.IdleSprites[frame];
            if(weaponSpriteRenderer!=null&&spriteSet.IdleSpritesWeapon!=null&& spriteSet.IdleSpritesWeapon.Length>frame)
                weaponSpriteRenderer.sprite = spriteSet.IdleSpritesWeapon[frame];
            if (spriteSet is BossSpriteSet bossSpriteSet&& specialState)
            {
                if (specialStateSpriteRenderer != null && bossSpriteSet.IdleSpritesState2VFX != null &&
                    bossSpriteSet.IdleSpritesState2VFX.Length > frame)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.IdleSpritesState2VFX[frame];
            }
        }
        public void OnIdleAlternateAnimation(int frame)
        {
            if (spriteSet is BossSpriteSet bossSpriteSet)
            {
                spriteRenderer.sprite = bossSpriteSet.IdleSpritesAlternate[frame];
                if (weaponSpriteRenderer != null && bossSpriteSet.IdleSpritesAlternateWeapon != null &&
                    bossSpriteSet.IdleSpritesAlternateWeapon.Length > frame)
                    weaponSpriteRenderer.sprite = bossSpriteSet.IdleSpritesAlternateWeapon[frame];
                if (specialStateSpriteRenderer != null && bossSpriteSet.IdleSpritesAlternateVFX != null &&
                    bossSpriteSet.IdleSpritesAlternateVFX.Length > frame)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.IdleSpritesAlternateVFX[frame];
            }
        }
        public void OnAttackAnimation()
        {
         
            spriteRenderer.sprite=spriteSet.AttackSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.AttackSpriteWeapon;
            if (spriteSet is BossSpriteSet bossSpriteSet&& specialState)
            {
                if (specialStateSpriteRenderer != null && bossSpriteSet.AttackSpriteState2VFX != null)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.AttackSpriteState2VFX;
            }
        }
        public void OnDodgeAnimation()
        {
            spriteRenderer.sprite=spriteSet.DodgeSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.DodgeSpriteWeapon;
            if (spriteSet is BossSpriteSet bossSpriteSet&& specialState)
            {
                if (specialStateSpriteRenderer != null && bossSpriteSet.DodgeSpriteState2VFX != null)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.DodgeSpriteState2VFX;
            }
        }
        public void OnTakeDamageAnimation()
        {
            spriteRenderer.sprite=spriteSet.TakeDamageSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.TakeDamageWeapon;
            if (spriteSet is BossSpriteSet bossSpriteSet&& specialState)
            {
                if (specialStateSpriteRenderer != null && bossSpriteSet.TakeDamageSpriteState2VFX != null)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.TakeDamageSpriteState2VFX;
            }
        }
        public void OnCriticalAnimation()
        {
            spriteRenderer.sprite=spriteSet.CriticalSprite;
            if(weaponSpriteRenderer!=null)
                weaponSpriteRenderer.sprite = spriteSet.CriticalSpriteWeapon;
            if (spriteSet is BossSpriteSet bossSpriteSet&& specialState)
            {
                if (specialStateSpriteRenderer != null && bossSpriteSet.CriticalSpriteState2VFX != null)
                    specialStateSpriteRenderer.sprite = bossSpriteSet.CriticalSpriteState2VFX;
            }
        }
    }
}
