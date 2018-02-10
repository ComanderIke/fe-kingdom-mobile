using Assets.Scripts.Events;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeMechanic : MonoBehaviour , IPointerDownHandler, IDragHandler
{

    const float DRAG_FOLLOW_SPEED = 13;

    public bool IsDragging { get; set; }
    public float DragTime { get; set; }
    public bool IsDragDelay { get; set; }
    /*DragOffset*/
    private float startPosX;
    private float startPosY;
    private float oldPosX;

    private Vector3 posBeforeDrag;
    private Quaternion rotationBeforeDrag;

    public void Start()
    {
        IsDragging = false;
        IsDragDelay = true;
    }
    void OnEnable()
    {
        gameObject.transform.localRotation = Quaternion.identity;
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
            
            if (gameObject.GetComponent<Rigidbody2D>().angularVelocity < 1f&& gameObject.GetComponent<Rigidbody2D>().angularVelocity > -1f)
            {

                gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
                gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, Quaternion.identity, 4.0f*Time.deltaTime);
            }
        }
        if (gameObject.transform.localEulerAngles.z >= 70 && gameObject.transform.localEulerAngles.z < 180)
        {
            //EventContainer.swipeLeftEvent();
        }
        else if (gameObject.transform.localEulerAngles.z <= 290 && gameObject.transform.localEulerAngles.z > 180)
        {
            //EventContainer.swipeRightEvent();
        }
        else if(gameObject.transform.localEulerAngles.z < 70|| gameObject.transform.localEulerAngles.z > 290)
        {
            //TODO Performance
            EventContainer.swipeIdleEvent();
        }
        if (!IsDragging)
        {
            if (gameObject.transform.localEulerAngles.z >= 70 && gameObject.transform.localEulerAngles.z < 180)
            {
                StartCoroutine(DelaySwipeLeftActivation(0.15f));

            }
            else if (gameObject.transform.localEulerAngles.z <= 290 && gameObject.transform.localEulerAngles.z > 180)
            {
                StartCoroutine(DelaySwipeRightActivation(0.15f));
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
        Color color = Color.black;
        if (gameObject.transform.localEulerAngles.z <= 90 && gameObject.transform.localEulerAngles.z >= 0)
            color.g=MathUtility.MapValues(gameObject.transform.localEulerAngles.z, 0, 90, 0, 1);
        if (gameObject.transform.localEulerAngles.z >= 270 && gameObject.transform.localEulerAngles.z <= 360)
            color.r=1-MathUtility.MapValues(gameObject.transform.localEulerAngles.z, 270, 360, 0, 1);
        GetComponent<Image>().color = color;

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
    }

    public void Dragging()
    {
        IsDragging = true;
        //Vector3 delta = new Vector3(UnityEngine.Input.mousePosition.x - startPosX, UnityEngine.Input.mousePosition.y - startPosY, 0);
        //gameObject.transform.Rotate(Vector3.forward, 2.0f*Time.deltaTime*-delta.x);
        if (oldPosX != -1)
        {
            float delta = Input.mousePosition.x - oldPosX;
            gameObject.transform.Rotate(Vector3.forward, 10.0f * Time.deltaTime * -delta);
        }
        oldPosX = Input.mousePosition.x;

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
        EventContainer.swipeRightConfirmedEvent();
    }
    IEnumerator DelaySwipeLeftActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        EventContainer.swipeLeftConfirmedEvent();
    }
}
