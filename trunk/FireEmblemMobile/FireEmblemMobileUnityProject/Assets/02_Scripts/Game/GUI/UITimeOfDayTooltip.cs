using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class UITimeOfDayTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI curseResistance;
        [SerializeField] private TextMeshProUGUI enemyLevels;
        [SerializeField] private TextMeshProUGUI enemyCritical;
        [SerializeField] private TextMeshProUGUI allyCritical;
        [SerializeField] private TextMeshProUGUI critical;
        [SerializeField] private TextMeshProUGUI goldModifier;
        [SerializeField] private TextMeshProUGUI healperNode;
        [SerializeField] private TextMeshProUGUI accuracy;
        [SerializeField] private TextMeshProUGUI other;
        public void Show(int hours, TimeOfDayBonuses bonus)
        {
            time.text = (hours*100).ToString("00:00");
            curseResistance.gameObject.SetActive(bonus.curseResistance!=0);
            curseResistance.text = (bonus.curseResistance > 0 ? "+" + bonus.curseResistance : ""+bonus.curseResistance)+ "% Curse Resistance";
            goldModifier.text = "+" + ((bonus.goldModifier * 100) - 100) + "% Gold received";
            healperNode.text = "+" + bonus.healPerNode + "Hp per node";
            accuracy.text = bonus.accuracy + "% Hit Rate (Heroes)";
            critical.text = "+" + bonus.critical + "% Critical Rate (All)";
            enemyCritical.text = bonus.enemyCritical + "% Critical Rate (Enemies)";
            allyCritical.text = "+"+bonus.allyCritical + "% Critical Rate (Heroes)";
            allyCritical.gameObject.SetActive(bonus.allyCritical!=0);
            enemyCritical.gameObject.SetActive(bonus.enemyCritical!=0);
            critical.gameObject.SetActive(bonus.critical!=0);
            healperNode.gameObject.SetActive(bonus.healPerNode!=0);
            accuracy.gameObject.SetActive(bonus.accuracy!=0);
            goldModifier.gameObject.SetActive(bonus.goldModifier!=0);
            curseResistance.gameObject.SetActive(bonus.curseResistance!=0);
            // enemyLevels.gameObject.SetActive(bonus.enemylevelsPerArea!=0);
            // enemyLevels.text = "+"+bonus.enemylevelsPerArea+ " Enemy Lvl";
            // other.gameObject.SetActive(!String.IsNullOrEmpty(bonus.other));
            // other.text = bonus.other;
        }
    }
}
