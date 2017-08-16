using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameStates
{
    public class AttackPreview :MonoBehaviour
    {
        [HideInInspector]
        public bool visible = false;
        [HideInInspector]
        public Character attacker;
        [HideInInspector]
        public Character defender;
        public GameObject attackpreview;
        public void Show(Character attacker, Character defender)
        {
          /*  visible = true;
            this.attacker = attacker;
            this.defender = defender;
            attackpreview.SetActive(true);
            GameObject.Find("AttackerName").GetComponent<Text>().text = attacker.name;
            GameObject.Find("DefenderName").GetComponent<Text>().text = defender.name;
            GameObject.Find("AttackerSprite").GetComponent<Image>().sprite = attacker.activeSpriteObject;
            GameObject.Find("DefenderSprite").GetComponent<Image>().sprite = defender.activeSpriteObject;
            string doubletext = "";
            if (attacker.CanDoubleAttack(defender))
                doubletext = " x 2";
            GameObject.Find("AttackerDamage").GetComponent<Text>().text = ""+attacker.GetDamageAgainstTarget(defender) +doubletext;
            if (defender.CanDoubleAttack(attacker))
                doubletext = " x 2";
            GameObject.Find("DefenderDamage").GetComponent<Text>().text = "" + defender.GetDamageAgainstTarget(attacker) + doubletext;
            GameObject.Find("AttackerAccuracy").GetComponent<Text>().text = "" + attacker.GetHitAgainstTarget(defender);
            GameObject.Find("DefenderAccuracy").GetComponent<Text>().text = "" + defender.GetHitAgainstTarget(attacker);*/

        }
        public void Hide()
        {
            visible = false;
            attackpreview.SetActive(false);
        }
    }
}
