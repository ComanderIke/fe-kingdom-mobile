using UnityEngine;
using Assets.Scripts.GameStates;
using UnityEngine.EventSystems;
using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Input;

public class UnitController :  MonoBehaviour, DragAble {

    public static bool lockInput = true;
    public LivingObject Unit { get; set; }
    public DragManager DragManager { get; set; }
    public RaycastManager RaycastManager { get; set; }
    private HPBarOnMap hpBar;

    private void Start()
    {
        DragManager = new DragManager(this);
        RaycastManager = new RaycastManager();
        EventContainer.hpValueChanged += HPValueChanged;
        EventContainer.unitWaiting+= SetWaitingSprite;
        hpBar = GetComponentInChildren<HPBarOnMap>();
        HPValueChanged();
    }

    void Update () {
        if (lockInput)
            return;
        DragManager.Update();
	}
    void HPValueChanged()
    {
        if(hpBar!=null&&Unit!=null)
            hpBar.SetHealth(Unit.Stats.HP, Unit.Stats.MaxHP);
    }

    #region Renderer
    void SetWaitingSprite(LivingObject unit, bool waiting)
    {
        if (unit == Unit)
        {
            if (!waiting)
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
            else
                GetComponentInChildren<SpriteRenderer>().color = Color.grey;
        }
    }
    #endregion

    #region MouseInteraction
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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Unit.UnitTurnState.IsDragable())
            {

                DragManager.Dragging();
            }
        }
    }

    void OnMouseDown()
    {
        if (!Unit.IsAlive())
            return;
        if (lockInput)
            return;
       
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            DragManager.StartDrag();
        }
    }
    #endregion

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
        EventContainer.unitShowActiveEffect(Unit, false, true);
        GetComponent<BoxCollider>().enabled = false;
        Vector2 gridPos = RaycastManager.GetMousePositionOnGrid();
        EventContainer.unitDragged((int)gridPos.x, (int)gridPos.y, Unit);
    }

    public void EndDrag()
    {
        EventContainer.unitShowActiveEffect(Unit, true, false);
        EventContainer.endDrag();
        FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;


    }

    public void NotDragging()
    {
        
    }
    #endregion
}
