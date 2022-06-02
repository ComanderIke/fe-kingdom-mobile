using System;
using Game.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class ExpBarController : MonoBehaviour
    {
        Vector3 oldPosition;
        public TextMeshProUGUI text;
        public Image fill;
        public GameObject glow;
        public int currentExp;

        private int addedExp;
        
        // float textspeed = 3;
        public float AnimateInDuration=0.25f;
        public float AnimateStayDuration = 0.45f;
        public float AnimateOutDuration = 0.2f;


        public void ParticleArrived()
        {
            addedExp--;
            currentExp++;
             fill.fillAmount = currentExp / 100f;
            // text.text = "" + (currentExp);
            // LeanTween.scale(gameObject, new Vector3(1.4f, 1.4f, 1), 0.1f).setEaseSpring();
            // oldPosition = transform.localPosition;
           // Debug.Log("1EXP! Arrived ");
            if (addedExp <=0)
            {
                Debug.Log("AllParticlesArrived!");
                animation = false;
               FindObjectOfType<ExpParticleSystem>().AllFinished();
               
            }
        }

       
        public void UpdateValues(int addedExp)
        {
           // Debug.Log("UPDATE VALUEAS!");
            Debug.Log("CurrentExp: "+currentExp+" Gained: "+addedExp);
            //this.currentExp = currentExp;
            animation = true;
            this.addedExp +=  addedExp;
        }

        
  
        void ShowTextAnimation()
        {
            // int exp = currentExp;
            // float fillAmount = 0;
            // float expLeft = 0;
            // LeanTween.value(gameObject, 0, addedExp, Math.Max(addedExp / 100f*1.5f, 0.4f)).setEaseOutQuad().setOnUpdate((float val) =>
            // {
            //
            //     int intVal = (int)val;
            //     int expVal = exp + intVal;
            //     if (expVal > 100)
            //     {
            //         expLeft = expVal - 100;
            //         expVal = 100;
            //       
            //     }
            //
            //     text.text = "" + (expVal);
            //     fillAmount =  (expVal) /100f;
            //     fill.fillAmount = fillAmount;
            //
            // }).setOnComplete(Hide).setDelay(0.10f);
        }
       

        public void SetText(int exp)
        {
            text.text = "" + (exp);
        }

        private bool animation = false;
        public void SetFillAmount(int expVal)
        {
            if (animation)
            {
                return;
            }
            float expLeft = 0;
            if (expVal > 100)
            {
                expLeft = expVal - 100;
                expVal = 100;
                  
            }

            this.currentExp = expVal;
            fill.fillAmount = currentExp / 100f;
            if (Math.Abs(fill.fillAmount - 1) < 0.1f)
                fill.fillAmount = 0;
        }
    }
}
