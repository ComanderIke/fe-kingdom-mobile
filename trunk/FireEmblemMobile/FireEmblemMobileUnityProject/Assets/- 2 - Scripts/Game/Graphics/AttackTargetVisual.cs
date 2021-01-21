using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Graphics
{
    [Serializable]
    public class AttackTargetVisual
    {
        private List<GameObject> attackableEnemyEffects;
        private List<GameObject> attackableFieldEffects;
        [SerializeField]
        private GameObject attackableEnemyPrefab;
        [SerializeField]
        private GameObject attackIconPrefab;

        [SerializeField] private Transform parenTransform;

        public AttackTargetVisual()
        {
            attackableEnemyEffects = new List<GameObject>();
            attackableFieldEffects = new List<GameObject>();
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

        public void ShowAttackableField(int x, int y)
        {
            //Debug.Log("Show Attackable Field: "+x+" " +y );
            if (attackableFieldEffects.Any(gameObj =>
                !gameObj.activeSelf || (gameObj.transform.localPosition.x - 0.5f == x &&
                                        gameObj.transform.localPosition.y - 0.5f == y)))
            {
                GameObject go2 = attackableFieldEffects.Find(gameObj =>
                    gameObj.transform.localPosition.x - 0.5f == x && gameObj.transform.localPosition.y - 0.5f == y ||
                    !gameObj.activeSelf);
                go2.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go2.transform.localPosition.z);
                go2.SetActive(true);
                return;
            }

            var go =GameObject.Instantiate(attackIconPrefab,
                parenTransform);
            go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, go.transform.localPosition.z);
            attackableFieldEffects.Add(go);
        }

        public void HideAttackableField()
        {
            foreach (var go in attackableFieldEffects)
            {
                go.SetActive(false);
            }

            //attackableFieldEffects.Clear();
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