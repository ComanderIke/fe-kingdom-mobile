using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Feedbacks;
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
        [SerializeField] private TextMeshProUGUI hpText = default;
        [SerializeField] private TextMeshProUGUI valueAfterText = default;
        [SerializeField] private MMF_Player feedbacks;

        private RectTransform rectTransform;
        public void UpdateValues(int maxHp, int currentHp, int afterBattleHp)
        {

            float width = backgroundHpBar.rectTransform.rect.width;
            Debug.Log(maxHp+" "+currentHp+" "+afterBattleHp);
            currentHpBar.rectTransform.sizeDelta = new Vector2(width*((afterBattleHp * 1.0f)/maxHp),currentHpBar.rectTransform.sizeDelta.y);
            
            beforeHpBar.rectTransform.sizeDelta = new Vector2(width*((currentHp * 1.0f) / maxHp),beforeHpBar.rectTransform.sizeDelta.y);
             if (rectTransform == null)
                 rectTransform = GetComponent<RectTransform>();

             valueAfterText.text = "" + afterBattleHp;
             hpText.text =""+ currentHp;
             
             MonoUtility.InvokeNextFrame(() =>
             {
                 feedbacks.PlayFeedbacks();
             });


        }
        private void OnDisable()
        {
            // for (int i = 0; i < incDamageMarkers.Count; i++)
            // {
            //     incDamageMarkers[i].gameObject.SetActive(false);
            // }
            // LeanTween.pause(valueAfterText.gameObject);
        }
    }
}