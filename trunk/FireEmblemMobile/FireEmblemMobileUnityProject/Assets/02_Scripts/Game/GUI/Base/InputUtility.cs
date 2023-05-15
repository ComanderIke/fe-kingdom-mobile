using UnityEngine;

namespace LostGrace
{
    public class InputUtility
    {
        public static bool TouchEnd()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    return true;
                }
            }

            return false;
        }
    
    }
}