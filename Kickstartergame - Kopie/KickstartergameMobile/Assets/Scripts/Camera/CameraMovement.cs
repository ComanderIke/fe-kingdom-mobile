using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CameraMovement : MonoBehaviour {

    public delegate void MoveToFinishedEvent();
    public static MoveToFinishedEvent moveToFinishedEvent;
    public static bool locked=false;
	public float speed;
	public float maxX;
	public float minX;
	public float maxZ;
	public float minZ;
    private float distance = 10.0f;
    private Transform target;
    float height = 5.0f;
    float heightDamping = 2.0f;
    float roationDamping = 3.0f;
	private float dragSpeed = 0.3f;
	private Vector3 dragOrigin;
	private Vector3 oldPos;
	private float edgespeed = 6f;
    public GameObject gameOverFade;
    float moveToTime = 0;
    float offset = 0.01f;
    public float mouseSensitivity = 0.0003f;
    private Vector3 lastPosition;
    MainScript mainScript;
 
    // Use this for initialization
    void Start () {
        mainScript = FindObjectOfType<MainScript>();
    }
	
	// Update is called once per frame
	void Update () {

        if (desaturate)
        {
            satTimeCurrent += Time.deltaTime / satTime;

            float sat = Mathf.Lerp(currentSaturation, desaturation, satTimeCurrent);
            GameObject.Find("Main Camera").GetComponent<ColorCorrectionCurves>().saturation = sat;
            if (satTimeCurrent >= 1)
                desaturate = false;
        }
        if (locked)
			return;
        if (moveTo)
        {
            Vector3 dir=new Vector3();
            moveToTime += Time.deltaTime;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, moveToPosition, ref dir, 0.1f);
            if (Mathf.Abs(transform.localPosition.x-moveToPosition.x)<offset&& Mathf.Abs(transform.localPosition.y- moveToPosition.y) < offset&&Mathf.Abs(transform.localPosition.z - moveToPosition.z) < offset)
            {
                if (moveToFinishedEvent != null)
                    moveToFinishedEvent();
                Debug.Log(transform.localPosition + " " + moveToPosition);
                moveTo = false;
                moveToTime = 0;
            }
            return;
        }
       
        mainScript.movingCam = false;
		/*if (this.transform.localPosition.x < minX)
			this.transform.localPosition = new Vector3 (minX, this.transform.localPosition.y, this.transform.localPosition.z);
		if (this.transform.localPosition.z < minZ)
			this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, minZ);
		if (this.transform.localPosition.x > maxX)
			this.transform.localPosition = new Vector3 (maxX, this.transform.localPosition.y, this.transform.localPosition.z);
		if (this.transform.localPosition.z > maxZ)
			this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, maxZ);*/

		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (new Vector3(1,0,0) * edgespeed * Time.deltaTime, Space.Self);
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (new Vector3(-1,0,0) * edgespeed * Time.deltaTime, Space.Self);
		}
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (new Vector3(0,0,1) * edgespeed * Time.deltaTime, Space.Self);
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (new Vector3(0,0,-1) * edgespeed * Time.deltaTime, Space.Self);
		}
        if (Input.GetMouseButtonDown(2))
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta  = Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * mouseSensitivity*Time.deltaTime,- delta.y * mouseSensitivity* Time.deltaTime, 0);
            lastPosition = Input.mousePosition;
        }

        if (follow) {
			followtime += Time.deltaTime;
			float x = Mathf.Lerp (startpos.x, -followTrans.position.x,followtime / 1);
			//Debug.Log (x);
			float z= Mathf.Lerp (startpos.z, -followTrans.position.z,followtime / 1);
			//Debug.Log (z);
			float orthosize= Mathf.Lerp(orthostart,orthogoal,followtime/2);
			GameObject.Find ("Main Camera").GetComponent<Camera> ().orthographicSize = orthosize;
			transform.localPosition = new Vector3 (x, transform.localPosition.y, z);
			saturation = Mathf.Lerp (1.25f, 0, followtime / 2);
			GameObject.Find ("Main Camera").GetComponent<ColorCorrectionCurves> ().saturation = saturation;
			alpha = Mathf.Lerp (0, 1, followtime/2.8f);
            gameOverFade.SetActive(true);
            gameOverFade.GetComponent<Image>().color = new Color (0, 0, 0, alpha);
			if (followtime >= 2.8f) {
				Application.LoadLevel ("BackgroundMenu");
			}
			if (followtime >= 0.5f) {
				if(followtime <=2)
				gameovertextalpha = Mathf.Lerp (0, 0.8f, (followtime - 0.5f) / 1.5f);
				else
					gameovertextalpha = Mathf.Lerp (0.8f, 0, (followtime - 2f) / 0.8f);
				GameObject.Find ("GameOver").GetComponent<Text> ().enabled = true;
				Color c = GameObject.Find ("GameOver").GetComponent<Text> ().color;
				GameObject.Find ("GameOver").GetComponent<Text> ().color = new Color (c.r, c.g, c.b, gameovertextalpha);
				gameOverY = Mathf.Lerp (700.0f, 850.0f, (followtime - 0.5f) / 2);
				GameObject.Find ("GameOver").transform.position= new Vector3(GameObject.Find ("GameOver").transform.position.x,gameOverY,GameObject.Find ("GameOver").transform.position.z);
			}

		}
        
	}
    static float currentSaturation = 0;
    static float satTimeCurrent = 0;
    static bool moveTo = false;
    static Vector3 moveToPosition;
	static float gameovertextalpha=0;
	static float followtime=0;
	static float orthogoal = 2;
	static float gameOverY=650;
	static Transform followTrans;
	static bool follow=false;
	static Vector3 startpos;
	static float alpha = 0;
	static float saturation = 1.25f;
				static float orthostart;
    static float satTime = 0;
    static float desaturation=0;
    static bool desaturate = false;
    public static void Desaturate(float time, float saturation)
    {
        satTime = time;
        currentSaturation = GameObject.Find("Main Camera").GetComponent<ColorCorrectionCurves>().saturation;
        desaturation = saturation;
        desaturate = true;
        satTimeCurrent = 0;
    }
    public static void MoveTo(Vector3 position)
    {
        moveToPosition = position;
        moveTo = true;

    }
	public static void Follow(Transform transformfollow){
        if (follow)
            return;
		Debug.Log ("FOLLO" +transformfollow.position);
		followTrans = transformfollow;
		follow = true;
		followtime = 0;
		alpha = 0;
		saturation = 1.25f;
		orthostart = GameObject.Find ("Main Camera").GetComponent<Camera> ().orthographicSize;
		Debug.Log ("orthostart:" + orthostart);
		startpos = GameObject.Find ("Main Camera").transform.parent.transform.localPosition;
		Debug.Log (startpos);

	}
}
