using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace LostGrace
{
    public class ClickAndHoldButton : MonoBehaviour
    {
        private const bool clickAndHold = true;
        [SerializeField] private float holdTime = 1.2f;
        private float time = 0;
        [SerializeField] private Image fill;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image backGround;
        public event Action OnClick;
        private bool pointerdown = false;
        private bool pointerUpLastFrame = false;

        public bool IsPressing
        {
            get
            {
                return pointerdown;
            }
        }

        public bool WasPressingUntilLastFrame
        {
            get
            {
                return pointerUpLastFrame;
            }
        }

        public void SetBackgroundColor(Color color)
        {
            backGround.color = color;
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        void Update()
        {
            if (pointerdown)
            {
                time += Time.deltaTime;
                if (time >= holdTime)
                {
                    OnClick?.Invoke();
                    Reset();
                }
                fill.fillAmount = time / holdTime;
            }
           
        }

        private void Reset()
        {
            pointerdown = false;
            time = 0;
            fill.fillAmount = 0;
        }

        public void Click()
        {
            if(clickAndHold==false)
                OnClick?.Invoke();
        }

        public void OnPointerDown()
        {
            pointerdown = true;
        }
        public void OnPointerUp()
        {
            Reset();
            pointerUpLastFrame = true;
            StartCoroutine(ResetPointerUp());
        }

        IEnumerator ResetPointerUp()
        {
            yield return null;
            pointerUpLastFrame = false;
        }
    }
}
