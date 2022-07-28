using UnityEngine;

namespace GameEngine
{
    public abstract class SingletonScriptableObject<T>: ScriptableObject where T :ScriptableObject
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] results = Resources.FindObjectsOfTypeAll<T>();
                    if (results.Length == 0)
                    {
                        Debug.LogError("SingletonScriptableObject results length==0 "+typeof(T).ToString());
                        return null;
                    }
                    if (results.Length > 1)
                    {
                        Debug.LogError("SingletonScriptableObject results length>1 "+typeof(T).ToString());
                        return null;
                    }

                    instance = results[0];
                    instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }

                return instance;
            }
        }
    }
}