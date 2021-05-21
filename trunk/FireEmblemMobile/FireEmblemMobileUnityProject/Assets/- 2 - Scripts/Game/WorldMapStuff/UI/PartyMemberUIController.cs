using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyMemberUIController:MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private CanvasGroup canvasGroup;
    public TextMeshProUGUI nameText;
    public Image image;
    public Image BackgroundImage;
    public Sprite selectedSprite;
    public Sprite normalSprite;
    private bool selected;
    private RectTransform rectTransform;
    private Transform saveParent;
    public int saveSiblingIndex;

    private void Awake()
    {
        saveParent = transform.parent;
        saveSiblingIndex = transform.GetSiblingIndex();
        GameObject tmp = gameObject;
        while (canvas == null)
        {
            canvas = tmp.GetComponentInParent<Canvas>();
            tmp = tmp.transform.parent.gameObject;
        }
        
        
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string memberName)
    {
        nameText.SetText(memberName);
    }

    public void SetSprite(Sprite mapSprite)
    {
        image.sprite = mapSprite;
    }

    public void Clicked()
    {
        selected = !selected;
        BackgroundImage.sprite = selected?selectedSprite:normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }

    private bool noParent = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        transform.SetParent(canvas.transform);
        noParent = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (noParent)
        {
            transform.SetParent(saveParent.transform);
            transform.SetSiblingIndex(saveSiblingIndex);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
       
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            
            var partyUI=eventData.pointerDrag.gameObject.GetComponent<PartyMemberUIController>();
            if (partyUI != null)
            {
              //  Debug.Log("SWAP"+partyUI.gameObject.name+" "+this.gameObject.name);
                var tmpTransform = partyUI.saveParent;
                partyUI.UpdateParent(transform.parent, saveSiblingIndex);
                //this.transform.SetParent(tmpTransform);
                UpdateParent(tmpTransform, partyUI.saveSiblingIndex);
            }
               
        }
    }

    public void UpdateParent(Transform parent, int siblingIndex)
    {
        transform.SetParent(parent);
        this.transform.SetSiblingIndex(siblingIndex);
        saveParent = parent;
        saveSiblingIndex = transform.GetSiblingIndex();
        noParent = false;
    }
    public void UpdateParent(Transform parent)
    {
        transform.SetParent(parent);
        saveParent = parent;
        saveSiblingIndex = transform.GetSiblingIndex();
        noParent = false;
    }
}