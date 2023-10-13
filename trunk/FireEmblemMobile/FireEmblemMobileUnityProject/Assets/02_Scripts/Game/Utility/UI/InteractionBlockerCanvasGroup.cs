using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
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
