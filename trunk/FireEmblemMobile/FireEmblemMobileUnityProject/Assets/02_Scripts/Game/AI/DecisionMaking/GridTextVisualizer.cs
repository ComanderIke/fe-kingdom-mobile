using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.AI
{
    public class GridTextVisualizer:MonoBehaviour
    {
        private List<TextMeshProUGUI> texts;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Camera uiCamera;

        [SerializeField] private Canvas canvas;
        private void Start()
        {
            texts = new List<TextMeshProUGUI>();
        }

        public void ShowRed(Vector3 position, int Distance)
        {
            canvas.enabled = true;
            var text = Instantiate(textPrefab, transform);
            text.transform.position = uiCamera.WorldToScreenPoint(position);
            var textComp = text.GetComponent<TextMeshProUGUI>();
            textComp.SetText(""+Distance);
            texts.Add(textComp);
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
      
    }
}