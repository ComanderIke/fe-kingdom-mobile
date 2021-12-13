using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIDragable : MonoBehaviour, IInitializePotentialDragHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
    
        
        public RectTransform rectTransform;
        private Canvas canvas;

        public Action<PointerEventData> OnBeginDragHandler;
        public Action<PointerEventData> OnDragHandler;
        public Action<PointerEventData, bool> OnEndDragHandler;

        public bool FollowCursor { get; set; } = true;
        public Vector3 StartPosition;
        public bool CanDrag { get; set; } = true;
        private Transform startParent;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void OnDrag(PointerEventData eventData)
        {
        
            rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanDrag)
                return;
            OnDragHandler?.Invoke(eventData);
            //Debug.Log(rectTransform.anchoredPosition);
            if (FollowCursor)
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            transform.SetParent(canvas.transform);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!CanDrag)
                return;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            DropArea dropArea = null;
            foreach (var result in results)
            {
                dropArea = result.gameObject.GetComponent<DropArea>();
                if (dropArea != null)
                    break;
            }

            if (dropArea != null)
            {
                if (dropArea.Accepts(this))
                {
                    dropArea.Drop(this);
                    OnEndDragHandler?.Invoke(eventData, true);
                    return;
                }
            }

            rectTransform.anchoredPosition = StartPosition;
            transform.SetParent(startParent);
            OnEndDragHandler?.Invoke(eventData, false);
            
        }




        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            StartPosition = rectTransform.anchoredPosition;
            startParent = transform.parent;
        }
    }
}
