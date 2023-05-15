using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Utility.UI
{
     
    public class InvisibleBGCtrl : MonoBehaviour, IPointerClickHandler
    {
        MenuVisibilityCtrl _parentCtrl;
        GraphicRaycaster m_Raycaster;
 
        public void setParentCtrl(MenuVisibilityCtrl ctrl)
        {
            _parentCtrl = ctrl;
        }
 
        public void OnPointerClick(PointerEventData eventData)
        {
            _parentCtrl.hide();
 
            m_Raycaster = transform.parent.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(eventData, results);
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);
                if (result.gameObject.GetComponent<Selectable>())
                {
                    ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
                }
            }
        }
    }
}