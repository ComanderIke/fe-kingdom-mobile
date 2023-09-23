using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.Mechanics;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class UIAttackOrder : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private GameObject attackLabelPrefab;
        [SerializeField] private GameObject allyAttackPrefab;
        [SerializeField] private GameObject enemyAttackPrefab;

        public void Show(List<AttackData> battlePreviewAttacksData)
        {
            parent.DeleteAllChildren();
            GameObject.Instantiate(attackLabelPrefab, parent);
            Debug.Log("TODO Consecutive Attacks(Brave Effect)");
            foreach (var attack in battlePreviewAttacksData)
            {
                var prefab = attack.attacker ? allyAttackPrefab : enemyAttackPrefab;
                var go = GameObject.Instantiate(prefab, parent);
                go.GetComponentInChildren<TextMeshProUGUI>().SetText(""+attack.Dmg);
            }
        }
       
    }
}
