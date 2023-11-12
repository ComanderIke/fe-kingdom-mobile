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
        [SerializeField] private GameObject allyHealPrefab;
        [SerializeField] private GameObject enemyAttackPrefab;
       

        public void Show(List<AttackData> battlePreviewAttacksData,string attackLabel, bool attackerIsPlayer)
        {
            parent.DeleteAllChildren();
            var label= GameObject.Instantiate(attackLabelPrefab, parent);
            label.GetComponentInChildren<TextMeshProUGUI>().text = attackLabel;
           // Debug.Log("TODO Consecutive Attacks(Brave Effect)");
          //  Debug.Log("Attacks in Order: ");
         // attackLabelText.text = attackLabel;
            foreach (var attack in battlePreviewAttacksData)
            {
               // Debug.Log(attack.attacker);
                var prefab = allyAttackPrefab;
                if (attackerIsPlayer)
                {
                    if (attack.Heal > 0)
                    {
                        prefab = attack.attacker ? allyHealPrefab : enemyAttackPrefab;
                    }
                    else
                    {
                        prefab = attack.attacker ? allyAttackPrefab : enemyAttackPrefab;
                    }
                }
                else
                    prefab = attack.attacker ? enemyAttackPrefab: allyAttackPrefab;
                var go = GameObject.Instantiate(prefab, parent);
                go.GetComponentInChildren<TextMeshProUGUI>().SetText(""+(attack.Heal>0?attack.Heal:attack.Dmg));
            }
        }
       
    }
}
