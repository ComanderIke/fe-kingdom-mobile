using System;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GUI.Controller
{
    public class EncounterPlayerUnitController : MonoBehaviour
    {

        [FormerlySerializedAs("unitBp")] [FormerlySerializedAs("Unit")] public Unit unit;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private UnitAnimator unitAnimator;

        public int baseSortOrder = 10;

        public float speed = 2f;

        private Transform follow;

        public float baseOffset = 0.0f;

        private float offset;

        public event Action<EncounterPlayerUnitController> onClicked;
        // Start is called before the first frame update
        void Start()
        {
        
        }
    
        public void Clicked()
        {

            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                onClicked?.Invoke(this);
                //     FindObjectOfType<UICharacterViewController>().Show(Unit);
            }
        }
        private static readonly int Moving = Animator.StringToHash("Moving");
        // Update is called once per frame
        void Update()
        {

            if (offset == 0)
                offset = baseOffset;
            if (follow != null && Vector2.Distance(transform.position, follow.position) > offset)
            {
                Debug.Log("Distance: "+Vector2.Distance(transform.position, follow.position)+" "+transform.position+" "+follow.position);
                transform.position = Vector2.MoveTowards(transform.position, follow.position, speed * Time.deltaTime);
            }
        
          
        }

        public void SetTarget(Transform target)
        {
            follow = target;
        }

    
        public void SetUnit(Unit unit)
        {
            this.unit = unit;
            unitAnimator.SetUnit(unit);
        }
        public void SetSortOrder(int order)
        {
            spriteRenderer.sortingOrder =baseSortOrder+ order;
        }
        // public void SetActiveUnit(bool active)
        // {
        //     Debug.Log("Show Active Unit Effect On direct Unit?");
        //     }

        public void SetOffsetCount(int cnt)
        {
            this.offset = baseOffset * cnt;
      
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
