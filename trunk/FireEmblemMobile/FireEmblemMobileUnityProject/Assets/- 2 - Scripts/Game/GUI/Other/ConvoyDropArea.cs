using UnityEngine;

namespace Game.GUI
{
    public class ConvoyDropArea:MonoBehaviour
    {
        protected DropArea DropArea;

        protected virtual void Awake()
        {
            DropArea = GetComponent<DropArea>() ?? gameObject.GetComponent<DropArea>();
            DropArea.OnDropHandler += OnItemDropped;
        }

        private void OnItemDropped(UIDragable dragable)
        {
            dragable.transform.SetParent(transform);
        }
    }
}