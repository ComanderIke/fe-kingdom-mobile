using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class DragManager
    {
        
        const float DRAG_DELAY = 0.15f;
        const float DRAG_FOLLOW_SPEED = 13;
        public static bool IsAnyUnitDragged = false;

        private bool IsDraggingBeforeDelay { get; set; }
        public Dictionary<Transform, bool> IsDragging { get; set; }
        private float DragTime { get; set; }
        private bool IsDragDelay { get; set; }
        private Dictionary<Transform, bool> DragStarted { get; set; }

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
            IsDragging = new Dictionary<Transform, bool>();
            DragStarted = new Dictionary<Transform, bool>();
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

            // if (Input.GetMouseButtonUp(0))
            // {
            //     foreach(var key in DragStarted.Keys)
            //         DragStarted[key] = false;
            //     foreach(var key in IsDragging.Keys)
            //         IsDragging[key] = false;
            // }
        }


        public void AddToDictionary(Transform transform)
        {
            if(!IsDragging.ContainsKey(transform))
                IsDragging.Add(transform,false);
            if(!DragStarted.ContainsKey(transform))
                DragStarted.Add(transform,false);
        }
        public void StartDrag(Transform dragObjectTransform)
        {
            MyDebug.LogInput("Start drag for: "+dragObjectTransform);
            AddToDictionary(dragObjectTransform);
            DragStarted[dragObjectTransform] = true;
            IsDragging[dragObjectTransform] = false;
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
            AddToDictionary(dragObjectTransform);
             if (!DragStarted[dragObjectTransform])
                 return;
            IsDraggingBeforeDelay = true;
           
            if (!IsDragDelay)
            {
                // Debug.Log("DRAGGING FOR: "+dragObjectTransform);
                IsDragging[dragObjectTransform] = true;
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
            AddToDictionary(dragObjectTransform);
            // if (!DragStarted[dragObjectTransform])
            //     return;
            MyDebug.LogInput("END DRAG FOR: "+dragObjectTransform);
            IsDragging[dragObjectTransform] = false;
            if(dragObjectTransform!=null)
                dragObjectTransform.localPosition = posBeforeDrag;
            DragObserver.EndDrag();
            DragStarted[dragObjectTransform] = false;
        }
    }
}
