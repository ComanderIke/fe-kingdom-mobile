using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    [Serializable]
    public class AttackPreviewStatBar : MonoBehaviour
    {
        private const float MIN_WIDTH = 0;
        [SerializeField] private Image backgroundHpBar;
        [SerializeField] private Image currentHpBar;
        [SerializeField] private Image beforeHpBar;
        [SerializeField] private Image losingHpBar;
        [SerializeField] private TextMeshProUGUI hpText = default;
        [SerializeField] private TextMeshProUGUI valueAfterText = default;
        [SerializeField] private MMF_Player feedbacks;
        [SerializeField] private Image hpIndicator;
        [SerializeField] private MMProgressBar progressBar;
        private RectTransform rectTransform;
        private float width;
        private int currentHp;
        public void UpdateValues(int maxHp, int currentHp, int afterBattleHp)
        {

//            Debug.Log("UpdateValues");
            this.currentHp = currentHp;
            width = backgroundHpBar.rectTransform.rect.width;
            Debug.Log(maxHp+" "+currentHp+" "+afterBattleHp);
            currentHpBar.rectTransform.sizeDelta = new Vector2(width*((afterBattleHp * 1.0f)/maxHp),currentHpBar.rectTransform.sizeDelta.y);
            beforeHpBar.gameObject.SetActive(true);
            beforeHpBar.rectTransform.sizeDelta = new Vector2(width*((currentHp * 1.0f) / maxHp),beforeHpBar.rectTransform.sizeDelta.y);
             if (rectTransform == null)
                 rectTransform = GetComponent<RectTransform>();

             valueAfterText.gameObject.SetActive(true);
             losingHpBar.gameObject.SetActive(false);
             valueAfterText.text = "" + afterBattleHp;
           
             hpText.text =""+ currentHp;
             hpIndicator.gameObject.SetActive(true);
             MonoUtility.InvokeNextFrame(() =>
             {
                 feedbacks.PlayFeedbacks();
             });


        }
    
        public void UpdateValuesWithoutDamagePreview(int maxHp, int currentHp, int afterHp)
        {
            this.currentHp = currentHp;
            width = backgroundHpBar.rectTransform.rect.width;
            hpText.text =""+ currentHp;
            Debug.Log(maxHp+" "+currentHp+" "+afterHp);
           // currentHpBar.rectTransform.sizeDelta = new Vector2(width*((currentHp * 1.0f)/maxHp),currentHpBar.rectTransform.sizeDelta.y);
            valueAfterText.gameObject.SetActive(false);
            losingHpBar.gameObject.SetActive(true);
           // beforeHpBar.gameObject.SetActive(false);
            hpIndicator.gameObject.SetActive(false);
            Debug.Log("set HP Bar: "+(this.currentHp * 1.0f)/maxHp);
            //progressBar.Initialization();
            progressBar.SetBar01(((this.currentHp * 1.0f)/maxHp));
           // progressBar.UpdateBar01(((afterHp * 1.0f)/maxHp));
        }

        public void UpdateValuesAnimated(int maxHp, int newHp)
        {
            hpText.text =""+ newHp;
            //currentHpBar.rectTransform.sizeDelta = new Vector2(width*((newHp * 1.0f)/maxHp),currentHpBar.rectTransform.sizeDelta.y);
           // beforeHpBar.gameObject.SetActive(true);
          //  beforeHpBar.rectTransform.sizeDelta = new Vector2(width*((currentHp * 1.0f) / maxHp),beforeHpBar.rectTransform.sizeDelta.y);
         // progressBar.SetBar01(((this.currentHp * 1.0f)/maxHp)); 
          currentHp = newHp;
            //progressBar.SetBar01(((currentHp * 1.0f) / maxHp));
            Debug.Log("update HP Bar: "+(newHp * 1.0f)/maxHp);

            
            progressBar.UpdateBar01(((newHp * 1.0f)/maxHp));
            MonoUtility.InvokeNextFrame(() =>
            {
               // progressBar.Minus10Percent();
                feedbacks.PlayFeedbacks();
            });
        }
    }
}