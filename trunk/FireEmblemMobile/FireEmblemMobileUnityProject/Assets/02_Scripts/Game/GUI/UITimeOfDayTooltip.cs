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
        //[SerializeField] private TextMeshProUGUI enemyCritical;
        [SerializeField] private TextMeshProUGUI other;
        public void Show(int hours, TimeOfDayBonuses bonus)
        {
            time.text = (hours*100).ToString("00:00");
            curseResistance.gameObject.SetActive(bonus.curseResistance!=0);
            curseResistance.text = (bonus.curseResistance > 0 ? "+" + bonus.curseResistance : ""+bonus.curseResistance)+ "% Curse Resistance";
            enemyLevels.gameObject.SetActive(bonus.enemylevelsPerArea!=0);
            enemyLevels.text = "+"+bonus.enemylevelsPerArea+ " Enemy Lvl";
            other.gameObject.SetActive(!String.IsNullOrEmpty(bonus.other));
            other.text = bonus.other;
        }
    }
}
