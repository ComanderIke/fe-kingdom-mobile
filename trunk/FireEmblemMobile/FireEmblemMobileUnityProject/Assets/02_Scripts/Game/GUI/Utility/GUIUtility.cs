using UnityEngine;

namespace Game.GUI.Utility
{
    public class GUIUtility
    {
        public static void ClearChildren(Transform parent)
        {
            var children=parent.GetComponentsInChildren<Transform>();
            if (children.Length != 0)
            {
                for (int i = children.Length - 1; i >= 0; i--)
                {
                    if(children[i]!=parent)
                        GameObject.Destroy(children[i].gameObject);
                }
            
            }
        }
    }
}
