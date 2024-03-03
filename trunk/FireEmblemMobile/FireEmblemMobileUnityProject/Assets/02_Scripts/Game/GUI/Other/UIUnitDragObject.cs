using Game.GameActors.Units;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.GUI.Other
{
    public class UIUnitDragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
    

        public Image UnitSpriteRenderer;
        public Color normalColor;
        public Color normalColorBG2;
        public Color selectedColor;
        public Color selectedColorBG2;
        public Image BackGroundRenderer;
        public Image BackGround2;
        public RectTransform rectTransform;
        private CanvasGroup canvasGroup;

        [FormerlySerializedAs("unitBp")] public Unit unit;
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
            if(BackGround2!=null)
                BackGround2.color = selectedColorBG2;
        }

        public void HideSelected()
        {
            BackGroundRenderer.color = normalColor;
            if(BackGround2!=null)
                BackGround2.color = normalColorBG2;
        }
    }
}
