using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class DragManager
    {
        
        const float DRAG_DELAY = 0.15f;
        const float DRAG_FOLLOW_SPEED = 13;
        public static bool IsAnyUnitDragged = false;

        private bool IsDraggingBeforeDelay { get; set; }
        public bool IsDragging { get; set; }
        private float DragTime { get; set; }
        private bool IsDragDelay { get; set; }

        private IDragAble DragObserver { get; set; }
        /*DragOffset*/
        private float deltaPosX;
        private float deltaPosY;
        private Camera camera = Camera.main;

        private Vector3 posBeforeDrag;
        private Transform dragObjectTransform;

        public DragManager(IDragAble dragObserver)
        {
            IsDraggingBeforeDelay = false;
            IsDragDelay = true;
            IsDragging = false;
            DragObserver = dragObserver;
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
                        EndDrag(dragObjectTransform);
                    }
                }
             }
            else
            {
                DragObserver.NotDragging();
            }
        }

        public void StartDrag(Transform dragObjectTransform)
        {
            IsDragging = false;
            IsDragDelay = true;
            DragTime = 0;
            this.dragObjectTransform = dragObjectTransform;
            Vector3 dist = camera.WorldToScreenPoint(dragObjectTransform.position);
            deltaPosX = Input.mousePosition.x - dist.x;
            deltaPosY = Input.mousePosition.y - dist.y;
            posBeforeDrag = dragObjectTransform.localPosition;
            DragObserver.StartDrag(dragObjectTransform);
        }

        public void Dragging(Transform dragObjectTransform)
        {
            IsDraggingBeforeDelay = true;
           
            if (!IsDragDelay)
            {
                IsDragging = true;
                Vector3 curPos = new Vector3(Input.mousePosition.x - deltaPosX, Input.mousePosition.y - deltaPosY, 0);
                Vector3 worldPos = camera.ScreenToWorldPoint(curPos);
                worldPos.z = 0;
                worldPos.x -= Map.GridSystem.GRID_X_OFFSET;
                dragObjectTransform.localPosition = Vector3.Lerp(dragObjectTransform.localPosition, worldPos, Time.deltaTime * DRAG_FOLLOW_SPEED);
                DragObserver.Dragging(dragObjectTransform, worldPos.x, worldPos.y);
            }
        }

        private void EndDrag(Transform dragObjectTransform)
        {
            IsDragging = false;
            dragObjectTransform.localPosition = posBeforeDrag;
            DragObserver.EndDrag();
        }
    }
}
