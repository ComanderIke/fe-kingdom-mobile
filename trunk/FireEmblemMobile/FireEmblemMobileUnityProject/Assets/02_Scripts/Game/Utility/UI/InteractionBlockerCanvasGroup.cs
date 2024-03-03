using UnityEngine;

namespace Game.Utility.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InteractionBlockerCanvasGroup : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

        }

        void Update()
        {
            canvasGroup.interactable = !InteractionBlocker.Instance.gameObject.activeSelf;
        }
    }
}
