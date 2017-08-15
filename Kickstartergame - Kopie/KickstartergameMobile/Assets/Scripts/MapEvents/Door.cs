using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public bool opened = false;
    //private bool isOpenAnimationPlaying = false;
    //private bool isCloseAnimationPlaying = false;
    private bool init = true;
    // Use this for initialization
    void Start () {
		
	}
    public int getX()
    {
        return (int)(transform.position.x - 0.5f+1);
    }
    public int getZ()
    {
        return (int)(transform.position.z - 0.5f);
    }
    public void Open()
	{
		GetComponent<Animator> ().SetTrigger ("OpenDoor");
		Debug.Log ("Open DOor");
      opened = true;
      //isOpenAnimationPlaying = true;
    }
    public void Close()
    {
        GetComponent<MeshRenderer>().enabled = true;
        opened = false;
        //isCloseAnimationPlaying = true;
    }

    // Update is called once per frame
    float time = 0;
    void Update()
    {
        if (init)
        {
            init = false;
            //SendMessageUpwards("PlaceDoor", this);
        }
        //if (isOpenAnimationPlaying)
        //{
        //    time += Time.deltaTime;
        //    Transform t = GetComponentInChildren<MeshRenderer>().transform;
        //    t.position = new Vector3(t.position.x, t.position.y-1.8f*Time.deltaTime,t.position.z);
        //    if(time > 2.5)
        //    {
        //        isOpenAnimationPlaying = false;
        //        time = 0;
        //    }
        //    //Animation
        //}
        //if (isCloseAnimationPlaying)
        //{
        //    time += Time.deltaTime;
        //    gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.8f * Time.deltaTime, gameObject.transform.position.z);
        //    if (time > 2.5)
        //    {
        //        time = 0;
        //        isCloseAnimationPlaying = false;
        //    }
        //    //Animation
        //}
    }
}
