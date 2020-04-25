using Assets.Core;
using Assets.GameInput;
using Assets.GUI;
using Assets.Mechanics.Dialogs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameActors.Units.OnGameObject
{
    public class UnitController : MonoBehaviour, IDragAble
    {
        public delegate void OnAnimationEndedEvent();

        public OnAnimationEndedEvent OnAnimationEnded;
        public delegate void OnAttackAnimationConnectedEvent();

        public OnAttackAnimationConnectedEvent OnAttackAnimationConnected;
        public static bool LockInput = true;
        private bool dragInitiated;
        private bool dragStarted;
        private bool doubleClick;
        [SerializeField] private StatsBarOnMap hpBar;
        [SerializeField] private StatsBarOnMap spBar;
        [SerializeField] private ExpBarController expBar;
        private float timerForDoubleClick;
        private const float DOUBLE_CLICK_TIME = 0.4f;
        [SerializeField] private Animator animator;
        public Unit Unit;
        public DragManager DragManager { get; set; }
        public RaycastManager RaycastManager { get; set; }

        private void Start()
        {
            DragManager = new DragManager(this);
            RaycastManager = new RaycastManager();
            Unit.HpValueChanged += HpValueChanged;
            Unit.SpValueChanged += SpValueChanged;
            Unit.ExperienceManager.OnExpGained += ExpValueChanged;
            Unit.UnitWaiting += SetWaitingSprite;
           
            HpValueChanged();
            SpValueChanged();
        }
        void Destroy()
        {
            Unit.HpValueChanged -= HpValueChanged;
            Unit.SpValueChanged -= SpValueChanged;
            Unit.ExperienceManager.OnExpGained -= ExpValueChanged;
            Unit.UnitWaiting -= SetWaitingSprite;
        }

        private void Update()
        {
            if (LockInput)
                return;
            DragManager.Update();
        }

        private void HpValueChanged()
        {
            if (hpBar != null && Unit != null)
                hpBar.SetHealth(Unit.Hp, Unit.Stats.MaxHp);
        }

        private void SpValueChanged()
        {
            if (spBar != null && Unit != null)
                spBar.SetHealth(Unit.Sp, Unit.Stats.MaxSp);
        }
        private void ExpValueChanged(int currentExp, int expGained)
        {
            if (expBar != null && Unit != null)
            {
                AnimationQueue.Add(()=>expBar.Show(currentExp, expGained));
                
            }
        }

        #region Renderer

        private void SetWaitingSprite(Unit unit, bool waiting)
        {
            if (unit == Unit)
            {
                GetComponentInChildren<SpriteRenderer>().color = !waiting ? Color.white : Color.grey;
            }
        }

        #endregion

        #region MouseInteraction

        private void OnMouseEnter()
        {
            if (!EventSystem.current.IsPointerOverGameObject()) InputSystem.OnDraggedOverUnit(Unit);
        }

        private void OnMouseExit()
        {
            if (DragManager.IsAnyUnitDragged) GridGameManager.Instance.GetSystem<InputSystem>().DraggedExit();
        }

        private void OnMouseDrag()
        {
            if (LockInput)
                return;
            if (!EventSystem.current.IsPointerOverGameObject() && (dragStarted || dragInitiated))
            {
                dragStarted = false;
                dragInitiated = true;
                if (Unit.UnitTurnState.IsDragable()) DragManager.Dragging();
            }
        }

        private bool unitSelectedBeforeClicking = false;
        private void OnMouseDown()
        {

            dragStarted = false;
            dragInitiated = false;
            doubleClick = false;
            
            if (!Unit.IsAlive())
                return;
            if (LockInput)
                return;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log(Time.time-timerForDoubleClick);
                if (timerForDoubleClick != 0 &&  Time.time - timerForDoubleClick < DOUBLE_CLICK_TIME)
                {
                   
                    timerForDoubleClick = 0;
                    unitSelectedBeforeClicking = Unit.UnitTurnState.Selected;
                    doubleClick = true;
                }
                else
                {
                    timerForDoubleClick = Time.time;
                    DragManager.StartDrag();
                }

                
            }
        }
        private void OnMouseUp()
        {
            
            if (DragManager.IsDragging)
            {
                DragManager.Update();// Update Dragmanager because he should notice first when MouseUp happens
                dragStarted = false;
                dragInitiated = false;
                Unit.UnitShowActiveEffect(Unit, true, false);
                Debug.Log("ENDDRAGUNIT");
               
                InputSystem.OnEndDrag();
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            else if (unitSelectedBeforeClicking)
            {
                Debug.Log(doubleClick);
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if(doubleClick)
                        InputSystem.OnUnitDoubleClicked(Unit);
                    else
                        InputSystem.OnUnitClicked(Unit);
                }
            }

            if (doubleClick)
            {
                doubleClick = false;
                Debug.Log("DOUBLE CLICK");
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
            dragStarted = true;
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            InputSystem.OnStartDrag((int) gridPos.x, (int) gridPos.y);
            //Debug.Log("STARTDRAG");
            unitSelectedBeforeClicking = Unit.UnitTurnState.Selected;
            if (!Unit.UnitTurnState.Selected)//If unit is already selected wait for MouseUp/DragEnd to invoke OnUnitClicked
                if (!EventSystem.current.IsPointerOverGameObject()) InputSystem.OnUnitClicked(Unit);
        }

        public void Dragging()
        {
            //Debug.Log("DRAGGING");
            Unit.UnitShowActiveEffect(Unit, false, true);
            GetComponent<BoxCollider>().enabled = false;
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
                InputSystem.OnUnitDragged((int) gridPos.x, (int) gridPos.y, Unit);
        }

        public void EndDrag()
        {
            //dragStarted = false;
            //dragInitiated = false;
            //Unit.UnitShowActiveEffect(Unit, true, false);
            //InputSystem.OnEndDrag();
            ////FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
            //gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        public void NotDragging()
        {
        }

        #endregion

        #region Animations

        public void AttackConnected()
        {
            OnAttackAnimationConnected?.Invoke();
            Debug.Log("Attack Connected!");
        }
        public void AnimationEnded()
        {
            OnAnimationEnded?.Invoke();
            Debug.Log("Attack Finished!");
        }

        public void BattleAnimationUp()
        {
            animator.SetTrigger("BattleAnimationUp");
        }
        public void BattleAnimationDown()
        {
            animator.SetTrigger("BattleAnimationDown");
        }
        public void BattleAnimationLeft()
        {
            animator.SetTrigger("BattleAnimationLeft");
        }
        public void BattleAnimationRight()
        {
            animator.SetTrigger("BattleAnimationRight");
        }
        public void BattleAnimationDownLeft()
        {
            animator.SetTrigger("BattleAnimationDownLeft");
        }
        public void BattleAnimationDownRight()
        {
            animator.SetTrigger("BattleAnimationDownRight");
        }
        public void BattleAnimationUpLeft()
        {
            animator.SetTrigger("BattleAnimationUpLeft");
        }
        public void BattleAnimationUpRight()
        {
            animator.SetTrigger("BattleAnimationUpRight");
        }
        #endregion
    }
}