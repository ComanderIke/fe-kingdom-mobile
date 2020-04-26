using UnityEngine;

namespace Assets.GameActors.Units.OnGameObject
{
    public class DragManager
    {
        
        const float DRAG_DELAY = 0.15f;
        const float DRAG_FOLLOW_SPEED = 13;
        public static bool IsAnyUnitDragged = false;

        public bool IsDraggingBeforeDelay { get; set; }
        public bool IsDragging { get; set; }
        public float DragTime { get; set; }
        public bool IsDragDelay { get; set; }
        public IDragAble DragObject { get; set; }
        /*DragOffset*/
        private float deltaPosX;
        private float deltaPosY;

        private Vector3 posBeforeDrag;

        public DragManager(IDragAble dragObject)
        {
            IsDraggingBeforeDelay = false;
            IsDragDelay = true;
            IsDragging = false;
            DragObject = dragObject;
        }

        public void Update()
        {
            if (IsDraggingBeforeDelay)
            {
                if (Input.GetMouseButton(0))
                {
                    DragTime += Time.deltaTime;
                    if (DragTime >= DRAG_DELAY)
                    {
                        IsAnyUnitDragged = true;
                        IsDragDelay = false;
                    }
                }
                else
                {
                    IsDraggingBeforeDelay = false;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (!IsDragDelay)
                    {
                        IsDraggingBeforeDelay = false;
                        IsAnyUnitDragged = false;
                        EndDrag();
                    }
                }
             }
            else
            {
                DragObject.NotDragging();
            }
        }

        public void StartDrag()
        {
            IsDragging = false;
            IsDragDelay = true;
            DragTime = 0;
            Vector3 dist = Camera.main.WorldToScreenPoint(DragObject.GetTransform().position);
            deltaPosX = Input.mousePosition.x - dist.x;
            deltaPosY = Input.mousePosition.y - dist.y;
            posBeforeDrag = DragObject.GetTransform().localPosition;
            DragObject.StartDrag();
        }

        public void Dragging()
        {
            IsDraggingBeforeDelay = true;
           
            if (!IsDragDelay)
            {
                IsDragging = true;
                Vector3 curPos = new Vector3(Input.mousePosition.x - deltaPosX, Input.mousePosition.y - deltaPosY, 0);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
                worldPos.z = 0;
                worldPos.x -= Map.MapSystem.GRID_X_OFFSET;
                DragObject.GetTransform().localPosition = Vector3.Lerp(DragObject.GetTransform().localPosition, worldPos, Time.deltaTime * DRAG_FOLLOW_SPEED);
                DragObject.Dragging();
            }
        }

        public void EndDrag()
        {
            IsDragging = false;
            DragObject.GetTransform().localPosition = posBeforeDrag;
            DragObject.EndDrag();
        }
    }
}
