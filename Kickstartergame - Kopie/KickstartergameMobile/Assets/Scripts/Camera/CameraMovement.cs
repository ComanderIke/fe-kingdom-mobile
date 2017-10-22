using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {
    const float LERP_SPEED = 0.1f;
    public delegate void MoveToFinishedEvent();
    public static MoveToFinishedEvent moveToFinishedEvent;
    public static bool locked=false;
	public float speed;
	public float maxX;
	public float minX;
	public float maxY;
	public float minY;
    private float distance = 10.0f;
    private Transform target;
    float height = 5.0f;
    float heightDamping = 2.0f;
    float roationDamping = 3.0f;
	private float dragSpeed = 0.3f;
	private Vector3 dragOrigin;
	private Vector3 oldPos;
	private float edgespeed = 6f;
    float moveToTime = 0;
    float offset = 0.01f;
    private Vector3 lastPosition;
    MainScript mainScript;
    bool drag = false;
    private Vector3 targetPosition;
    private float lerpTime;

    // Use this for initialization
    void Start () {
        mainScript = FindObjectOfType<MainScript>();
    }
	
	// Update is called once per frame
	void Update () {

        if (locked)
			return;
		
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider.gameObject.tag == "Grid")
            {
                lastPosition = Input.mousePosition;
                drag = true;
            }
        }

        if (Input.GetMouseButton(0)&&drag)
        {
            Vector3 delta  = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * Time.deltaTime,- delta.y * Time.deltaTime, 0);
            lastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
            drag = false;
        if (this.transform.localPosition.x < minX)
            this.transform.localPosition = new Vector3(minX, this.transform.localPosition.y, this.transform.localPosition.z);
        if (this.transform.localPosition.y < minY)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, minY, this.transform.localPosition.z);
        if (this.transform.localPosition.x > maxX)
            this.transform.localPosition = new Vector3(maxX, this.transform.localPosition.y, this.transform.localPosition.z);
        if (this.transform.localPosition.y > maxY)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, maxY, this.transform.localPosition.z);
        if (!drag)
        {
            lerpTime = Time.deltaTime / LERP_SPEED;
            targetPosition = new Vector3(Mathf.Round(this.transform.localPosition.x), Mathf.Round(this.transform.localPosition.y), Mathf.Round(this.transform.localPosition.z));
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetPosition, lerpTime);
        }
    }
}
