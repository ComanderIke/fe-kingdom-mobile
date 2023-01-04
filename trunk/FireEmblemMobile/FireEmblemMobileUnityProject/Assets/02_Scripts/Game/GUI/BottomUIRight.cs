using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class BottomUIRight : BottomUIBase
    {

        [SerializeField] private CanvasGroup redbackgroundAndFrame;
        public void Show(IGridActor unit)
        {
            redbackgroundAndFrame.gameObject.SetActive(true);
            TweenUtility.FadeIn(redbackgroundAndFrame);
            base.Show();
        }

        public override void Hide()
        {
            
            TweenUtility.FadeOut(redbackgroundAndFrame).setOnComplete(()=>redbackgroundAndFrame.gameObject.SetActive(false));
            base.Hide();
        }

      
    }
}
