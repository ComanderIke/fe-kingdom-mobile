using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Characters;
using Assets.Scripts.Input;

public class UnitController :  MonoBehaviour, DragAble {

    public static bool lockInput = true;

    public Unit Unit;
    public DragManager DragManager { get; set; }
    public RaycastManager RaycastManager { get; set; }
    public SpeechBubble SpeechBubble { get; set; }
    
    private StatsBarOnMap hpBar;
    private StatsBarOnMap spBar;

    private void Start()
    {
       
        DragManager = new DragManager(this);
        RaycastManager = new RaycastManager();
        SpeechBubble = GetComponentInChildren<SpeechBubble>();
        if(SpeechBubble)
            this.SpeechBubble.gameObject.SetActive(false);
        Unit.onHpValueChanged += HPValueChanged;
        Unit.onSpValueChanged += SPValueChanged;
        Unit.onUnitWaiting+= SetWaitingSprite;
        hpBar = GetComponentsInChildren<StatsBarOnMap>()[0];
        spBar = GetComponentsInChildren<StatsBarOnMap>()[1];
        HPValueChanged();
        SPValueChanged();
    }

    void Update () {
        if (lockInput)
            return;
        DragManager.Update();
	}
    void HPValueChanged()
    {
        if(hpBar!=null&&Unit!=null)
            hpBar.SetHealth(Unit.HP, Unit.Stats.MaxHP);
    }
    void SPValueChanged()
    {
        if (spBar != null && Unit != null)
            spBar.SetHealth(Unit.SP, Unit.Stats.MaxSP);
    }

    #region Renderer
    void SetWaitingSprite(Unit unit, bool waiting)
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

            InputSystem.onDraggedOverUnit(Unit);
        }
    }

    void OnMouseExit(){
        if (DragManager.IsAnyUnitDragged)
        {
            MainScript.instance.GetSystem<InputSystem>().DraggedExit();
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
        InputSystem.onStartDrag((int)gridPos.x, (int)gridPos.y);
        
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            InputSystem.onUnitClicked(Unit);
        }
    }

    public void Dragging()
    {
        Unit.onUnitShowActiveEffect(Unit, false, true);
        GetComponent<BoxCollider>().enabled = false;
        Vector2 gridPos = RaycastManager.GetMousePositionOnGrid();
        InputSystem.onUnitDragged((int)gridPos.x, (int)gridPos.y, Unit);
    }

    public void EndDrag()
    {
        Unit.onUnitShowActiveEffect(Unit, true, false);
        InputSystem.onEndDrag();
        //FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;


    }

    public void NotDragging()
    {
        
    }
    #endregion
}
