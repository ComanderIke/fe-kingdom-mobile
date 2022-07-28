using System;
using UnityEngine;

namespace Game.GameInput
{
    public abstract class ISelectionUI : MonoBehaviour
    {
     
        public abstract void ShowUndo();

        public abstract void HideUndo();

    }
}