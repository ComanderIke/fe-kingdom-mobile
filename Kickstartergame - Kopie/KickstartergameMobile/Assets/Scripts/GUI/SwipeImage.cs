using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeImage : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    const float DRAG_FOLLOW_SPEED = 13;

    public bool IsDragging { get; set; }
    public float DragTime { get; set; }
    public bool IsDragDelay { get; set; }
    [SerializeField]
    private RectTransform mask;
    [SerializeField]
    private GameObject halfSwipe;
    [SerializeField]
    private Image colorImage;
    [SerializeField]
    private GameObject fullSwipe;
    [SerializeField]
    private GameObject arrowsLeft;
    [SerializeField]
    private GameObject arrowsRight;
    [SerializeField]
    private GameObject textGO;
    [SerializeField]
    private GameObject gradientRight;
    [SerializeField]
    private GameObject gradientLeft;
    [SerializeField]
    private Image idleImage;
    [SerializeField]
    private GameObject idleObject;
    [SerializeField]
    private Color halfSwipeColor;
    [SerializeField]
    private Color fullSwipeColor;
    [SerializeField]
    public UnityEvent halfSwipeEvent;
    [SerializeField]
    public UnityEvent halfSwipePreviewEvent;
    [SerializeField]
    public UnityEvent fullSwipeEvent;
    [SerializeField]
    public UnityEvent fullSwipePreviewEvent;
    [SerializeField]
    public UnityEvent noSwipeEvent;



    private float maskStartWidth;
    /*DragOffset*/
    private float startPosX;
    private float startPosY;
    private float oldPosX;
    private bool isActive = true;
    private float maskStartSizeX = 0;

    private Vector3 posBeforeDrag;

    public void Start()
    {
        IsDragging = false;
        IsDragDelay = true;
        maskStartWidth = mask.sizeDelta.x;

    }
    void OnEnable()
    {
        gameObject.transform.localRotation = Quaternion.identity;
        maskStartWidth = mask.sizeDelta.x;
        mask.sizeDelta = new Vector2(0, mask.sizeDelta.y);
        isActive = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Dragging();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDrag();
    }

    public void Update()
    {
        if (!IsDragging)
        {
           // mask.sizeDelta = Vector2.Lerp(mask.sizeDelta, new Vector2(0, mask.sizeDelta.y), 4f * Time.deltaTime);
        }
        if (!isActive)
            return;
        if (IsDragging)
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                DragTime += Time.deltaTime;
            }
            else
            {
                IsDragging = false;
            }
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                IsDragging = false;

                EndDrag();
            }
            if (mask.sizeDelta.x <= -700)
            {
                if (fullSwipePreviewEvent != null)
                    fullSwipePreviewEvent.Invoke();
            }
            else if (mask.sizeDelta.x <= -550)
            {
                if(halfSwipePreviewEvent!=null)
                    halfSwipePreviewEvent.Invoke();
            }
            else
            {
                if (noSwipeEvent != null)
                {
                    noSwipeEvent.Invoke();
                }
            }
        }
        
        if (!IsDragging)
        {
            if (mask.sizeDelta.x <= -675)
            {
               // StartCoroutine(DelaySwipeStrongActivation(0.0f));
                //isActive = false;
            }
            else if(mask.sizeDelta.x <= -525)
            {
                //StartCoroutine(DelaySwipeFastActivation(0.0f));
               // isActive = false;
            }
           
        }
        UpdateVisuals();
    }
    void UpdateVisuals()
    {
        if(mask.sizeDelta.x < -800)
        {
            colorImage.color = fullSwipeColor;
            if (fullSwipe)
                fullSwipe.GetComponent<CanvasGroup>().alpha = 1;
            if (halfSwipe && fullSwipe)
                halfSwipe.GetComponent<CanvasGroup>().alpha = 0;

            idleObject.gameObject.SetActive(false);
            arrowsLeft.GetComponent<CanvasGroup>().alpha = 1;
            arrowsRight.GetComponent<CanvasGroup>().alpha = 1;
            if (textGO)
                textGO.GetComponent<CanvasGroup>().alpha = 1;
        }
        else if (mask.sizeDelta.x <= -700)
        {
            if (fullSwipe)
                fullSwipe.GetComponent<CanvasGroup>().alpha = 1;// MathUtility.MapValues(mask.sizeDelta.x, -700, -750, 0, 1);
            if (halfSwipe && fullSwipe)
                halfSwipe.GetComponent<CanvasGroup>().alpha = 0;// 1-MathUtility.MapValues(mask.sizeDelta.x, -650, -700, 0, 1);

            idleObject.gameObject.SetActive(false);
            colorImage.color = Color.Lerp(halfSwipeColor, fullSwipeColor, MathUtility.MapValues(mask.sizeDelta.x, -600, -700, 0, 1));
            if(textGO)
                textGO.GetComponent<CanvasGroup>().alpha = 1;
            arrowsLeft.GetComponent<CanvasGroup>().alpha = 1;
            arrowsRight.GetComponent<CanvasGroup>().alpha = 1;
        }
        else if(mask.sizeDelta.x <-8f)
        {
            if (fullSwipe)
                fullSwipe.GetComponent<CanvasGroup>().alpha = 0;
            if (halfSwipe)
                halfSwipe.GetComponent<CanvasGroup>().alpha = MathUtility.MapValues(mask.sizeDelta.x, -550, -600, 0, 1);
            arrowsLeft.GetComponent<CanvasGroup>().alpha = MathUtility.MapValues(mask.sizeDelta.x, -8, -150, 0, 1);
            arrowsRight.GetComponent<CanvasGroup>().alpha = MathUtility.MapValues(mask.sizeDelta.x, -8, -150, 0, 1);
            idleObject.gameObject.SetActive(false);
            if (textGO)
                textGO.GetComponent<CanvasGroup>().alpha =  MathUtility.MapValues(mask.sizeDelta.x, -550, -600, 0, 1);
            //idleImage.color = new Color(0, 0, 0, 1-MathUtility.MapValues(mask.sizeDelta.x, 0, -900, 0, 1));
            colorImage.color = Color.Lerp(Color.black, halfSwipeColor, MathUtility.MapValues(mask.sizeDelta.x,0,-550,0,1));
        }
        else
        {
            if(halfSwipe)
                halfSwipe.GetComponent<CanvasGroup>().alpha = 0;
            if (fullSwipe)
                fullSwipe.GetComponent<CanvasGroup>().alpha = 0;
            idleObject.gameObject.SetActive(true);
            idleImage.color = new Color(0, 0, 0, 1);
            colorImage.color = Color.black;
            arrowsLeft.GetComponent<CanvasGroup>().alpha = 0;
            arrowsRight.GetComponent<CanvasGroup>().alpha = 0;
            if (textGO)
                textGO.GetComponent<CanvasGroup>().alpha = 0;
        }
      

    }
    public void StartDrag()
    {
        IsDragDelay = true;
        DragTime = 0;
        startPosX = Input.mousePosition.x;
        startPosY = Input.mousePosition.y;
        maskStartSizeX = mask.sizeDelta.x;
        //Debug.Log("StartSize" + maskStartSizeX);
        oldPosX = -1;
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        if(maskStartSizeX == 0)
            initDragFlag = true;
    }

    bool initDragFlag = false;
    public void Dragging()
    {
        IsDragging = true;
        if (oldPosX != -1)
        {
           
            float screenX = 0;
            float screenMaxX = startPosX-Input.mousePosition.x;
            float screenMaxOld = oldPosX;
            float delta = screenMaxX - screenX;
            if (initDragFlag)
            {
                if(screenMaxOld < screenMaxX)
                {
                    mask.pivot = new Vector2(0, 0.5f);
                    arrowsLeft.SetActive(true);
                    arrowsRight.SetActive(false);
                    if (gradientLeft != null)
                        gradientLeft.SetActive(true);
                    if (gradientRight != null)
                        gradientRight.SetActive(false);
                    initDragFlag = false;
                }
                else if(screenMaxOld > screenMaxX)
                {
                    mask.pivot = new Vector2(1, 0.5f);
                    initDragFlag = false;
                    arrowsLeft.SetActive(false);
                    arrowsRight.SetActive(true);
                    if(gradientLeft!=null)
                        gradientLeft.SetActive(false);
                    if (gradientRight != null)
                        gradientRight.SetActive(true);
                }
                
            }
            delta *= -1;
            if (mask.pivot.x == 0)
            {
                if (maskStartSizeX +delta >= 0)
                    delta = -maskStartSizeX;
            }
            else if (mask.pivot.x == 1)
            {
                if(delta<maskStartSizeX)
                {
                    delta = -maskStartSizeX;
                }
                else if (delta <= 0)
                {
                    //    delta = 0;
                    delta *= -1;
                }
                else if (delta > 0)
                {
                    delta *= -1;
                }
            }
           
            mask.sizeDelta = new Vector2(maskStartSizeX+delta, mask.sizeDelta.y);
        }
        oldPosX = startPosX - Input.mousePosition.x;

    }

    public void EndDrag()
    {
    }

    IEnumerator DelaySwipeFastActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(halfSwipeEvent!=null)
            halfSwipeEvent.Invoke();
    }

    IEnumerator DelaySwipeStrongActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (fullSwipeEvent != null)
            fullSwipeEvent.Invoke();

    }
}
