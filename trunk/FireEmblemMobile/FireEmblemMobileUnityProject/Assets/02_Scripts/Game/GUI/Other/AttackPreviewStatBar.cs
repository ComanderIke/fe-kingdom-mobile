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
        [SerializeField] private Image healingHpBar;
        [SerializeField] private TextMeshProUGUI hpText = default;
        [SerializeField] private TextMeshProUGUI valueAfterText = default;
        [SerializeField] private MMF_Player feedbacks;
        [SerializeField] private MMF_Player losingHPBarFeedbacks;
        [SerializeField] private Image hpIndicator;
        [SerializeField] private MMProgressBar progressBar;
        private RectTransform rectTransform;
        private float width;
        private int currentHp;
        [SerializeField] private Color mainAllyColor;
        [SerializeField] private Color mainEnemyColor;
        [SerializeField] private Color previewAllyColor;
        [SerializeField] private Color previewEnemyColor;
        [SerializeField] private Gradient blinkColorAlly;
        [SerializeField] private Gradient blinkColorEnemy;
        [SerializeField] private Color healColor;
        //[SerializeField] private Color losingHPBarColor;
        
        [SerializeField] private Gradient blinkColorEnemyHeal;
        public void UpdateValues(int maxHp, int currentHp, int afterBattleHp)
        {

//            Debug.Log("UpdateValues");
            this.currentHp = currentHp;
            width = GetComponent<RectTransform>().rect.width;
            // Debug.Log(maxHp+" "+currentHp+" "+afterBattleHp+ " "+width+" "+width*((currentHp * 1.0f) / maxHp)+" "+Math.Min(width,width*((currentHp * 1.0f) / maxHp)));
            currentHpBar.rectTransform.sizeDelta = new Vector2(Math.Min(width, width*((afterBattleHp * 1.0f)/maxHp)),currentHpBar.rectTransform.sizeDelta.y);
            beforeHpBar.gameObject.SetActive(true);
            beforeHpBar.rectTransform.sizeDelta = new Vector2(Math.Min(width,width*((currentHp * 1.0f) / maxHp)),beforeHpBar.rectTransform.sizeDelta.y);
             if (rectTransform == null)
                 rectTransform = GetComponent<RectTransform>();

             if(valueAfterText!=null)
                valueAfterText.gameObject.SetActive(true);
             losingHpBar.gameObject.SetActive(false);
             if (afterBattleHp > currentHp)
             {
                 healingHpBar.rectTransform.sizeDelta = new Vector2(Math.Min(width,width*((afterBattleHp * 1.0f) / maxHp)),beforeHpBar.rectTransform.sizeDelta.y);
                 currentHpBar.rectTransform.sizeDelta = new Vector2(Math.Min(width,width*((currentHp * 1.0f)/maxHp)),currentHpBar.rectTransform.sizeDelta.y);
                
             }
             beforeHpBar.gameObject.SetActive(afterBattleHp<=currentHp);
             healingHpBar.gameObject.SetActive(afterBattleHp>currentHp);
             if(valueAfterText!=null)
                valueAfterText.text = "<incr>" + afterBattleHp;
             if(hpText!=null)
                 hpText.text =""+ currentHp;
             if(hpIndicator!=null)
                hpIndicator.gameObject.SetActive(true);
             MonoUtility.InvokeNextFrame(() =>
             {
                 feedbacks.PlayFeedbacks();
             });


        }
    
        public void UpdateValuesWithoutDamagePreview(int maxHp, int currentHp, int afterHp)
        {
            this.currentHp = currentHp;
            width = GetComponent<RectTransform>().rect.width;
            if(hpText!=null)
                hpText.text =""+ currentHp;
         //   Debug.Log(maxHp+" "+currentHp+" "+afterHp);
           // currentHpBar.rectTransform.sizeDelta = new Vector2(width*((currentHp * 1.0f)/maxHp),currentHpBar.rectTransform.sizeDelta.y);
           if(valueAfterText!=null)
                valueAfterText.gameObject.SetActive(false);
           // Debug.Log(maxHp+" "+currentHp+" "+afterHp+ " "+width+" "+width*((currentHp * 1.0f) / maxHp)+" "+Math.Min(width,width*((currentHp * 1.0f) / maxHp)));

           healingHpBar.rectTransform.sizeDelta = new Vector2(Math.Min(width,width*((afterHp * 1.0f) / maxHp)),beforeHpBar.rectTransform.sizeDelta.y);
           currentHpBar.rectTransform.sizeDelta = new Vector2(Math.Min(width,width*((currentHp * 1.0f)/maxHp)),currentHpBar.rectTransform.sizeDelta.y);

           // beforeHpBar.gameObject.SetActive(afterHp<=currentHp);
           // healingHpBar.gameObject.SetActive(afterHp>currentHp);
           // beforeHpBar.gameObject.SetActive(false);
           if(hpIndicator!=null)
             hpIndicator.gameObject.SetActive(false);
            // Debug.Log("set HP Bar: "+(this.currentHp * 1.0f)/maxHp);
            //progressBar.Initialization();
            progressBar.SetBar01(((this.currentHp * 1.0f)/maxHp));
           // progressBar.UpdateBar01(((afterHp * 1.0f)/maxHp));
        }

        public void UpdateValuesAnimated(int maxHp, int newHp)
        {
            if(hpText!=null)
                hpText.text =""+ newHp;
            beforeHpBar.gameObject.SetActive(false);
            healingHpBar.gameObject.SetActive(true);
            //currentHpBar.rectTransform.sizeDelta = new Vector2(width*((newHp * 1.0f)/maxHp),currentHpBar.rectTransform.sizeDelta.y);
           // beforeHpBar.gameObject.SetActive(true);
          //  beforeHpBar.rectTransform.sizeDelta = new Vector2(width*((currentHp * 1.0f) / maxHp),beforeHpBar.rectTransform.sizeDelta.y);
         // progressBar.SetBar01(((this.currentHp * 1.0f)/maxHp)); 
          currentHp = newHp;
            //progressBar.SetBar01(((currentHp * 1.0f) / maxHp));
            // Debug.Log("update HP Bar: "+(newHp * 1.0f)/maxHp);
          
            
            progressBar.UpdateBar01(((newHp * 1.0f)/maxHp));
            MonoUtility.InvokeNextFrame(() =>
            {
               // progressBar.Minus10Percent();
                feedbacks.PlayFeedbacks();
            });
        }

        public void SetEnemyColors()
        {
            currentHpBar.color = mainEnemyColor;
            beforeHpBar.color=previewEnemyColor;
            
            feedbacks.GetFeedbackOfType<MMF_Image>().ColorOverTime = blinkColorEnemy;
            if(losingHPBarFeedbacks!=null)
                losingHPBarFeedbacks.GetFeedbackOfType<MMF_Image>().ColorOverTime = blinkColorEnemy;
        }

        public void SetAllyColors()
        {
            currentHpBar.color = mainAllyColor;
            beforeHpBar.color=previewAllyColor;
            feedbacks.GetFeedbackOfType<MMF_Image>().ColorOverTime = blinkColorAlly;
        }
    }
}