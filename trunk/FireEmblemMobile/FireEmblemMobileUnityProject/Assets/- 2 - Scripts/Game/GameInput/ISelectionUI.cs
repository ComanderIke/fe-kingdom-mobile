using System;
using UnityEngine;

namespace Game.GameInput
{
    public abstract class ISelectionUI : MonoBehaviour
    {
     
        public abstract void ShowUndo();

        public abstract void HideUndo();
        public abstract void ShowSkills();

        public abstract void HideSkills();
        public abstract void ShowItems();

        public abstract void HideItems();
 
    }
}