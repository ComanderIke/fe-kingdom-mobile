using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace __2___Scripts.Game.Utility
{
    public static class UIClickChecker
    {
        
        public static bool CheckUIObjectsInPosition()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            //Debug.Log("Raycast Position: " + pointer.position);
 
            if (raycastResults.Count > 0)
            {
                //Debug.Log("Results: " + raycastResults.Count);
                foreach (var go in raycastResults)
                {
                    if (!go.gameObject.CompareTag("Grid"))
                    {
                      //  Debug.Log("RAYCAST NOT GRID: "+go.gameObject.name);
                       
                    }
                    else
                    {
                    //    Debug.Log("RAYCAST GRID: "+go.gameObject.name);
                    }
                }
            }

            if (raycastResults.Count > 0)
            {  
            //    Debug.Log("UICLICKCHECKER RETURN TRUE");
                return true;
            }

         //   Debug.Log("UICLICKCHECKER RETURN FALSE");
            return false;
        }
    }
}