using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class DragManager
    {
        
        const float DRAG_DELAY = 0.15f;
        const float DRAG_FOLLOW_SPEED = 13;
        public static bool IsAnyUnitDragged = false;

        public bool IsDragging { get; set; }
        public float DragTime { get; set; }
        public bool IsDragDelay { get; set; }
        public DragAble DragObject { get; set; }
        /*DragOffset*/
        private float deltaPosX;
        private float deltaPosY;

        private Vector3 posBeforeDrag;

        public DragManager(DragAble dragObject)
        {
            IsDragging = false;
            IsDragDelay = true;
            DragObject = dragObject;
        }

        public void Update()
        {
            if (IsDragging)
            {
                if (UnityEngine.Input.GetMouseButton(0))
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
                    IsDragging = false;
                }
                if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    if (!IsDragDelay)
                    {
                        IsDragging = false;
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
            IsDragDelay = true;
            DragTime = 0;
            Vector3 dist = Camera.main.WorldToScreenPoint(DragObject.GetTransform().position);
            deltaPosX = UnityEngine.Input.mousePosition.x - dist.x;
            deltaPosY = UnityEngine.Input.mousePosition.y - dist.y;
            posBeforeDrag = DragObject.GetTransform().localPosition;
            DragObject.StartDrag();
        }

        public void Dragging()
        {
            IsDragging = true;

            if (!IsDragDelay)
            {
                Vector3 curPos = new Vector3(UnityEngine.Input.mousePosition.x - deltaPosX, UnityEngine.Input.mousePosition.y - deltaPosY, 0);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
                worldPos.z = 0;
                worldPos.x -= GridSystem.GRID_X_OFFSET;
                DragObject.GetTransform().localPosition = Vector3.Lerp(DragObject.GetTransform().localPosition, worldPos, Time.deltaTime * DRAG_FOLLOW_SPEED);
                DragObject.Dragging();
            }
        }

        public void EndDrag()
        {
            DragObject.GetTransform().localPosition = posBeforeDrag;
            DragObject.EndDrag();
        }
    }
}
