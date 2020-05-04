using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    [System.Serializable]
    public class AttackPreviewStatBar : MonoBehaviour
    {
        private const float MIN_WIDTH = 0;
        [SerializeField] private FilledBarController filledBarController = default;
        [SerializeField] private GameObject valueBeforeMarker = default;
        [SerializeField] private RectTransform incDamageSection = default;
        [SerializeField] private TextMeshProUGUI valueAfterText = default;
        //[SerializeField] private TextMeshProUGUI incomingDamageText = default;
        [SerializeField] private TextMeshProUGUI currentValue = default;

        [SerializeField] private GameObject incDamageBarPrefab = default;
        
        private List<GameObject> incDamageMarkers = new List<GameObject>();
        //[SerializeField] private TextMeshProUGUI maxValue = default;
        private ColorManager colorManager;
  
        public void UpdateValues(int maxHp, int currentHp, int afterBattleHp,List<int> incomingDamage)
        {
            incomingDamage.Reverse();
            filledBarController.SetFillAmount((afterBattleHp * 1.0f)/maxHp);
            float width = GetComponent<RectTransform>().rect.width;
            float sumIncDamage = incomingDamage.Sum();
            foreach (GameObject go in incDamageMarkers)
            {
                Destroy(go);
            }

            //Debug.Log(maxHp+" "+ currentHp+ " "+ afterBattleHp+": "+sumIncDamage);
            incDamageMarkers.Clear();
            float sumBefore = 0;
            for (int i = 0; i < incomingDamage.Count - 1; i++)
            {
                //Debug.Log(incomingDamage[i]);
                float xOffset = (incomingDamage[i] * 1.0f) / maxHp+sumBefore;
                sumBefore += xOffset;
                GameObject go = GameObject.Instantiate(incDamageBarPrefab, incDamageSection);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(xOffset * width, 0);
                go.GetComponent<Image>().color = incDamageSection.GetComponent<Image>().color;
                incDamageMarkers.Add(go);
            }
            //Debug.Log(incomingDamage[incomingDamage.Count - 1]);
            float value = (sumIncDamage * 1.0f) / maxHp;
            float value2 =  (maxHp*1.0f-currentHp)/maxHp;
            incDamageSection.anchoredPosition = new Vector2(value2 * -width, incDamageSection.anchoredPosition.y);
            incDamageSection.sizeDelta = new Vector2(Math.Max(MIN_WIDTH,(int)(value* width)),incDamageSection.sizeDelta.y);
         
            //valueBeforeMarker.SetActive(afterBattleHp != currentHp);

            //incomingDamageText.text = "-" + incomingDamage;
            valueAfterText.text="" + afterBattleHp;
            var textColor = Color.white;
            if (colorManager == null)
                colorManager = FindObjectOfType<ColorManager>();
            if (value2 > 0.75f)
                textColor = colorManager.MainRedColor; ;
            valueAfterText.color = textColor;
            currentValue.text = "" + currentHp;
        }
    }
}