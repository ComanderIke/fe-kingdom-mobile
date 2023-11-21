using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class StatusEffectUI : MonoBehaviour
    {
       
        [SerializeField] private Image icon;
        [SerializeField] private GameObject durationCounterPrefab;

        [SerializeField] private Transform durationCounterContainer;

        public void Show(Sprite sprite, Color color, Color durationColor, int duration)
        {
            durationCounterContainer.DeleteAllChildren();
            icon.sprite = sprite;
            icon.color = color;
            for (int i = 0; i < duration; i++)
            {
                var go = Instantiate(durationCounterPrefab, durationCounterContainer);
                go.GetComponent<Image>().color =durationColor;
            }
        }
    }
}
