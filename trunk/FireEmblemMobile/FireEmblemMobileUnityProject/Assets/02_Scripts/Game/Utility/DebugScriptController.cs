using Game.GUI.Other;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Utility
{
    public class DebugScriptController : MonoBehaviour
    {
        // Start is called before the first frame update
        public AudioListener audioListener;
        public EventSystem eventSystem;
        public StandaloneInputModule inputModule;
        public SingletonReferences singletonReferences;

        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            if (FindObjectsOfType<AudioListener>().Length >= 2)
            {
                if(audioListener!=null)
                    audioListener.enabled = false;
            }
            if (FindObjectsOfType<EventSystem>().Length >= 2)
            {
                eventSystem.enabled = false;
            }
            if (FindObjectsOfType<StandaloneInputModule>().Length >= 2)
            {
                inputModule.enabled = false;
            }
            if (FindObjectsOfType<SingletonReferences>().Length >= 2)
            {
                if(singletonReferences!=null)
                    singletonReferences.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}