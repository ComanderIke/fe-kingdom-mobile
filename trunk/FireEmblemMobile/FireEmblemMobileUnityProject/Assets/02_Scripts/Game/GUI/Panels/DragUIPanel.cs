using GameCamera;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GUI.Panels
{
    public class DragUIPanel : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform dragRectTransform;
        [SerializeField] private Canvas canvas;
   
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
            DragCameraMixin.blockDrag = true;

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragCameraMixin.blockDrag = false;
        }
    }
}
