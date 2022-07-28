using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameInput
{
    public class StandaloneInputModuleV2 : StandaloneInputModule
    {
        public GameObject GameObjectUnderPointer(int pointerId)
        {
            var lastPointer = GetLastPointerEventData(pointerId);
            if (lastPointer != null)
                return lastPointer.pointerCurrentRaycast.gameObject;
            return null;
        }
 
        public GameObject GameObjectUnderPointer()
        {
            return GameObjectUnderPointer(PointerInputModule.kMouseLeftId);
        }
    }
}