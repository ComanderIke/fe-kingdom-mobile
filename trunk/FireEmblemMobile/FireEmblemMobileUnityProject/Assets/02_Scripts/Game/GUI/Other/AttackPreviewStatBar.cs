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
        [SerializeField] private UIFilledBarController uiFilledBarController = default;
        [SerializeField] private RectTransform incDamageSection = default;
        [SerializeField] private TextMeshProUGUI valueAfterText = default;

        private List<RectTransform> incDamageMarkers = new List<RectTransform>();
        private RectTransform rectTransform;
        public void UpdateValues(int maxHp, int currentHp, int afterBattleHp,List<int> incomingDamage, bool ShowHP = false)
        {
            incomingDamage.Reverse();
            if(afterBattleHp == -1)
                uiFilledBarController.SetFillAmount((currentHp * 1.0f)/maxHp);
            else
                uiFilledBarController.SetFillAmount((afterBattleHp * 1.0f) / maxHp);
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            float width = rectTransform.rect.width;
            float sumIncDamage = incomingDamage.Sum();

            float sumBefore = 0;
            for (int i = 0; i < incomingDamage.Count - 1; i++)
            {
                float xOffset = (incomingDamage[i] * 1.0f) / maxHp+sumBefore;
                sumBefore += xOffset;
                
            }
            float value = (sumIncDamage * 1.0f) / maxHp;
            float value2 =  (maxHp*1.0f-currentHp)/maxHp;
            incDamageSection.anchoredPosition = new Vector2(value2 * -width, incDamageSection.anchoredPosition.y);
            incDamageSection.sizeDelta = new Vector2(Math.Max(MIN_WIDTH,(int)(value* width)),incDamageSection.sizeDelta.y);

            var textColor = Color.white;
            if (value2 > 0.75f)
                textColor = ColorManager.Instance.MainRedColor;
            valueAfterText.color = textColor;
            valueAfterText.text = "" + afterBattleHp;
            
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