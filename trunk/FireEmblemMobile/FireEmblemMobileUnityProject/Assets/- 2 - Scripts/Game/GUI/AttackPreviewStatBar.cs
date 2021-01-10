using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utility;

namespace Game.GUI
{
    [Serializable]
    public class AttackPreviewStatBar : MonoBehaviour
    {
        private const float MIN_WIDTH = 0;
        [SerializeField] private FilledBarController filledBarController = default;
        [SerializeField] private GameObject valueBeforeMarker = default;
        [SerializeField] private RectTransform incDamageSection = default;
        [SerializeField] private TextMeshProUGUI valueAfterText = default;
        //[SerializeField] private TextMeshProUGUI incomingDamageText = default;
        [SerializeField] private TextMeshProUGUI currentValue = default;
        [SerializeField] private TextMeshProUGUI Arrow = default;

        [SerializeField] private GameObject incDamageBarPrefab = default;
        
        private List<RectTransform> incDamageMarkers = new List<RectTransform>();
        //[SerializeField] private TextMeshProUGUI maxValue = default;
        private RectTransform rectTransform;
        public void UpdateValues(int maxHp, int currentHp, int afterBattleHp,List<int> incomingDamage, bool ShowHP = false)
        {
            incomingDamage.Reverse();
            if(afterBattleHp == -1)
                filledBarController.SetFillAmount((currentHp * 1.0f)/maxHp);
            else
                filledBarController.SetFillAmount((afterBattleHp * 1.0f) / maxHp);
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            float width = rectTransform.rect.width;
            float sumIncDamage = incomingDamage.Sum();
            
            //Debug.Log(maxHp+" "+ currentHp+ " "+ afterBattleHp+": "+sumIncDamage);
            float sumBefore = 0;
            for (int i = 0; i < incomingDamage.Count - 1; i++)
            {
                //Debug.Log(incomingDamage[i]);
                float xOffset = (incomingDamage[i] * 1.0f) / maxHp+sumBefore;
                sumBefore += xOffset;
                //RectTransform go;
                //if (i < incDamageMarkers.Count)
                //{
                //    go = incDamageMarkers[i];
                //    go.gameObject.SetActive(true);

                //}
                //else
                //{
                //    go = Instantiate(incDamageBarPrefab, incDamageSection).GetComponent<RectTransform>();
                //    incDamageMarkers.Add(go);
                //}
                //go.anchoredPosition = new Vector2(xOffset * width, 0);
                
            }
            //Debug.Log(incomingDamage[incomingDamage.Count - 1]);
            float value = (sumIncDamage * 1.0f) / maxHp;
            float value2 =  (maxHp*1.0f-currentHp)/maxHp;
            incDamageSection.anchoredPosition = new Vector2(value2 * -width, incDamageSection.anchoredPosition.y);
            incDamageSection.sizeDelta = new Vector2(Math.Max(MIN_WIDTH,(int)(value* width)),incDamageSection.sizeDelta.y);

            //valueBeforeMarker.SetActive(afterBattleHp != currentHp);
            //incomingDamageText.text = "-" + incomingDamage;
            if (afterBattleHp == -1)
            {
                valueAfterText.text = "?";
                valueAfterText.color = Color.white;
                if (ShowHP)
                {
                    valueAfterText.text = "?<"+currentHp;
                }

                LeanTween.cancel(valueAfterText.gameObject);
            }
            else
            {
                
                
                var textColor = Color.white;
                if (value2 > 0.75f)
                    textColor = ColorManager.Instance.MainRedColor;
                valueAfterText.color = textColor;
                if (afterBattleHp == 0)
                {
                    //valueAfterText.color = colorManager.MainRedColor;

                    //if (!LeanTween.isTweening(valueAfterText.gameObject.GetComponent<RectTransform>()))
                    //{
                    //    valueAfterText.gameObject.transform.localScale = Vector3.one;
                    //    LeanTween.scale(valueAfterText.gameObject.GetComponent<RectTransform>(), Vector3.one * 1.3f, 0.5f).setLoopPingPong();
                    //}
                    //else
                    //{
                    //    valueAfterText.gameObject.transform.localScale = Vector3.one;
                    //    LeanTween.resume(valueAfterText.gameObject);
                    //}
                 }
                else
                {
                    //LeanTween.cancel(valueAfterText.gameObject);
                }
                if (afterBattleHp == currentHp)
                {
                    valueAfterText.text = "" + afterBattleHp;

                }
                else {
                    valueAfterText.text = "" + afterBattleHp+"<"+ currentHp;
                }

            }
            
        }
        private void OnDisable()
        {
            for (int i = 0; i < incDamageMarkers.Count; i++)
            {
                incDamageMarkers[i].gameObject.SetActive(false);
            }
            LeanTween.pause(valueAfterText.gameObject);
        }
    }
}