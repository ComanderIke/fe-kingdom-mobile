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
        private int maxHp;
        

        public void Show(Sprite face, int dmg, int hit, int crit, int maxHp, int currentHp, int afterHp, bool canCounter=true)
        {
            this.maxHp = maxHp;
            Debug.Log(afterHp +" "+currentHp);
            if (afterHp > currentHp) //Heal{
            {
                hpBar.UpdateValues(maxHp, currentHp, afterHp);
            }
            else
            {
                hpBar.UpdateValues(maxHp, currentHp, afterHp);
            }

            UpdateAllButHpBar(face, dmg, hit, crit, canCounter);
        }

        void UpdateAllButHpBar(Sprite face, int dmg, int hit, int crit, bool canCounter=true)
        {
            
            faceSprite.sprite = face;
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

        public void ShowInBattleContext(Sprite face, int dmg, int hit, int crit, int maxHp, int currentHp, int afterHp, bool canCounter=true)
        {
            UpdateAllButHpBar(face, dmg, hit, crit, canCounter);
            this.maxHp = maxHp;
            hpBar.UpdateValuesWithoutDamagePreview(maxHp, currentHp, afterHp);
        }

        public void UpdateHP(int currentHpRight)
        {
            Debug.Log("update HP in Container: "+maxHp+ " "+currentHpRight);
            hpBar.UpdateValuesAnimated(maxHp, currentHpRight);
        }
    }
}
