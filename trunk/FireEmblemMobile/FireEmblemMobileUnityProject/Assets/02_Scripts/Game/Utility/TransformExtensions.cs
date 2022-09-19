using UnityEditor;
using UnityEngine;

namespace __2___Scripts.Game.Utility
{
    public static class TransformExtensions
    {
        public static void DeleteChildren(this Transform transform)
        {
            
            foreach(Transform child in transform)Object.Destroy(child.gameObject);
        }
        public static void DeleteAllChildren(this Transform transform)
        {
            var children = transform.GetComponentsInChildren<Transform>();
            for (int i = children.Length - 1; i >= 0; i--)
            {
                if(children[i]!=transform)
                    GameObject.Destroy(children[i].gameObject);
            }
        }
        public static void DeleteAllChildrenImmediate(this Transform transform)
        {
            var children = transform.GetComponentsInChildren<Transform>();
            for (int i = children.Length - 1; i >= 0; i--)
            {
                if(children[i]!=transform)
                    GameObject.DestroyImmediate(children[i].gameObject);
            }
        }

      
    }
}