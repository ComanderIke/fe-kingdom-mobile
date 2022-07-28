using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utility
{
    public class UIHelper
    {
        public static bool IsPointerOverGameObject(){
            //check mouse
            if(EventSystem.current.IsPointerOverGameObject())
                return true;
             
            //check touch
            if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ){
                if(EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }
             
            return false;
        }
    }
}