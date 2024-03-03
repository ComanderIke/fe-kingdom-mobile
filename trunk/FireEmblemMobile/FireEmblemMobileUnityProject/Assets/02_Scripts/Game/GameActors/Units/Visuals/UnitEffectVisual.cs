using System;
using Game.GameActors.Units.Interfaces;
using Game.Graphics.Interfaces;
using UnityEngine;

namespace Game.GameActors.Units.Visuals
{
    [CreateAssetMenu(menuName = "Unit/UnitEffectVisual", fileName = "unitEffectVisual")]
    [Serializable]
    public class UnitEffectVisual :  IUnitEffectVisual
    {
        private GameObject attackableEnemyEffect;
        private GameObject noStaminaVfx;
        [SerializeField]
        private GameObject attackableEnemyPrefab;
        [SerializeField]
        private GameObject noStaminaVfxPrefab;

 
        public override void ShowAttackable(IGridActor actor)
        {
            if (actor.GameTransformManager.GameObject == null)
                return;
            if (attackableEnemyEffect == null)
            {

                attackableEnemyEffect = GameObject.Instantiate(attackableEnemyPrefab,
                    actor.GameTransformManager.Transform);
                attackableEnemyEffect.transform.position = actor.GameTransformManager.GetCenterPosition();
            }
            else
            {
                attackableEnemyEffect.transform.position = actor.GameTransformManager.GetCenterPosition();
                attackableEnemyEffect.SetActive(true);
            }
        }
        public override void HideAttackable()
        {
            if (attackableEnemyEffect != null)
            {
                attackableEnemyEffect.SetActive(false);
            }
        }
        public override void HideNoStamina()
        {
            // if (noStaminaVfx != null)
            // {
            //     noStaminaVfx.SetActive(false);
            // }
        }

        public override void ShowNoStamina(IGridActor actor)
        {
            // if (actor.GameTransformManager.GameObject == null)
            //     return;
            // if (noStaminaVfx == null)
            // {
            //
            //     noStaminaVfx = GameObject.Instantiate(noStaminaVfxPrefab,
            //         actor.GameTransformManager.Transform);
            //     noStaminaVfx.transform.localPosition = Vector3.zero;
            // }
            // else
            // {
            //     noStaminaVfx.transform.localPosition = Vector3.zero;
            //     noStaminaVfx.SetActive(true);
            // }
        }
    }
}