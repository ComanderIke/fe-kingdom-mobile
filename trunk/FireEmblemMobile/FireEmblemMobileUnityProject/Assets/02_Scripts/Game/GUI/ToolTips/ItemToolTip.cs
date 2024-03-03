using Game.GameActors.Units;
using Game.GUI.Convoy;
using Game.GUI.EncounterUI.Merchant;
using TMPro;
using UnityEngine;

namespace Game.GUI.ToolTips
{
    [ExecuteInEditMode]
    public class ItemToolTip : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI descriptionText;
        public UIConvoyItemController itemIcon;
  
        [SerializeField]
        private RectTransform rectTransform;
        private Unit itemOwner;
        [SerializeField] private Vector3 offset;
    
    
        // Start is called before the first frame update
  

        // Update is called once per frame
        private void Update()
        {
            if (Application.isEditor)
            {
                UpdateTextWrap(transform.position);
            }
        }
        void ClampOnScreen()
        {
            var canvasRect = canvas.transform as RectTransform;
            if (rectTransform.anchoredPosition.x +rectTransform.rect.width> canvasRect.rect.width)
            {
                rectTransform.anchoredPosition = new Vector2(canvasRect.rect.width - rectTransform.rect.width,rectTransform.anchoredPosition.y);
            }
            if (rectTransform.anchoredPosition.y +rectTransform.rect.height> canvasRect.rect.height)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, canvasRect.rect.height - rectTransform.rect.height);
            }
            if (rectTransform.anchoredPosition.x -rectTransform.pivot.x*rectTransform.rect.width< 0)
            {
                rectTransform.anchoredPosition = new Vector2( -rectTransform.pivot.x*rectTransform.rect.width,rectTransform.anchoredPosition.y);
            }
            if (rectTransform.anchoredPosition.y-rectTransform.pivot.y*rectTransform.rect.height< 0)
            {
                rectTransform.anchoredPosition = new Vector2( rectTransform.anchoredPosition.x,-rectTransform.pivot.y*rectTransform.rect.height);
            }
        }

        void UpdateTextWrap(Vector3 position)
        {
            ClampOnScreen();
            // if(rectTransform==null)
            //     rectTransform = GetComponent<RectTransform>();
            // float pivotX = 0;//position.x / Screen.width;
            // float pivotY = position.y / Screen.height;
            // rectTransform.pivot = new Vector2(pivotX, pivotY);
        }

        public void ExitClicked()
        {
            gameObject.SetActive(false);
        }
        public void SetValues(StockedItem item, Vector3 position, bool exactPos=false)
        {
            headerText.text = item.item.Name;
            descriptionText.text = item.item.Description;
            MyDebug.LogTest("Tooltip Position before Offset: "+position);
            if(itemIcon!=null)
                itemIcon.SetValues(item, 0, false, false, false);
            rectTransform.anchoredPosition = position+ new Vector3(offset.x,exactPos?0:offset.y,0);
            UpdateTextWrap(position);
        }
    }
}
