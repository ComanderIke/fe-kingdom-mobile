using UnityEngine;

namespace Game.Utility.UI
{
    [RequireComponent(typeof (Canvas))]
    public class UIInteractionBlocker : MonoBehaviour
    {
       
        private Canvas canvas;
        private bool enabled;
        void Start()
        {
            canvas = GetComponent<Canvas>();
            
        }

        void Update()
        {
            if (canvas.enabled)
            {
                InteractionBlocker.Instance.SetActive(gameObject,true);
                enabled = true;
            }
            else if (!canvas.enabled && enabled)
            {
                InteractionBlocker.Instance.SetActive(gameObject,false);
                enabled = false;
            }
        }
    }
}
