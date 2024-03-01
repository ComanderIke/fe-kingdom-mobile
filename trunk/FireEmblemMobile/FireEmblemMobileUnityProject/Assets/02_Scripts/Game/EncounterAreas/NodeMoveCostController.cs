using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class NodeMoveCostController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Vector3 startScale;

        private void Start()
        {
            startScale = transform.localScale;
        }

        public void Show(string text)
        {
            this.text.text = text;
        }

        public void Select()
        {
            LeanTween.scale(gameObject, startScale * 1.1f, .65f).setEaseOutQuad().setLoopPingPong(-1);
        }

        public void Deselect()
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, startScale, .2f).setEaseInOutQuad();
            //gameObject.transform.localScale = startScale;
        }
    }
}
