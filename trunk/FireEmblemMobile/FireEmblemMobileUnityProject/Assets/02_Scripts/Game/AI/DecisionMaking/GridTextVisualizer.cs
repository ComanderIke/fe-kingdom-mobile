using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utility;

namespace Game.AI
{
    public class GridTextVisualizer:MonoBehaviour
    {
        private List<TextMeshProUGUI> texts;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Camera uiCamera;

        [SerializeField] private Canvas canvas;
        [SerializeField] private Color red;
        [SerializeField] private Color white;
        private void Start()
        {
            texts = new List<TextMeshProUGUI>();
        }

        public void ShowRed(Vector3 position, string text)
        {
            ShowText(position, text, red);
        }

        public void Clear()
        {
            for (int i = texts.Count-1; i >= 0; i--)
            {
                DestroyImmediate(texts[i].gameObject);
            }
            texts.Clear();
            canvas.enabled = false;
        }

        public void ShowText(Vector3 position, string text, Color color)
        {
            canvas.enabled = true;
            var textGo = Instantiate(textPrefab, transform);
            textGo.transform.position = uiCamera.WorldToScreenPoint(position);
            var textComp = textGo.GetComponent<TextMeshProUGUI>();
            textComp.SetText(""+text);
            textComp.color = color;
            texts.Add(textComp);
        }
        public void ShowWhite(Vector3 position, string text)
        {
            ShowText(position, text, white);
        }
    }
}