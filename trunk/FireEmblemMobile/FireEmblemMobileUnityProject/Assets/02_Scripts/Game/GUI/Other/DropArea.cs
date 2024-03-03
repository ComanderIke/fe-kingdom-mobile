using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GUI.Other
{
    public  class DropArea:MonoBehaviour
    {
        public List<DropCondition> dropConditions = new List<DropCondition>();
        public event Action<UIDragable> OnDropHandler;
        public event Action<UIDragable> OnDaggedOverHandler;

        public bool Accepts(UIDragable dragable)
        {
            return dropConditions.TrueForAll(cond => cond.Check(dragable));
        }

        public void Drop(UIDragable dragable)
        {
            OnDropHandler?.Invoke(dragable);
        }
        public void DraggedOver(UIDragable dragable)
        {
            OnDaggedOverHandler?.Invoke(dragable);
        }

    }
}