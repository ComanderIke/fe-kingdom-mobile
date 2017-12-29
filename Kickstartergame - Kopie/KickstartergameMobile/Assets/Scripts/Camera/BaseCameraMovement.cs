using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Events;

public class BaseCameraMovement : MonoBehaviour {

    const float LERP_SPEED = 0.1f;
    const float DRAG_SPEED = 0.2f;

    public delegate void MoveToFinishedEvent();
    public static MoveToFinishedEvent moveToFinishedEvent;
    public static bool locked=false;

	public float speed;
	public float maxX;
	public float minX;
	public float maxY;
	public float minY;

    private float distance = 10.0f;
    private float height = 5.0f;
    private float heightDamping = 2.0f;
    private float roationDamping = 3.0f;
	private float dragSpeed = 0.3f;
	private float edgespeed = 6f;
    private float moveToTime = 0;
    private float offset = 0.01f;
    private float lerpTime;
    private Vector3 lastPosition;
    private Vector3 dragOrigin;
    private Vector3 oldPos;
    private Vector3 targetPosition;
    private Vector2 targetPos;
    private Vector3 delta;
    private Vector3 lerpTargetPos;
    private Transform target;
    private MainScript mainScript;
    private bool drag = false;

    void Start () {
        mainScript = FindObjectOfType<MainScript>();
        EventContainer.attackUIVisible += SetLocked;
        EventContainer.reactUIVisible += SetLocked;
    }
	
	void Update () {
        if (locked)
			return;

        #region DragCamera
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            lastPosition = Input.mousePosition;
            drag = true;
        }

        if (Input.GetMouseButton(0)&&drag)
        {
            delta  = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * Time.deltaTime*DRAG_SPEED,- delta.y * Time.deltaTime * DRAG_SPEED, 0);
            lastPosition = Input.mousePosition;
            lerpTargetPos = transform.localPosition + (-1 * delta.normalized);
        }
        if (Input.GetMouseButtonUp(0))
        {
            lerpTime = 0;
            drag = false;
        }
        #endregion

      

        #region SmoothSnapToGrid
        if (!drag)
        {
            lerpTime = Time.deltaTime / LERP_SPEED;
            if (targetPos.x != -1)
            {
                targetPosition = new Vector3(targetPos.x, targetPos.y, Mathf.Round(transform.localPosition.z));
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpTime);
                if(targetPos.x == transform.localPosition.x && targetPos.y == transform.localPosition.y)
                {
                    targetPos = new Vector2(-1,-1);
                }
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, lerpTargetPos, lerpTime);
            }
        }
        #endregion

        #region ClampCamera
        if (this.transform.localPosition.x < minX)
            this.transform.localPosition = new Vector3(minX, this.transform.localPosition.y, this.transform.localPosition.z);
        if (this.transform.localPosition.y < minY)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, minY, this.transform.localPosition.z);
        if (this.transform.localPosition.x > maxX)
            this.transform.localPosition = new Vector3(maxX, this.transform.localPosition.y, this.transform.localPosition.z);
        if (this.transform.localPosition.y > maxY)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, maxY, this.transform.localPosition.z);
        #endregion
    }
    public void MoveCameraTo( int x, int y)
    {
        int deltaX =(int)transform.localPosition.x - x;
        int deltaY = (int)transform.localPosition.y - y;
        int targetX = (int)transform.localPosition.x;
        int targetY = (int)transform.localPosition.y;
        if (x > (int)transform.localPosition.x + 5 || x < (int)transform.localPosition.x )  
        {
            targetX = -1 * (deltaX + 5);

        }
        if (y > (int)transform.localPosition.y + 7 || y < (int)transform.localPosition.y)
        {
            targetY = -1 * (deltaY + 7);
        }
        if (targetX < (int)minX)
            targetX = (int) minX;
        if (targetY < (int)minY)
            targetY = (int)minY;
        if (targetX > (int)maxX)
            targetX = (int)maxX;
        if (targetY > (int)maxY)
            targetY = (int)maxY;
        lerpTime = 0;
        targetPos = new Vector2(targetX, targetY);
    }
    void SetLocked(bool value)
    {
        locked = value;
    }
}
