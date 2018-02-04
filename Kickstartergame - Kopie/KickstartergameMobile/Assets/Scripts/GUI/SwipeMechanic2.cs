using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeMechanic2 : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    const float DRAG_FOLLOW_SPEED = 13;

    public bool IsDragging { get; set; }
    public float DragTime { get; set; }
    public bool IsDragDelay { get; set; }
    [SerializeField]
    private RectTransform mask;
    [SerializeField]
    private GameObject fastAttack;
    [SerializeField]
    private Image colorImage;
    [SerializeField]
    private GameObject strongAttack;
    [SerializeField]
    private GameObject arrowsLeft;
    [SerializeField]
    private GameObject arrowsRight;
    [SerializeField]
    private Image idleImage;
    [SerializeField]
    private GameObject idleObject;
    [SerializeField]
    private Color fastAttackColor;
    [SerializeField]
    private Color strongAttackColor;
    [SerializeField]
    public UnityEvent swipeEvent;


    private float maskStartWidth;
    /*DragOffset*/
    private float startPosX;
    private float startPosY;
    private float oldPosX;
    private bool isActive = true;

    private Vector3 posBeforeDrag;
    private Quaternion rotationBeforeDrag;

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
        }
        else
        {
            mask.sizeDelta = Vector2.Lerp(mask.sizeDelta, new Vector2(0, mask.sizeDelta.y), 4f * Time.deltaTime);
        }
        if (!IsDragging)
        {
            if (mask.sizeDelta.x >= 650)
            {
                StartCoroutine(DelaySwipeLeftActivation(0.0f));
                isActive = false;
            }
            else if (mask.sizeDelta.x <= -650)
            {
                StartCoroutine(DelaySwipeRightActivation(0.0f));
                isActive = false;
            }
           
        }
        Clamp();
        UpdateVisuals();
    }
    void Clamp()
    {
        if (gameObject.transform.localEulerAngles.z >= 90 && gameObject.transform.localEulerAngles.z < 180)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else if (gameObject.transform.localEulerAngles.z <= 270 && gameObject.transform.localEulerAngles.z > 180)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
        }
    }
    void UpdateVisuals()
    {
        if(mask.sizeDelta.x < -750)
        {
            colorImage.color = strongAttackColor;
        }
        else if (mask.sizeDelta.x < -600)
        {
            if (strongAttack)
                strongAttack.GetComponent<CanvasGroup>().alpha = MathUtility.MapValues(mask.sizeDelta.x, -700, -750, 0, 1);
            if (fastAttack)
                fastAttack.GetComponent<CanvasGroup>().alpha = 1-MathUtility.MapValues(mask.sizeDelta.x, -700, -750, 0, 1);
            arrowsLeft.GetComponent<CanvasGroup>().alpha = 0;
            arrowsRight.GetComponent<CanvasGroup>().alpha = 0;
            colorImage.color = Color.Lerp(fastAttackColor, strongAttackColor, MathUtility.MapValues(mask.sizeDelta.x, -600, -750, 0, 1));
        }
        else if(mask.sizeDelta.x <-8f)
        {
            if (fastAttack)
                fastAttack.GetComponent<CanvasGroup>().alpha = MathUtility.MapValues(mask.sizeDelta.x, -550, -600, 0, 1);
            arrowsLeft.GetComponent<CanvasGroup>().alpha = 1-MathUtility.MapValues(mask.sizeDelta.x, -450, -550, 0, 1);
            arrowsRight.GetComponent<CanvasGroup>().alpha = 1-MathUtility.MapValues(mask.sizeDelta.x, -450, -550, 0, 1);
            idleObject.gameObject.SetActive(false);
            //idleImage.color = new Color(0, 0, 0, 1-MathUtility.MapValues(mask.sizeDelta.x, 0, -900, 0, 1));
            colorImage.color = Color.Lerp(Color.black, fastAttackColor, MathUtility.MapValues(mask.sizeDelta.x,0,-600,0,1));
        }
        else
        {
            if(fastAttack)
                fastAttack.GetComponent<CanvasGroup>().alpha = 0;
            if (strongAttack)
                strongAttack.GetComponent<CanvasGroup>().alpha = 0;
            idleObject.gameObject.SetActive(true);
            idleImage.color = new Color(0, 0, 0, 1);
            colorImage.color = Color.black;
            arrowsLeft.GetComponent<CanvasGroup>().alpha = 1;
            arrowsRight.GetComponent<CanvasGroup>().alpha = 1;
        }
      

    }
    public void StartDrag()
    {
        IsDragDelay = true;
        DragTime = 0;
        startPosX = Input.mousePosition.x;
        startPosY = Input.mousePosition.y;
        oldPosX = -1;
        rotationBeforeDrag = gameObject.transform.localRotation;
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
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
                    initDragFlag = false;
                }
                else if(screenMaxOld > screenMaxX)
                {
                    mask.pivot = new Vector2(1, 0.5f);
                    initDragFlag = false;
                    arrowsLeft.SetActive(false);
                    arrowsRight.SetActive(true);
                }
                
            }
            delta *= -1;
            if (mask.pivot.x == 0)
            {
                if (delta >= 0)
                    delta = 0;
            }
            else if (mask.pivot.x == 1)
            {
                if (delta <= 0)
                    delta = 0;
                if (delta > 0)
                {
                    delta *= -1;
                }
            }
           
            mask.sizeDelta = new Vector2(delta, mask.sizeDelta.y);
        }
        oldPosX = startPosX - Input.mousePosition.x;

    }

    public void EndDrag()
    {
        //TODO Force cant activate this

        /* Vector3 diff = UnityEngine.Input.mousePosition - new Vector3(startPosX, startPosY, 0);
         float force = diff.magnitude / DragTime/30.0f;
         if (diff.x > 0)
             force = -force;
         Debug.Log(force);
         gameObject.GetComponent<Rigidbody2D>().AddTorque(force, ForceMode2D.Force);*/
    }
    IEnumerator DelaySwipeRightActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(swipeEvent!=null)
            swipeEvent.Invoke();
        if (EventContainer.swipeRightConfirmedEvent != null)
            EventContainer.swipeRightConfirmedEvent();
    }
    IEnumerator DelaySwipeLeftActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (swipeEvent != null)
            swipeEvent.Invoke();
        if(EventContainer.swipeLeftConfirmedEvent!=null)
            EventContainer.swipeLeftConfirmedEvent();
    }
}
