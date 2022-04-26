using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.GUI
{
    public class UIUnitDragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
    

        public Image UnitSpriteRenderer;
        public Color normalColor;
        public Color selectedColor;
        public Image BackGroundRenderer;
        public RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        public Unit unit;
        public UnitSelectionUI UnitSelectionUI
        {
            get;
            set;
        }
    
        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetUnitSprite(Sprite sprite)
        {
            UnitSpriteRenderer.sprite = sprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
        
            //UnitPlacement.OnDrag(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //canvasGroup.blocksRaycasts = false;
            //UnitPlacement.OnBeginDrag(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // UnitPlacement.OnEndDrag(this, eventData);
            //canvasGroup.blocksRaycasts = true;
        }

        public void Clicked()
        {
            UnitSelectionUI.UnitClicked(this);
        }

        public void SetUnit(Unit unit)
        {
            this.unit = unit;
            SetUnitSprite(unit.visuals.CharacterSpriteSet.MapSprite);
        }

        public void ShowSelected()
        {
            BackGroundRenderer.color = selectedColor;
        }

        public void HideSelected()
        {
            BackGroundRenderer.color = normalColor;
        }
    }
}
