using UnityEngine;
using Assets.Scripts.GameStates;
using UnityEngine.EventSystems;
using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Input;

public class MovableObject :  MonoBehaviour, DragAble {

    public static bool lockInput = false;
    public LivingObject Unit { get; set; }
    public DragManager DragManager { get; set; }
    public RaycastManager RaycastManager { get; set; }

    private void Start()
    {
        DragManager = new DragManager(this);
        RaycastManager = new RaycastManager();
    }

    void Update () {
        if (lockInput)
            return;
        DragManager.Update();
	}

    void OnMouseEnter(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {

            EventContainer.draggedOverUnit(Unit);
        }
    }

    void OnMouseExit(){
        if (DragManager.IsAnyUnitDragged)
        {
            MainScript.GetInstance().GetSystem<MouseManager>().DraggedExit();
        }
    }
     
    void OnMouseDrag()
    {
        if (lockInput)
            return;
        if (Unit.UnitTurnState.IsDragable())
        {
            DragManager.Dragging();
        }
    }

    void OnMouseDown()
    {
        if (!Unit.IsAlive())
            return;
        if (lockInput)
            return;
        
        DragManager.StartDrag();
    }

    #region INTERFACE DragAble
    public Transform GetTransform()
    {
        return transform;
    }

    public void StartDrag()
    {
        Vector2 gridPos = RaycastManager.GetMousePositionOnGrid();
        EventContainer.startDrag((int)gridPos.x, (int)gridPos.y);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EventContainer.unitClicked(Unit);
        }
    }

    public void Dragging()
    {
        GetComponent<BoxCollider>().enabled = false;
        Vector2 gridPos = RaycastManager.GetMousePositionOnGrid();
        EventContainer.unitDragged((int)gridPos.x, (int)gridPos.y, Unit);
    }

    public void EndDrag()
    {
        EventContainer.endDrag();
        FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;

       
    }

    public void NotDragging()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    #endregion
}
