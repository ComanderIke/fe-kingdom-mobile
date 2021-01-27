using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.Graphics
{
    [CreateAssetMenu(menuName = "Unit/UnitEffectVisual", fileName = "unitEffectVisual")]
    public class UnitEffectVisual :  IUnitEffectVisual
    {
        private GameObject attackableEnemyEffect;
        [SerializeField]
        private GameObject attackableEnemyPrefab;
        

 
        public override void ShowAttackable(IGridActor actor)
        {
            if (attackableEnemyEffect == null)
            {

                attackableEnemyEffect = GameObject.Instantiate(attackableEnemyPrefab,
                    actor.GameTransformManager.Transform);
                attackableEnemyEffect.transform.localPosition = Vector3.zero;
            }
            else
            {
                attackableEnemyEffect.transform.localPosition = Vector3.zero;
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

    }
}