using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Graphics
{
    public class UnitEffectVisual
    {
        private List<GameObject> attackableEnemyEffects;
        [SerializeField]
        private GameObject attackableEnemyPrefab;
        
        [SerializeField] private Transform parenTransform;

        public UnitEffectVisual()
        {
            attackableEnemyEffects = new List<GameObject>();
        }
        public void ShowAttackableEnemy(int x, int y)
        {
            if (attackableEnemyEffects.Any(gameObj =>
                (int) gameObj.transform.localPosition.x == x && (int) gameObj.transform.localPosition.y == y))
            {
                attackableEnemyEffects.Find(gameObj =>
                        (int) gameObj.transform.localPosition.x == x && (int) gameObj.transform.localPosition.y == y)
                    .SetActive(true);
                return;
            }

            var go = GameObject.Instantiate(attackableEnemyPrefab,
                parenTransform);
            go.transform.localPosition = new Vector3(x, y, go.transform.localPosition.z);
            attackableEnemyEffects.Add(go);
        }
        public void HideAttackableEnemy()
        {
            foreach (var go in attackableEnemyEffects)
            {
                go.SetActive(false);
            }

            //attackableEnemyEffects.Clear();
        }

    }
}