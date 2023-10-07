using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    [RequireComponent(typeof (Canvas))]
    public class UIInteractionBlocker : MonoBehaviour
    {
        [SerializeField] private GameObject blockerGO;
        private Canvas canvas;
        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            blockerGO.gameObject.SetActive(canvas.enabled);
        }
    }
}
