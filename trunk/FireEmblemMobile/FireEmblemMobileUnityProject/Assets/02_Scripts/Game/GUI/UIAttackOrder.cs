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

        public void Show(List<AttackData> battlePreviewAttacksData, bool attackerIsPlayer)
        {
            parent.DeleteAllChildren();
            GameObject.Instantiate(attackLabelPrefab, parent);
           // Debug.Log("TODO Consecutive Attacks(Brave Effect)");
          //  Debug.Log("Attacks in Order: ");
            foreach (var attack in battlePreviewAttacksData)
            {
               // Debug.Log(attack.attacker);
                var prefab = allyAttackPrefab;
                if(attackerIsPlayer)
                    prefab = attack.attacker ? allyAttackPrefab : enemyAttackPrefab;
                else
                    prefab = attack.attacker ? enemyAttackPrefab: allyAttackPrefab;
                var go = GameObject.Instantiate(prefab, parent);
                go.GetComponentInChildren<TextMeshProUGUI>().SetText(""+attack.Dmg);
            }
        }
       
    }
}
