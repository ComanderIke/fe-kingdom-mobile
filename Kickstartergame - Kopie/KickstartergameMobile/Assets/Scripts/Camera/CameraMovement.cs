using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

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
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta  = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * Time.deltaTime,- delta.y * Time.deltaTime, 0);
            lastPosition = Input.mousePosition;
        }

        if (this.transform.localPosition.x < minX)
            this.transform.localPosition = new Vector3(minX, this.transform.localPosition.y, this.transform.localPosition.z);
        if (this.transform.localPosition.y < minY)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, minY, this.transform.localPosition.z);
        if (this.transform.localPosition.x > maxX)
            this.transform.localPosition = new Vector3(maxX, this.transform.localPosition.y, this.transform.localPosition.z);
        if (this.transform.localPosition.y > maxY)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, maxY, this.transform.localPosition.z);

    }
}
