using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIAttackPreviewContainer : MonoBehaviour
    {
 
        [SerializeField] private Image faceSprite;
        [SerializeField] private TextMeshProUGUI dmgValue;
        [SerializeField] private TextMeshProUGUI hitValue;
        [SerializeField] private TextMeshProUGUI critValue;
        [SerializeField] private AttackPreviewStatBar hpBar;


        public void Show(Sprite face, int dmg, int hit, int crit, int maxHp, int currentHp, int afterHp, bool canCounter=true)
        {
            faceSprite.sprite = face;
            hpBar.UpdateValues(maxHp, currentHp, afterHp);
            if (!canCounter)
            {
                dmgValue.text = "-";
                hitValue.text = "-";
                critValue.text = "-";
                return;
            }
            dmgValue.text = "" + dmg;
            hitValue.text = "" + hit;
            critValue.text = "" + crit;
           
        }

  
    }
}
