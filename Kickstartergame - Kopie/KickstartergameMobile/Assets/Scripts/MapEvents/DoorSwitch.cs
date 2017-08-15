using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorSwitch : MonoBehaviour {

    public bool activated = false;
	// Use this for initialization
	void Start () {
	
	}
    //private Vector3 oldPosition;
	public void Activate()
    {
        first = true;
        if (!activated)
        {
            activated = true;
            Door d = GetComponent<Door>();
            d.Open();
        }

    }
    bool shown = false;
    float time = 0;
    bool first = true;
    float time2 = 0;
    void Update()
    {

    }
    public int getX()
    {
        return (int)(transform.position.x - 0.5f+1);
    }
    public int getZ()
    {
        return (int)(transform.position.z - 0.5f);
    }
}
