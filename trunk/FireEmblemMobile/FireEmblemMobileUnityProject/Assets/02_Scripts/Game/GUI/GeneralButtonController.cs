using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
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
