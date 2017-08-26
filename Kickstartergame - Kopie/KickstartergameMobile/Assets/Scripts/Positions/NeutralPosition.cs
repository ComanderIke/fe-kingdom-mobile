using UnityEngine;
using System.Collections.Generic;

public class NeutralPosition : MonoBehaviour {

	public int startroom;
	public List<ItemEnum> inventory;
    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public int GetX()
    {
        return (int)transform.localPosition.x;
    }
    public int GetZ()
    {
        return (int)transform.localPosition.z;
    }
	

    // Update is called once per frame
    void Update () {
	
	}
}
