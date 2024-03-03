using UnityEngine;

namespace Game.GUI.Other
{
    public class EquipmentSlot:MonoBehaviour
    {
        protected DropArea DropArea;

        protected virtual void Awake()
        {
            DropArea = GetComponent<DropArea>() ?? gameObject.GetComponent<DropArea>();
            DropArea.OnDropHandler += OnItemDropped;
        }

        private void OnItemDropped(UIDragable dragable)
        {
            dragable.transform.position = transform.position;
            dragable.transform.SetParent(transform);
        }
    }
}