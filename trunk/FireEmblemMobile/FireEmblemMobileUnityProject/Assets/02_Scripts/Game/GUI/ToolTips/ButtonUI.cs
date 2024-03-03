using System;
using UnityEngine;

namespace Game.GUI.ToolTips
{
    public class ButtonUI : MonoBehaviour
    {
        public event Action onClicked;

        public void Clicked()
        {
            onClicked?.Invoke();
        }
    }
}