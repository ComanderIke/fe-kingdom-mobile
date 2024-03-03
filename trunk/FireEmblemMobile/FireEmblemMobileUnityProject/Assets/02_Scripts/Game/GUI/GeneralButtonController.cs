using System;
using UnityEngine;

namespace Game.GUI
{
    public class GeneralButtonController : MonoBehaviour
    {
        public event Action<RectTransform> OnClicked;

        public void Clicked()
        {
            OnClicked?.Invoke(GetComponent<RectTransform>());
        }
    }
}
